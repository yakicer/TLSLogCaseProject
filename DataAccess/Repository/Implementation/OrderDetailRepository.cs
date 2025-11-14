using DataAccess.Context;
using DataAccess.Repository.Base;
using DataAccess.Repository.Interface;
using Entities.Entity;

namespace DataAccess.Repository.Implementation
{
    public class OrderDetailRepository : RepositoryBase<OrderDetail>, IOrderDetailRepository
    {
        public OrderDetailRepository(ApplicationDBContext context) : base(context)
        {
        }
    }
}
