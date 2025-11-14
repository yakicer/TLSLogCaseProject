using DataAccess.Context;
using DataAccess.Repository.Base;
using DataAccess.Repository.Interface;
using Entities.Entity;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Repository.Implementation
{
    public class CustomerRepository : RepositoryBase<Customer>, ICustomerRepository
    {
        public CustomerRepository(ApplicationDBContext context) : base(context)
        {
        }

        public async Task<List<Customer>> GetActiveCustomersAsync()
        {
            return await _dbSet
                .Where(x => x.IsActive)
                .ToListAsync();
        }
    }
}
