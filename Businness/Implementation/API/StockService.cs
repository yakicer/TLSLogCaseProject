using Businness.Application;
using Businness.Interface.API;
using DataAccess.Repository.Base;
using Entities.Entity;
using Entities.ResponseModel;
using Microsoft.EntityFrameworkCore;

namespace Businness.Implementation.API
{


    public class StockService : IStockService
    {
        private readonly IRepositoryBase<Stock> _stockRepo;
        private readonly IRepositoryBase<OrderDetail> _orderDetailRepo;
        private readonly IRepositoryBase<Customer> _customerRepo;

        private readonly IUnitOfWork _uow;

        public StockService(IRepositoryBase<Stock> stockRepo, IUnitOfWork uow, IRepositoryBase<OrderDetail> orderDetailRepo, IRepositoryBase<Customer> customerRepo)
        {
            _stockRepo = stockRepo;
            _uow = uow;
            _orderDetailRepo = orderDetailRepo;
            _customerRepo = customerRepo;
        }

        public async Task<BaseResponse<List<StockListItem>>> GetAllAsync(bool onlyActives)
        {
            var resp = new BaseResponse<List<StockListItem>>();
            try
            {
                var query = _stockRepo.Query(asNoTracking: true);
                if (onlyActives)
                    query = query.Where(x => x.IsActive);

                var list = await query
                    .Select(x => new StockListItem
                    {
                        Id = x.Id,
                        StockName = x.StockName,
                        Unit = x.Unit,
                        Price = x.Price,
                        Barcode = x.Barcode,
                        IsActive = x.IsActive
                    })
                    .ToListAsync();

                if (list.Count == 0)
                {
                    resp.Success = false;
                    resp.Response = "Stok bulunamadı.";
                    return resp;
                }

                resp.Success = true;
                resp.Data = list;
            }
            catch (Exception ex)
            {
                resp.Success = false;
                resp.Response = "Stoklar alınırken bir hata oluştu.";
                resp.Errors.Add(ex.Message);
            }

            return resp;
        }

        public async Task<BaseResponse<StockDetailItem>> GetByIdAsync(int id)
        {
            var resp = new BaseResponse<StockDetailItem>();
            try
            {
                if (id <= 0)
                {
                    resp.Success = false;
                    resp.Response = "Geçersiz stok Id.";
                    return resp;
                }

                var entity = await _stockRepo.Query(asNoTracking: true)
                                             .FirstOrDefaultAsync(x => x.Id == id);

                if (entity == null)
                {
                    resp.Success = false;
                    resp.Response = "Stok bulunamadı.";
                    return resp;
                }

                resp.Success = true;
                resp.Data = new StockDetailItem
                {
                    Id = entity.Id,
                    StockName = entity.StockName,
                    Unit = entity.Unit,
                    Price = entity.Price,
                    Barcode = entity.Barcode,
                    IsActive = entity.IsActive
                };
            }
            catch (Exception ex)
            {
                resp.Success = false;
                resp.Response = "Stok bilgisi alınırken bir hata oluştu.";
                resp.Errors.Add(ex.Message);
            }

            return resp;
        }

        public async Task<BaseResponse<StockDetailItem>> CreateAsync(StockCreateRequest dto)
        {
            var resp = new BaseResponse<StockDetailItem>();
            try
            {
                if (dto == null || string.IsNullOrWhiteSpace(dto.StockName) || string.IsNullOrWhiteSpace(dto.Barcode))
                {
                    resp.Success = false;
                    resp.Response = "Stok adı ve barkod zorunludur.";
                    return resp;
                }

                // barkod unique istendigi icin kontrol et
                var exists = await _stockRepo
                    .Search(x => x.Barcode == dto.Barcode, asNoTracking: true)
                    .AnyAsync();

                if (exists)
                {
                    resp.Success = false;
                    resp.Response = "Bu barkod zaten kullanılıyor.";
                    return resp;
                }

                var entity = new Stock
                {
                    StockName = dto.StockName,
                    Unit = dto.Unit,
                    Price = dto.Price,
                    Barcode = dto.Barcode,
                    IsActive = true
                };

                await _stockRepo.AddAsync(entity);
                await _uow.SaveChangesAsync();

                resp.Success = true;
                resp.Response = "Stok eklendi.";
                resp.Data = new StockDetailItem
                {
                    Id = entity.Id,
                    StockName = entity.StockName,
                    Unit = entity.Unit,
                    Price = entity.Price,
                    Barcode = entity.Barcode,
                    IsActive = entity.IsActive
                };
            }
            catch (Exception ex)
            {
                resp.Success = false;
                resp.Response = "Stok eklenirken bir hata oluştu.";
                resp.Errors.Add(ex.Message);
            }

            return resp;
        }

        public async Task<BaseResponse<bool>> UpdateAsync(StockUpdateRequest dto)
        {
            var resp = new BaseResponse<bool>();
            try
            {
                if (dto == null || dto.Id <= 0)
                {
                    resp.Success = false;
                    resp.Response = "Geçersiz stok bilgisi.";
                    return resp;
                }

                var entity = await _stockRepo.GetByIdAsync(dto.Id);
                if (entity == null)
                {
                    resp.Success = false;
                    resp.Response = "Güncellenecek stok bulunamadı.";
                    return resp;
                }

                // barkod uniqe istednigi icin kontrol eklemek lazim
                if (!string.Equals(entity.Barcode, dto.Barcode, StringComparison.OrdinalIgnoreCase))
                {
                    var barcodeInUse = await _stockRepo
                        .Search(x => x.Barcode == dto.Barcode && x.Id != dto.Id, asNoTracking: true)
                        .AnyAsync();

                    if (barcodeInUse)
                    {
                        resp.Success = false;
                        resp.Response = "Bu barkod başka bir stokta kullanılıyor.";
                        return resp;
                    }
                }

                entity.StockName = dto.StockName;
                entity.Unit = dto.Unit;
                entity.Price = dto.Price;
                entity.Barcode = dto.Barcode;
                entity.IsActive = dto.IsActive;

                _stockRepo.Update(entity);
                await _uow.SaveChangesAsync();

                resp.Success = true;
                resp.Data = true;
                resp.Response = "Stok güncellendi.";
            }
            catch (Exception ex)
            {
                resp.Success = false;
                resp.Response = "Stok güncellenirken bir hata oluştu.";
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
                    resp.Response = "Geçersiz stok Id.";
                    return resp;
                }

                var entity = await _stockRepo.GetByIdAsync(id);
                if (entity == null)
                {
                    resp.Success = false;
                    resp.Response = "Silinecek stok bulunamadı.";
                    return resp;
                }
                // TODO: YAKUP BUNU UNUTMA!!!!!!!!!!!!!!
                // stok sipariste kullanilmis mi diye kontrol eklemeye gerek var mi bi dusun bunu
                // EDIT: gerek varmis...
                var isUsed = await _orderDetailRepo
                    .Query(asNoTracking: true)
                    .AnyAsync(d => d.StockId == id && d.IsActive);

                if (isUsed)
                {
                    resp.Success = false;
                    resp.Response = "Bu stok en az bir sipariş detayında kullanılıyor. Silinemez. Dilerseniz stok adedini 0'a çekebilirsiniz.";
                    return resp;
                }
                _stockRepo.Delete(entity);
                await _uow.SaveChangesAsync();

                resp.Success = true;
                resp.Data = true;
                resp.Response = "Stok silindi.";
            }
            catch (Exception ex)
            {
                resp.Success = false;
                resp.Response = "Stok silinirken bir hata oluştu.";
                resp.Errors.Add(ex.Message);
            }

            return resp;
        }
        public async Task<BaseResponse<List<CustomerDetailItem>>> GetCustomersWhoBoughtStockAsync(int stockId)
        {
            var resp = new BaseResponse<List<CustomerDetailItem>>();
            try
            {
                if (stockId <= 0)
                {
                    resp.Success = false;
                    resp.Response = "Geçersiz stok Id.";
                    return resp;
                }

                // stok hangi ordrelarda kontrol et
                var customerIds = await _orderDetailRepo
                    .Search(od => od.StockId == stockId && od.IsActive, asNoTracking: true)
                    .Select(od => od.Order.CustomerId)
                    .Distinct()
                    .ToListAsync();

                if (customerIds.Count == 0)
                {
                    resp.Success = false;
                    resp.Response = "Bu ürünü alan müşteri bulunamadı.";
                    return resp;
                }

                var customers = await _customerRepo
                    .Search(c => customerIds.Contains(c.Id), asNoTracking: true)
                    .Include(c => c.Addresses)
                    .ToListAsync();

                resp.Success = true;
                resp.Data = customers.Select(c => new CustomerDetailItem
                {
                    Id = c.Id,
                    CustomerName = c.CustomerName,
                    Addresses = c.Addresses
                        .Where(a => a.IsActive)
                        .Select(a => new CustomerAddressItem
                        {
                            Id = a.Id,
                            CustomerId = a.CustomerId,
                            AdresType = (int)a.AdresType,
                            Address = a.Address,
                            City = a.City,
                            Country = a.Country,
                            Town = a.Town,
                            Email = a.Email,
                            Phone = a.Phone,
                            PostalCode = a.PostalCode
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
    }

}
