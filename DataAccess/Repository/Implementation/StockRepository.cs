using DataAccess.Context;
using DataAccess.Repository.Base;
using DataAccess.Repository.Interface;
using Entities.Entity;

namespace DataAccess.Repository.Implementation
{
    public class StockRepository : RepositoryBase<Stock>, IStockRepository
    {
        public StockRepository(ApplicationDBContext context) : base(context)
        {
        }
    }
}
