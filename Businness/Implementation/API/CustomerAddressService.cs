using Businness.Application;
using Businness.Interface.API;
using DataAccess.Repository.Base;
using Entities.Entity;
using Entities.Enums;
using Entities.ResponseModel;
using Microsoft.EntityFrameworkCore;

namespace Businness.Implementation.API
{
    //Bu servisi bi bakima backoffice mantigi icin ekledim
    public class CustomerAddressService : ICustomerAddressService
    {
        private readonly IRepositoryBase<CustomerAddress> _addressRepo;
        private readonly IRepositoryBase<Customer> _customerRepo;
        private readonly IRepositoryBase<Order> _orderRepo;
        private readonly IUnitOfWork _uow;

        public CustomerAddressService(
            IRepositoryBase<CustomerAddress> addressRepo,
            IRepositoryBase<Customer> customerRepo,
            IRepositoryBase<Order> orderRepo,
            IUnitOfWork uow)
        {
            _addressRepo = addressRepo;
            _customerRepo = customerRepo;
            _orderRepo = orderRepo;
            _uow = uow;
        }

        public async Task<BaseResponse<List<CustomerAddressListItem>>> GetAllAddressesAsync(bool onlyActives)
        {
            var resp = new BaseResponse<List<CustomerAddressListItem>>();
            try
            {

                var query = _addressRepo.Query(asNoTracking: true);
                if (onlyActives)
                {
                    query = query.Where(x => x.IsActive);
                }
                var list = await query
                    .Select(x => new CustomerAddressListItem
                    {
                        Id = x.Id,
                        CustomerId = x.CustomerId,
                        AdresType = (int)x.AdresType,
                        Country = x.Country,
                        City = x.City,
                        Town = x.Town,
                        Address = x.Address,
                        IsActive = x.IsActive
                    })
                    .ToListAsync();

                if (list.Count == 0)
                {
                    resp.Success = false;
                    resp.Response = "Herhangi Bir Adres bulunamadı.";
                    return resp;
                }

                resp.Success = true;
                resp.Data = list;
            }
            catch (Exception ex)
            {
                resp.Success = false;
                resp.Response = "Adresler alınırken bir hata oluştu.";
                resp.Errors.Add(ex.Message);
            }

            return resp;
        }

        public async Task<BaseResponse<List<CustomerAddressDetailItem>>> GetAllAddressesDetailedAsync(bool onlyActives)
        {
            var resp = new BaseResponse<List<CustomerAddressDetailItem>>();
            try
            {

                var query = _addressRepo.Query(asNoTracking: true);
                if (onlyActives)
                {
                    query = query.Where(x => x.IsActive);
                }
                var list = await query
                    .Select(x => new CustomerAddressDetailItem
                    {
                        Id = x.Id,
                        CustomerId = x.CustomerId,
                        CustomerName = x.Customer != null ? x.Customer.CustomerName : string.Empty,
                        AdresType = (int)x.AdresType,
                        Country = x.Country,
                        City = x.City,
                        Town = x.Town,
                        Phone = x.Phone,
                        Email = x.Email,
                        PostalCode = x.PostalCode,
                        Address = x.Address,
                        IsActive = x.IsActive,

                    })
                    .ToListAsync();

                if (list.Count == 0)
                {
                    resp.Success = false;
                    resp.Response = "Herhangi Bir Adres bulunamadı.";
                    return resp;
                }

                resp.Success = true;
                resp.Data = list;
            }
            catch (Exception ex)
            {
                resp.Success = false;
                resp.Response = "Adresler alınırken bir hata oluştu.";
                resp.Errors.Add(ex.Message);
            }

            return resp;
        }

        public async Task<BaseResponse<List<CustomerAddressListItem>>> GetByCustomerAsync(int customerId, bool onlyActives)
        {
            var resp = new BaseResponse<List<CustomerAddressListItem>>();
            try
            {
                //int oldugu icin boyle basit bir kontrol yeterli ancak guid vs olsaydi daha farkli kontrol eklenebilir bittabi
                if (customerId <= 0)
                {
                    resp.Success = false;
                    resp.Response = "Geçersiz müşteri Id.";
                    return resp;
                }
                var customer = await _customerRepo.GetByIdAsync(customerId);
                if (customer == null)
                {
                    resp.Success = false;
                    resp.Response = "Müşteri bulunamadı.";
                    return resp;
                }
                var query = _addressRepo.Search(ca => ca.CustomerId == customerId, asNoTracking: true);
                if (onlyActives)
                {
                    query = query.Where(x => x.IsActive);
                }
                var list = await query
                    .Select(x => new CustomerAddressListItem
                    {
                        Id = x.Id,
                        CustomerId = x.CustomerId,
                        AdresType = (int)x.AdresType,
                        Country = x.Country,
                        City = x.City,
                        Town = x.Town,
                        Address = x.Address,
                        IsActive = x.IsActive
                    })
                    .ToListAsync();
                if (list.Count == 0)
                {
                    resp.Success = false;
                    resp.Response = "Adres bulunamadı.";
                    return resp;
                }
                resp.Success = true;
                resp.Data = list;
            }
            catch (Exception ex)
            {
                resp.Success = false;
                resp.Response = "Adresler alınırken bir hata oluştu.";
                resp.Errors.Add(ex.Message);
            }

            return resp;
        }

        public async Task<BaseResponse<CustomerAddressDetailItem>> GetByIdAsync(int id)
        {
            var resp = new BaseResponse<CustomerAddressDetailItem>();
            try
            {
                var entity = await _addressRepo.GetByIdAsync(id);
                if (entity == null)
                {
                    resp.Success = false;
                    resp.Response = "Adres bulunamadı.";
                    return resp;
                }

                resp.Success = true;
                resp.Data = new CustomerAddressDetailItem
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
                    PostalCode = entity.PostalCode,
                    IsActive = entity.IsActive
                };
            }
            catch (Exception ex)
            {
                resp.Success = false;
                resp.Response = "Adres bilgisi alınırken hata oluştu.";
                resp.Errors.Add(ex.Message);
            }

            return resp;
        }

        public async Task<BaseResponse<CustomerAddressDetailItem>> CreateAsync(CustomerAddressCreateRequest dto)
        {
            var resp = new BaseResponse<CustomerAddressDetailItem>();
            try
            {
                if (dto == null || dto.CustomerId <= 0)
                {
                    resp.Success = false;
                    resp.Response = "Adres eklemek için geçerli müşteri seçilmelidir.";
                    return resp;
                }

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
                resp.Data = new CustomerAddressDetailItem
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
                    PostalCode = entity.PostalCode,
                    IsActive = entity.IsActive
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

        public async Task<BaseResponse<bool>> UpdateAsync(CustomerAddressUpdateRequest dto)
        {
            var resp = new BaseResponse<bool>();
            try
            {
                if (dto == null || dto.Id <= 0)
                {
                    resp.Success = false;
                    resp.Response = "Geçersiz adres bilgisi.";
                    return resp;
                }

                var entity = await _addressRepo.GetByIdAsync(dto.Id);
                if (entity == null)
                {
                    resp.Success = false;
                    resp.Response = "Güncellenecek adres bulunamadı.";
                    return resp;
                }

                entity.AdresType = (AddressType)dto.AdresType;
                entity.Country = dto.Country;
                entity.City = dto.City;
                entity.Town = dto.Town;
                entity.Address = dto.Address;
                entity.Email = dto.Email;
                entity.Phone = dto.Phone;
                entity.PostalCode = dto.PostalCode;
                entity.IsActive = dto.IsActive;

                _addressRepo.Update(entity);
                await _uow.SaveChangesAsync();

                resp.Success = true;
                resp.Data = true;
                resp.Response = "Adres güncellendi.";
            }
            catch (Exception ex)
            {
                resp.Success = false;
                resp.Response = "Adres güncellenirken hata oluştu.";
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
                    resp.Response = "Geçersiz adres Id.";
                    return resp;
                }

                var entity = await _addressRepo.GetByIdAsync(id);
                if (entity == null)
                {
                    resp.Success = false;
                    resp.Response = "Silinecek adres bulunamadı.";
                    return resp;
                }

                //adresi silmeden once sipariste kullaniliyor mu gormek lazim yoksa patlariz
                var isUsedInOrders = await _orderRepo
                    .Query(asNoTracking: true)
                    .AnyAsync(o =>
                    o.DeliveryAddressId == id ||
                    o.InvoiceAddressId == id);

                if (isUsedInOrders)
                {
                    resp.Success = false;
                    resp.Response = "Bu adres en az bir siparişte kullanılıyor. Siparişte kullanılan adres silinemez.";
                    return resp;
                }

                _addressRepo.Delete(entity);
                await _uow.SaveChangesAsync();

                resp.Success = true;
                resp.Data = true;
                resp.Response = "Adres silindi.";
            }
            catch (Exception ex)
            {
                resp.Success = false;
                resp.Response = "Adres silinirken hata oluştu.";
                resp.Errors.Add(ex.Message);
            }

            return resp;
        }
    }

}
