using DataAccess.Repository.Base;
using Entities.Entity;

namespace DataAccess.Repository.Interface
{
    public interface ICustomerRepository : IRepositoryBase<Customer>
    {
        //ornek amacli bir ozel metot tanimi
        Task<List<Customer>> GetActiveCustomersAsync();
    }
}
