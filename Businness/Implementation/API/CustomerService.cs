using Businness.Application;
using Businness.Interface.API;
using DataAccess.Repository.Base;
using Entities.Entity;
using Entities.Enums;
using Entities.ResponseModel;
using Microsoft.EntityFrameworkCore;

public class CustomerService : ICustomerService
{
    private readonly IRepositoryBase<Customer> _customerRepo;
    private readonly IRepositoryBase<CustomerAddress> _addressRepo;
    private readonly IRepositoryBase<Order> _orderRepo;
    private readonly IRepositoryBase<OrderDetail> _orderDetailRepo;
    private readonly IUnitOfWork _uow;

    public CustomerService(
        IRepositoryBase<Customer> customerRepo,
        IRepositoryBase<CustomerAddress> addressRepo,
        IRepositoryBase<Order> orderRepo,
        IRepositoryBase<OrderDetail> orderDetailRepo,
        IUnitOfWork uow)
    {
        _customerRepo = customerRepo;
        _addressRepo = addressRepo;
        _orderRepo = orderRepo;
        _orderDetailRepo = orderDetailRepo;
        _uow = uow;
    }

    public async Task<BaseResponse<List<CustomerListItem>>> GetAllAsync(bool onlyActives)
    {
        var resp = new BaseResponse<List<CustomerListItem>>();
        try
        {
            var query = _customerRepo.Query(asNoTracking: true);
            if (onlyActives)
            {
                query = query.Where(x => x.IsActive);
            }
            var customers = await query.Select(x => new CustomerListItem
            {
                Id = x.Id,
                CustomerName = x.CustomerName,
                IsActive = x.IsActive
            }).ToListAsync();

            if (customers.Count == 0)
            {
                resp.Success = false;
                resp.Response = "Müşteri bulunamadı.";
                return resp;
            }

            resp.Success = true;
            resp.Data = customers;
        }
        catch (Exception ex)
        {
            resp.Success = false;
            resp.Response = "Müşteriler alınırken hata oluştu.";
            resp.Errors.Add(ex.Message);
        }

        return resp;
    }
    public async Task<BaseResponse<CustomerDetailItem>> GetByIdAsync(int id)
    {
        var resp = new BaseResponse<CustomerDetailItem>();
        try
        {
            if (id <= 0)
            {
                resp.Success = false;
                resp.Response = "Geçersiz müşteri Id.";
                return resp;
            }
            var customer = await _customerRepo
                .Include(nameof(Customer.Addresses))
                .FirstOrDefaultAsync(x => x.Id == id);

            if (customer == null)
            {
                resp.Success = false;
                resp.Response = "Müşteri bulunamadı.";
                return resp;
            }

            var dto = new CustomerDetailItem
            {
                Id = customer.Id,
                CustomerName = customer.CustomerName,
                IsActive = customer.IsActive,
                Addresses = customer.Addresses.Select(a => new CustomerAddressItem
                {
                    Id = a.Id,
                    CustomerId = a.CustomerId,
                    AdresType = (int)a.AdresType,
                    Country = a.Country,
                    City = a.City,
                    Town = a.Town,
                    Address = a.Address,
                    Email = a.Email,
                    Phone = a.Phone,
                    PostalCode = a.PostalCode,
                    IsActive = a.IsActive
                }).ToList()
            };

            resp.Success = true;
            resp.Data = dto;
        }
        catch (Exception ex)
        {
            resp.Success = false;
            resp.Response = "Müşteri bilgisi alınırken hata oluştu.";
            resp.Errors.Add(ex.Message);
        }

        return resp;
    }

    public async Task<BaseResponse<CustomerDetailItem>> CreateAsync(CustomerCreateRequest model)
    {
        var resp = new BaseResponse<CustomerDetailItem>();
        try
        {
            if (model == null || string.IsNullOrWhiteSpace(model.CustomerName))
            {
                resp.Success = false;
                resp.Response = "Müşteri adı zorunludur.";
                return resp;
            }

            var entity = new Customer
            {
                CustomerName = model.CustomerName,
                IsActive = model.IsActive
            };

            await _customerRepo.AddAsync(entity);
            await _uow.SaveChangesAsync();

            resp.Success = true;
            resp.Response = "Müşteri eklendi.";
            resp.Data = new CustomerDetailItem
            {
                Id = entity.Id,
                CustomerName = entity.CustomerName,
                IsActive = entity.IsActive,
                Addresses = new List<CustomerAddressItem>()
            };
        }
        catch (Exception ex)
        {
            resp.Success = false;
            resp.Response = "Müşteri eklenirken hata oluştu.";
            resp.Errors.Add(ex.Message);
        }

        return resp;
    }

    public async Task<BaseResponse<bool>> UpdateAsync(CustomerUpdateRequest model)
    {
        var resp = new BaseResponse<bool>();
        try
        {
            var entity = await _customerRepo.GetByIdAsync(model.Id);
            if (entity == null)
            {
                resp.Success = false;
                resp.Response = "Müşteri bulunamadı.";
                return resp;
            }

            entity.CustomerName = model.CustomerName;
            entity.IsActive = model.IsActive;

            _customerRepo.Update(entity);
            await _uow.SaveChangesAsync();

            resp.Success = true;
            resp.Data = true;
            resp.Response = "Müşteri güncellendi.";
        }
        catch (Exception ex)
        {
            resp.Success = false;
            resp.Response = "Müşteri güncellenirken hata oluştu.";
            resp.Errors.Add(ex.Message);
        }

        return resp;
    }

    public async Task<BaseResponse<bool>> DeleteAsync(int id)
    {
        var resp = new BaseResponse<bool>();
        if (id <= 0)
        {
            resp.Success = false;
            resp.Response = "Geçersiz müşteri Id.";
            return resp;
        }
        try
        {
            var entity = await _customerRepo.GetByIdAsync(id);
            if (entity == null)
            {
                resp.Success = false;
                resp.Response = "Silinecek müşteri bulunamadı.";
                return resp;
            }
            //sql tarafinda fk constraint hatasindan patlamamak icin siparis kaydi var mi diye kontrol ediyorum
            var hasOrders = await _orderRepo
            .Query(asNoTracking: true)
            .AnyAsync(o => o.CustomerId == id);

            if (hasOrders)
            {
                resp.Success = false;
                resp.Response = "Bu müşteri için sipariş kaydı bulunduğundan müşteri silinemez.";
                return resp;
            }
            _customerRepo.Delete(entity);
            await _uow.SaveChangesAsync();

            resp.Success = true;
            resp.Data = true;
            resp.Response = "Müşteri silindi.";
        }
        catch (Exception ex)
        {
            resp.Success = false;
            resp.Response = "Müşteri silinirken hata oluştu.";
            resp.Errors.Add(ex.Message);
        }

        return resp;
    }

    public async Task<BaseResponse<CustomerAddressItem>> AddAddressAsync(CustomerAddressCreateRequest dto)
    {
        var resp = new BaseResponse<CustomerAddressItem>();
        try
        {
            if (dto == null || dto.CustomerId <= 0)
            {
                resp.Success = false;
                resp.Response = "Geçersiz adres bilgisi.";
                return resp;
            }

            // müşteri var mı
            var customer = await _customerRepo.GetByIdAsync(dto.CustomerId);
            if (customer == null)
            {
                resp.Success = false;
                resp.Response = "Adresi eklenecek müşteri bulunamadı.";
                return resp;
            }

            var entity = new CustomerAddress
            {
                CustomerId = dto.CustomerId,
                AdresType = (AddressType)dto.AdresType,
                Country = dto.Country,
                City = dto.City,
                Town = dto.Town,
                Address = dto.Address,
                Email = dto.Email,
                Phone = dto.Phone,
                PostalCode = dto.PostalCode,
                IsActive = true
            };

            await _addressRepo.AddAsync(entity);
            await _uow.SaveChangesAsync();

            resp.Success = true;
            resp.Response = "Adres eklendi.";
            resp.Data = new CustomerAddressItem
            {
                Id = entity.Id,
                CustomerId = entity.CustomerId,
                AdresType = (int)entity.AdresType,
                Country = entity.Country,
                City = entity.City,
                Town = entity.Town,
                Address = entity.Address,
                Email = entity.Email,
                Phone = entity.Phone,
                PostalCode = entity.PostalCode
            };
        }
        catch (Exception ex)
        {
            resp.Success = false;
            resp.Response = "Adres eklenirken hata oluştu.";
            resp.Errors.Add(ex.Message);
        }

        return resp;
    }


    public async Task<BaseResponse<List<CustomerAddressItem>>> GetAddressesAsync(int customerId)
    {
        var resp = new BaseResponse<List<CustomerAddressItem>>();
        if (customerId <= 0)
        {
            resp.Success = false;
            resp.Response = "Geçersiz müşteri Id.";
            return resp;
        }
        try
        {
            var customer = await _customerRepo.GetByIdAsync(customerId);
            if (customer == null)
            {
                resp.Success = false;
                resp.Response = "Müşteri bulunamadı.";
                return resp;
            }
            var addresses = await _addressRepo
                .Search(a => a.CustomerId == customerId && a.IsActive)
                .Select(a => new CustomerAddressItem
                {
                    Id = a.Id,
                    CustomerId = a.CustomerId,
                    AdresType = (int)a.AdresType,
                    Country = a.Country,
                    City = a.City,
                    Town = a.Town,
                    Address = a.Address,
                    Email = a.Email,
                    Phone = a.Phone,
                    PostalCode = a.PostalCode
                })
                .ToListAsync();
            if (addresses.Count == 0)
            {
                resp.Success = false;
                resp.Response = "Müşteriye ait adres bulunamadı.";
                return resp;
            }
            resp.Success = true;
            resp.Data = addresses;
        }
        catch (Exception ex)
        {
            resp.Success = false;
            resp.Response = "Adresler alınırken hata oluştu.";
            resp.Errors.Add(ex.Message);
        }

        return resp;
    }
    public async Task<BaseResponse<List<CustomerOrderModel>>> GetOrdersByCustomerNameAsync(string customerName)
    {
        var resp = new BaseResponse<List<CustomerOrderModel>>();
        try
        {
            if (string.IsNullOrWhiteSpace(customerName))
            {
                resp.Success = false;
                resp.Response = "Müşteri adı zorunludur.";
                return resp;
            }

            var orders = await _orderRepo
                .Search(o => o.Customer.CustomerName == customerName, asNoTracking: true)
                .Include(o => o.Customer)
                .Include(o => o.OrderDetails)
                    .ThenInclude(od => od.Stock)
                .ToListAsync();

            if (orders.Count == 0)
            {
                resp.Success = false;
                resp.Response = "Bu isimde müşterinin siparişi bulunamadı.";
                return resp;
            }

            resp.Success = true;
            resp.Data = orders.Select(o => new CustomerOrderModel
            {
                CustomerId = o.CustomerId,
                CustomerName = o.Customer.CustomerName,
                OrderId = o.Id,
                OrderNo = o.OrderNo,
                OrderDate = o.OrderDate,
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
    public async Task<BaseResponse<List<CustomerDetailItem>>> GetCustomersWithDifferentAddressesAsync()
    {
        var resp = new BaseResponse<List<CustomerDetailItem>>();
        try
        {

            var customerIds = await _orderRepo
                .Search(o => o.InvoiceAddressId != null
                         && o.DeliveryAddressId != null
                         && o.InvoiceAddressId != o.DeliveryAddressId, asNoTracking: true)
                .Select(o => o.CustomerId)
                .Distinct()
                .ToListAsync();

            if (customerIds.Count == 0)
            {
                resp.Success = false;
                resp.Response = "Farklı fatura/teslimat adresi olan müşteri bulunamadı.";
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
                        City = a.City,
                        Address = a.Address,
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
