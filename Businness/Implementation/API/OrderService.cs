using Businness.Application;
using Businness.Interface.API;
using DataAccess.Repository.Base;
using Entities.Entity;
using Entities.Enums;
using Entities.ResponseModel;
using Microsoft.EntityFrameworkCore;

namespace Businness.Implementation.API
{
    public class OrderService : IOrderService
    {
        private readonly IRepositoryBase<Order> _orderRepo;
        private readonly IRepositoryBase<OrderDetail> _orderDetailRepo;
        private readonly IRepositoryBase<Customer> _customerRepo;
        private readonly IRepositoryBase<CustomerAddress> _addressRepo;
        private readonly IRepositoryBase<Stock> _stockRepo;
        private readonly IUnitOfWork _uow;

        public OrderService(
            IRepositoryBase<Order> orderRepo,
            IRepositoryBase<OrderDetail> orderDetailRepo,
            IRepositoryBase<Customer> customerRepo,
            IRepositoryBase<CustomerAddress> addressRepo,
            IRepositoryBase<Stock> stockRepo,
            IUnitOfWork uow)
        {
            _orderRepo = orderRepo;
            _orderDetailRepo = orderDetailRepo;
            _customerRepo = customerRepo;
            _addressRepo = addressRepo;
            _stockRepo = stockRepo;
            _uow = uow;
        }

        public async Task<BaseResponse<List<OrderListItem>>> GetAllAsync(bool onlyActives)
        {
            var resp = new BaseResponse<List<OrderListItem>>();
            try
            {
                var query = _orderRepo.Query(asNoTracking: true);
                if (onlyActives)
                {
                    query = query.Where(o => o.IsActive);
                }
                var orders = await query
                    .Select(o => new OrderListItem
                    {
                        Id = o.Id,
                        OrderNo = o.OrderNo,
                        CustomerName = o.Customer.CustomerName,
                        OrderDate = o.OrderDate,
                        TotalPrice = o.TotalPrice,
                        Status = o.Status,
                        Tax = o.Tax
                    })
                    .ToListAsync();

                if (orders.Count == 0)
                {
                    resp.Success = false;
                    resp.Response = "Sipariş bulunamadı.";
                    return resp;
                }

                resp.Success = true;
                resp.Data = orders;
            }
            catch (Exception ex)
            {
                resp.Success = false;
                resp.Response = "Siparişler alınırken bir hata oluştu.";
                resp.Errors.Add(ex.Message);
            }

            return resp;
        }

        public async Task<BaseResponse<OrderDetailItem>> GetByIdAsync(int id)
        {
            var resp = new BaseResponse<OrderDetailItem>();
            try
            {
                if (id <= 0)
                {
                    resp.Success = false;
                    resp.Response = "Geçersiz sipariş Id.";
                    return resp;
                }

                var order = await _orderRepo
                    .Search(o => o.Id == id, asNoTracking: true)
                    .Select(o => new OrderDetailItem
                    {
                        Id = o.Id,
                        OrderNo = o.OrderNo,
                        CustomerId = o.CustomerId,
                        CustomerName = o.Customer.CustomerName,
                        OrderDate = o.OrderDate,
                        TotalPrice = o.TotalPrice,
                        Status = o.Status,
                        Tax = o.Tax,
                        DeliveryAddressId = o.DeliveryAddressId,
                        InvoiceAddressId = o.InvoiceAddressId,
                        //CustomerAddresses = o.Customer.Addresses
                        //.Select(a => new CustomerAddressItem
                        //{
                        //    Id = a.Id,
                        //    CustomerId = a.CustomerId,
                        //    AdresType = a.AdresType,
                        //    Country = a.Country,
                        //    City = a.City,
                        //    Town = a.Town,
                        //    Address = a.Address,
                        //    Email = a.Email,
                        //    Phone = a.Phone,
                        //    PostalCode = a.PostalCode,
                        //    //IsActive = a.IsActive
                        //}).ToList(),
                        DeliveryAddressText = o.DeliveryAddress != null
                            ? o.DeliveryAddress.Address
                            : null,
                        InvoiceAddressText = o.InvoiceAddress != null
                            ? o.InvoiceAddress.Address
                            : null,
                        Lines = o.OrderDetails.Select(d => new OrderDetailLineItem
                        {
                            StockId = d.StockId,
                            StockName = d.Stock.StockName,
                            Amount = d.Amount,
                            UnitPrice = d.Stock.Price
                        }).ToList()
                    })
                    .FirstOrDefaultAsync();

                if (order == null)
                {
                    resp.Success = false;
                    resp.Response = "Sipariş bulunamadı.";
                    return resp;
                }

                resp.Success = true;
                resp.Data = order;
            }
            catch (Exception ex)
            {
                resp.Success = false;
                resp.Response = "Sipariş detayları alınırken bir hata oluştu.";
                resp.Errors.Add(ex.Message);
            }

            return resp;
        }

        public async Task<BaseResponse<int>> CreateAsync(OrderCreateRequest dto)
        {
            var resp = new BaseResponse<int>();

            try
            {
                if (dto == null ||
                    dto.CustomerId <= 0 ||
                    string.IsNullOrWhiteSpace(dto.OrderNo))
                {
                    resp.Success = false;
                    resp.Response = "Sipariş bilgileri eksik veya hatalı.";
                    return resp;
                }

                // müşteri kontrolu yap
                var customer = await _customerRepo.GetByIdAsync(dto.CustomerId);
                if (customer == null)
                {
                    resp.Success = false;
                    resp.Response = "Seçilen müşteri bulunamadı.";
                    return resp;
                }

                // teslimat / fatura adresi müşteriyle ilişkili mi? varmi yokmu onu da kontrol ediyorz tabi
                if (dto.DeliveryAddressId.HasValue)
                {
                    var del = await _addressRepo.GetByIdAsync(dto.DeliveryAddressId.Value);
                    if (del == null || del.CustomerId != dto.CustomerId)
                    {
                        resp.Success = false;
                        resp.Response = "Teslimat adresi bulunamadı veya müşteriye ait değil.";
                        return resp;
                    }
                }
                if (dto.Tax > 100)
                {
                    resp.Success = false;
                    resp.Response = "Vergi orani 100'den büyük olamaz.";
                    return resp;
                }
                if (dto.InvoiceAddressId.HasValue)
                {
                    var inv = await _addressRepo.GetByIdAsync(dto.InvoiceAddressId.Value);
                    if (inv == null || inv.CustomerId != dto.CustomerId)
                    {
                        resp.Success = false;
                        resp.Response = "Fatura adresi bulunamadı veya müşteriye ait değil.";
                        return resp;
                    }
                }

                // kalem kontrolleri
                if (dto.Lines == null || dto.Lines.Count == 0)
                {
                    resp.Success = false;
                    resp.Response = "Sipariş kalemi olmadan sipariş oluşturulamaz.";
                    return resp;
                }

                foreach (var line in dto.Lines)
                {
                    if (line.StockId <= 0 || line.Amount <= 0)
                    {
                        resp.Success = false;
                        resp.Response = "Geçersiz sipariş kalemi bilgisi.";
                        return resp;
                    }
                    var isStockExists = await _stockRepo.GetByIdAsync(line.StockId);
                    if (isStockExists == null)
                    {
                        resp.Success = false;
                        resp.Response = $"StockId {line.StockId} için ürün bulunamadı.";
                        return resp;
                    }

                }
                var order = new Order
                {
                    CustomerId = dto.CustomerId,
                    DeliveryAddressId = dto.DeliveryAddressId,
                    InvoiceAddressId = dto.InvoiceAddressId,
                    OrderNo = dto.OrderNo,
                    OrderDate = DateTime.UtcNow,
                    Status = dto.Status,
                    Tax = dto.Tax
                };

                decimal total = 0m;

                foreach (var line in dto.Lines)
                {
                    if (line.StockId <= 0 || line.Amount <= 0)
                        continue;

                    var stock = await _stockRepo.Query(asNoTracking: true)
                                                .FirstOrDefaultAsync(s => s.Id == line.StockId);
                    if (stock == null)
                    {
                        resp.Errors.Add($"StockId {line.StockId} için ürün bulunamadı. Kalem atlandı.");
                        continue;
                    }

                    var detail = new OrderDetail
                    {
                        StockId = line.StockId,
                        Amount = line.Amount,
                        IsActive = true
                    };

                    order.OrderDetails.Add(detail);

                    total += stock.Price * line.Amount;
                }

                if (order.OrderDetails.Count == 0)
                {
                    resp.Success = false;
                    resp.Response = "Geçerli sipariş kalemi bulunamadığı için sipariş oluşturulamadı.";
                    return resp;
                }
                if (dto.Tax > 0)
                {
                    order.TotalPrice = total + ((total * dto.Tax) / 100);
                }
                else
                {
                    order.TotalPrice = total;
                }
                await _orderRepo.AddAsync(order);
                await _uow.SaveChangesAsync();

                resp.Success = true;
                resp.Response = "Sipariş başarıyla oluşturuldu.";
                resp.Data = order.Id;
            }
            catch (Exception ex)
            {
                resp.Success = false;
                resp.Response = "Sipariş oluşturulurken bir hata oluştu.";
                resp.Errors.Add(ex.Message);
            }

            return resp;
        }
        public async Task<BaseResponse<int>> UpdateStatusAsync(OrderStatusUpdateRequest dto)
        {
            var resp = new BaseResponse<int>();
            try
            {
                if (dto == null || dto.Id <= 0)
                {
                    resp.Success = false;
                    resp.Response = "Geçersiz istek.";
                    return resp;
                }

                var order = await _orderRepo.Query().FirstOrDefaultAsync(o => o.Id == dto.Id);
                if (order == null)
                {
                    resp.Success = false;
                    resp.Response = "Sipariş bulunamadı.";
                    return resp;
                }

                if (order.Status == OrderStatus.OrderDelivered || order.Status == OrderStatus.OrderInTransit)
                {
                    resp.Success = false;
                    resp.Response = "Teslim edilmiş veya sevkiyatta olan sipariş güncellenemez.";
                    return resp;
                }

                order.Status = dto.Status;

                await _uow.SaveChangesAsync();

                resp.Success = true;
                resp.Response = "Sipariş durumu güncellendi.";
                resp.Data = order.Id;
            }
            catch (Exception ex)
            {
                resp.Success = false;
                resp.Response = "Durum güncellenirken hata oluştu.";
                resp.Errors.Add(ex.Message);
            }
            return resp;
        }
        public async Task<BaseResponse<int>> UpdateAsync(OrderUpdateRequest dto)
        {
            var resp = new BaseResponse<int>();

            try
            {
                if (dto == null || dto.Id <= 0)
                {
                    resp.Success = false;
                    resp.Response = "Geçersiz güncelleme isteği.";
                    return resp;
                }

                if (dto.CustomerId <= 0 || string.IsNullOrWhiteSpace(dto.OrderNo))
                {
                    resp.Success = false;
                    resp.Response = "Sipariş bilgileri eksik veya hatalı.";
                    return resp;
                }

                // siparis kontroll
                var order = await _orderRepo
                    .Query()
                    .Include(o => o.OrderDetails)
                    .FirstOrDefaultAsync(o => o.Id == dto.Id);

                if (order == null)
                {
                    resp.Success = false;
                    resp.Response = "Güncellenecek sipariş bulunamadı.";
                    return resp;
                }

                if (order.Status == OrderStatus.OrderDelivered || order.Status == OrderStatus.OrderInTransit)
                {
                    resp.Success = false;
                    resp.Response = "Teslim edilmiş veya sevkiyatta olan sipariş güncellenemez.";
                    return resp;
                }

                // musteri kontrolu
                var customer = await _customerRepo.GetByIdAsync(dto.CustomerId);
                if (customer == null)
                {
                    resp.Success = false;
                    resp.Response = "Seçilen müşteri bulunamadı.";
                    return resp;
                }

                // adres kontrolu --- musteriye ait olmasi lazim
                if (dto.DeliveryAddressId.HasValue)
                {
                    var del = await _addressRepo.GetByIdAsync(dto.DeliveryAddressId.Value);
                    if (del == null || del.CustomerId != dto.CustomerId)
                    {
                        resp.Success = false;
                        resp.Response = "Teslimat adresi bulunamadı veya müşteriye ait değil.";
                        return resp;
                    }
                }

                if (dto.InvoiceAddressId.HasValue)
                {
                    var inv = await _addressRepo.GetByIdAsync(dto.InvoiceAddressId.Value);
                    if (inv == null || inv.CustomerId != dto.CustomerId)
                    {
                        resp.Success = false;
                        resp.Response = "Fatura adresi bulunamadı veya müşteriye ait değil.";
                        return resp;
                    }
                }

                // kalem dogrulamalari
                if (dto.Lines == null || dto.Lines.Count == 0)
                {
                    resp.Success = false;
                    resp.Response = "En az bir geçerli sipariş kalemi gereklidir.";
                    return resp;
                }
                foreach (var line in dto.Lines)
                {
                    if (line.StockId <= 0 || line.Amount <= 0)
                    {
                        resp.Success = false;
                        resp.Response = "Geçersiz sipariş kalemi bilgisi.";
                        return resp;
                    }
                    var isStockExists = await _stockRepo.GetByIdAsync(line.StockId);
                    if (isStockExists == null)
                    {
                        resp.Success = false;
                        resp.Response = $"StockId {line.StockId} için ürün bulunamadı.";
                        return resp;
                    }

                }

                order.CustomerId = dto.CustomerId;
                order.DeliveryAddressId = dto.DeliveryAddressId;
                order.InvoiceAddressId = dto.InvoiceAddressId;
                order.OrderNo = dto.OrderNo;
                order.Status = dto.Status;
                order.Tax = dto.Tax;


                var incomingValidLines = dto.Lines
                    .Where(l => l.StockId > 0 && l.Amount > 0)
                    .ToList();

                if (incomingValidLines.Count == 0)
                {
                    resp.Success = false;
                    resp.Response = "Geçerli sipariş kalemi bulunamadı.";
                    return resp;
                }

                // stoklari toplu cekmeyi dendim (performansli oluyor boyle)
                var incomingStockIds = incomingValidLines.Select(l => l.StockId).Distinct().ToList();
                var stocks = await _stockRepo.Query(asNoTracking: true)
                                             .Where(s => incomingStockIds.Contains(s.Id))
                                             .ToListAsync();

                var stockMap = stocks.ToDictionary(s => s.Id, s => s);

                var existingByStockId = order.OrderDetails
                                             .ToDictionary(d => d.StockId, d => d);

                foreach (var line in dto.Lines)
                {
                    if (!stockMap.TryGetValue(line.StockId, out var stock))
                    {
                        resp.Errors.Add($"StockId {line.StockId} için ürün bulunamadı. Kalem atlandı.");
                        continue;
                    }

                    if (existingByStockId.TryGetValue(line.StockId, out var existingDetail))
                    {
                        existingDetail.Amount = line.Amount;
                        existingDetail.IsActive = true;
                    }
                    else
                    {
                        var newDetail = new OrderDetail
                        {
                            StockId = line.StockId,
                            Amount = line.Amount,
                            IsActive = true
                        };
                        order.OrderDetails.Add(newDetail);
                    }
                }

                var dtoStockIdSet = incomingValidLines.Select(l => l.StockId).ToHashSet();
                foreach (var detail in order.OrderDetails)
                {
                    if (!dtoStockIdSet.Contains(detail.StockId))
                    {
                        detail.IsActive = false;
                    }
                }

                decimal total = 0m;

                var activeDetails = order.OrderDetails.Where(d => d.IsActive).ToList();
                if (activeDetails.Count == 0)
                {
                    resp.Success = false;
                    resp.Response = "Tüm kalemler pasif olduğundan sipariş güncellenemedi.";
                    return resp;
                }

                var activeStockIds = activeDetails.Select(d => d.StockId).Distinct().ToList();
                var missingStockIds = activeStockIds.Where(id => !stockMap.ContainsKey(id)).ToList();
                if (missingStockIds.Count > 0)
                {
                    var missingStocks = await _stockRepo.Query(asNoTracking: true)
                                                        .Where(s => missingStockIds.Contains(s.Id))
                                                        .ToListAsync();
                    foreach (var s in missingStocks)
                        stockMap[s.Id] = s;
                }

                foreach (var d in activeDetails)
                {
                    if (!stockMap.TryGetValue(d.StockId, out var st))
                    {
                        // burasi bi tik istisnai, hic denk gelmedim testlerde ama kafama takilinca nolur nolmaz ekledim.
                        resp.Errors.Add($"StockId {d.StockId} için fiyat bulunamadı. Kalem toplam dışı bırakıldı.");
                        continue;
                    }
                    total += st.Price * d.Amount;
                }
                if (dto.Tax > 0)
                {
                    order.TotalPrice = total + ((total * dto.Tax) / 100);
                }
                else
                {
                    order.TotalPrice = total;
                }

                await _uow.SaveChangesAsync();

                resp.Success = true;
                resp.Response = "Sipariş başarıyla güncellendi.";
                resp.Data = order.Id;
            }
            catch (Exception ex)
            {
                resp.Success = false;
                resp.Response = "Sipariş güncellenirken bir hata oluştu.";
                resp.Errors.Add(ex.Message);
            }

            return resp;
        }


        public async Task<BaseResponse<bool>> DeleteAsync(int id)
        {
            var resp = new BaseResponse<bool>();
            try
            {
                if (id <= 0)
                {
                    resp.Success = false;
                    resp.Response = "Geçersiz sipariş Id.";
                    return resp;
                }

                var order = await _orderRepo.GetByIdAsync(id);
                if (order == null)
                {
                    resp.Success = false;
                    resp.Response = "Silinecek sipariş bulunamadı.";
                    return resp;
                }
                if (order.Status == OrderStatus.OrderDelivered || order.Status == OrderStatus.OrderInTransit)
                {
                    resp.Success = false;
                    resp.Response = "Teslim edilmiş veya sevkiyatta olan sipariş güncellenemez.";
                    return resp;
                }
                _orderRepo.Delete(order);
                await _uow.SaveChangesAsync();

                resp.Success = true;
                resp.Data = true;
                resp.Response = "Sipariş silindi.";
            }
            //order silinince detaylarinin silinmesi problem degil normalde ama fk constraint tehlikesi var yine de
            catch (DbUpdateException dbEx)
            {
                resp.Success = false;
                resp.Response = "Sipariş ilişkili kayıtlar nedeniyle silinemedi.";
                resp.Errors.Add(dbEx.Message);
            }
            catch (Exception ex)
            {
                resp.Success = false;
                resp.Response = "Sipariş silinirken bir hata oluştu.";
                resp.Errors.Add(ex.Message);
            }

            return resp;
        }

        public async Task<BaseResponse<List<CustomerOrderModel>>> GetOrdersWithMinQuantityAsync(decimal minAmount)
        {
            var resp = new BaseResponse<List<CustomerOrderModel>>();
            try
            {
                var orderIds = await _orderDetailRepo
                    .Search(od => od.Amount > minAmount && od.IsActive, asNoTracking: true)
                    .Select(od => od.OrderId)
                    .Distinct()
                    .ToListAsync();

                if (orderIds.Count == 0)
                {
                    resp.Success = false;
                    resp.Response = "Kriteri sağlayan sipariş bulunamadı.";
                    return resp;
                }

                var orders = await _orderRepo
                    .Query(asNoTracking: true)
                    .Where(o => orderIds.Contains(o.Id))
                    .Include(o => o.Customer)
                    .Include(o => o.OrderDetails)
                        .ThenInclude(od => od.Stock)
                    .ToListAsync();

                resp.Success = true;
                resp.Data = orders.Select(o => new CustomerOrderModel
                {
                    CustomerId = o.CustomerId,
                    CustomerName = o.Customer.CustomerName,
                    OrderId = o.Id,
                    OrderNo = o.OrderNo,
                    OrderDate = o.OrderDate,
                    Status = o.Status,
                    TotalPrice = o.TotalPrice,
                    Lines = o.OrderDetails.Select(od => new OrderDetailLineItem
                    {
                        StockId = od.StockId,
                        StockName = od.Stock.StockName,
                        Amount = od.Amount,
                        UnitPrice = od.Stock.Price
                    }).ToList()
                }).ToList();
            }
            catch (Exception ex)
            {
                resp.Success = false;
                resp.Response = "Sorgu çalıştırılırken hata oluştu.";
                resp.Errors.Add(ex.Message);
            }

            return resp;
        }

        public async Task<BaseResponse<CityOrderCountModel>> GetOrderCountByCityAsync(string city)
        {
            var resp = new BaseResponse<CityOrderCountModel>();
            try
            {
                if (string.IsNullOrWhiteSpace(city))
                {
                    resp.Success = false;
                    resp.Response = "Şehir adı zorunludur.";
                    return resp;
                }

                var count = await _orderRepo
                    .Search(o => o.DeliveryAddress != null &&
                                o.DeliveryAddress.City != null &&
                                o.DeliveryAddress.City == city, asNoTracking: true)
                    .CountAsync();

                resp.Success = true;
                resp.Data = new CityOrderCountModel
                {
                    City = city,
                    OrderCount = count
                };
            }
            catch (Exception ex)
            {
                resp.Success = false;
                resp.Response = "Sorgu çalıştırılırken hata oluştu.";
                resp.Errors.Add(ex.Message);
            }

            return resp;
        }

    }

}
