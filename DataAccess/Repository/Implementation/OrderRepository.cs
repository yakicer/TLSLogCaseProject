using DataAccess.Context;
using DataAccess.Repository.Base;
using DataAccess.Repository.Interface;
using Entities.Entity;

namespace DataAccess.Repository.Implementation
{
    public class OrderRepository : RepositoryBase<Order>, IOrderRepository
    {
        public OrderRepository(ApplicationDBContext context) : base(context)
        {
        }
    }
}
