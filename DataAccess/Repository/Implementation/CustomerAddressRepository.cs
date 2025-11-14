using DataAccess.Context;
using DataAccess.Repository.Base;
using DataAccess.Repository.Interface;
using Entities.Entity;

namespace DataAccess.Repository.Implementation
{
    public class CustomerAddressRepository : RepositoryBase<CustomerAddress>, ICustomerAddressRepository
    {
        public CustomerAddressRepository(ApplicationDBContext context) : base(context)
        {
        }
    }
}
