using Businness.Application;
using Businness.Interface.API;
using DataAccess.Repository.Base;          // IRepositoryBase<T>
using Entities.Entity;
using Entities.Enums;
using Entities.ResponseModel;
using Microsoft.EntityFrameworkCore;

namespace Businness.Implementation.API
{
    public sealed class DashboardService : IDashboardService
    {
        private readonly IRepositoryBase<Order> _orderRepo;
        private readonly IRepositoryBase<OrderDetail> _orderDetailRepo;
        private readonly IRepositoryBase<Customer> _customerRepo;
        private readonly IRepositoryBase<Stock> _stockRepo;

        public DashboardService(
            IRepositoryBase<Order> orderRepo,
            IRepositoryBase<OrderDetail> orderDetailRepo,
            IRepositoryBase<Customer> customerRepo,
            IRepositoryBase<Stock> stockRepo)
        {
            _orderRepo = orderRepo;
            _orderDetailRepo = orderDetailRepo;
            _customerRepo = customerRepo;
            _stockRepo = stockRepo;
        }

        //bu kisim biraz Allah a emanet oldu. cok karisti ortalik. db ile birbirimize girdik... ellememek en hayirlisi sanirim.
        //anladigim tek bisey var, query atarken cok fazla hesaplama yapmamak lazim. o yuzden bazilarini c# tarafina aldim anca kurtardim.
        public async Task<BaseResponse<DashboardResponse>> GetDashboardAsync()
        {
            var resp = new BaseResponse<DashboardResponse>();
            try
            {
                var now = DateTime.UtcNow;
                var startOfMonth = new DateTime(now.Year, now.Month, 1);
                var startOfWeek = now.Date.AddDays(-(int)(now.Date.DayOfWeek == DayOfWeek.Sunday ? 6 : (now.Date.DayOfWeek - DayOfWeek.Monday)));
                var since90 = now.Date.AddDays(-90);
                var twelveMonthsAgo = new DateTime(now.Year, now.Month, 1).AddMonths(-11);
                var sevenDaysAgo = now.Date.AddDays(-6);

                var qOrders = _orderRepo.Query(asNoTracking: true);


                var newOrdersThisWeek = await qOrders.CountAsync(o => o.OrderDate >= startOfWeek);

                var revenueThisMonthRaw = await qOrders
                    .Where(o => o.OrderDate >= startOfMonth)
                    .Select(o => new
                    {
                        o.TotalPrice,
                        o.Tax,
                        OrderDetails = o.OrderDetails.Select(od => new { od.Amount, od.Stock.Price }).ToList()
                    })
                    .ToListAsync();

                var revenueThisMonth = revenueThisMonthRaw
                    .Sum(o => o.TotalPrice > 0
                        ? (o.TotalPrice + o.Tax)
                        : (o.OrderDetails.Sum(od => od.Amount * od.Price) + o.Tax));

                var ordersThisMonth = await qOrders.CountAsync(o => o.OrderDate >= startOfMonth);

                var avgOrderValueThisMonth = ordersThisMonth == 0 ? 0 : (revenueThisMonth / ordersThisMonth);

                var activeCustomers = await _customerRepo.Query(asNoTracking: true).CountAsync(c => c.IsActive);
                var activeStocks = await _stockRepo.Query(asNoTracking: true).CountAsync(s => s.IsActive);

                var summary = new SummaryCardsDto
                {
                    NewOrdersThisWeek = newOrdersThisWeek,
                    RevenueThisMonth = revenueThisMonth,
                    OrdersThisMonth = ordersThisMonth,
                    AvgOrderValueThisMonth = avgOrderValueThisMonth,
                    ActiveCustomers = activeCustomers,
                    ActiveStocks = activeStocks
                };


                var monthlySalesRaw = await qOrders
                    .Where(o => o.OrderDate >= twelveMonthsAgo)
                    .Select(o => new
                    {
                        Y = o.OrderDate.Year,
                        M = o.OrderDate.Month,
                        o.TotalPrice,
                        o.Tax,
                        Details = o.OrderDetails.Select(od => new { od.Amount, od.Stock.Price }).ToList()
                    })
                    .ToListAsync();

                var monthlySalesGrouped = monthlySalesRaw
                    .GroupBy(x => new { x.Y, x.M })
                    .Select(g => new
                    {
                        g.Key.Y,
                        g.Key.M,
                        Revenue = g.Sum(x => x.TotalPrice > 0
                            ? (x.TotalPrice + x.Tax)
                            : (x.Details.Sum(d => d.Amount * d.Price) + x.Tax))
                    })
                    .OrderBy(x => x.Y).ThenBy(x => x.M)
                    .ToList();

                var salesLast12 = monthlySalesGrouped
                    .Select(x => new MonthlySalesPoint { Month = $"{x.Y:D4}-{x.M:D2}", Revenue = x.Revenue })
                    .ToList();

                var orders7Raw = await qOrders
                    .Where(o => o.OrderDate.Date >= sevenDaysAgo)
                    .GroupBy(o => o.OrderDate.Date)
                    .Select(g => new { Day = g.Key, Count = g.Count() })
                    .OrderBy(x => x.Day)
                    .ToListAsync();

                var ordersLast7 = orders7Raw
                    .Select(x => new WeeklyOrdersPoint { Day = x.Day.ToString("yyyy-MM-dd"), Count = x.Count })
                    .ToList();


                var statusCounts = await qOrders
                    .GroupBy(o => o.Status)
                    .Select(g => new { Status = g.Key, Count = g.Count() })
                    .ToListAsync();

                var ordersByStatus = new OrdersByStatusDto
                {
                    Cancelled = statusCounts.FirstOrDefault(x => x.Status == OrderStatus.OrderCancelled)?.Count ?? 0,
                    Delivered = statusCounts.FirstOrDefault(x => x.Status == OrderStatus.OrderDelivered)?.Count ?? 0,
                    InProgress = statusCounts.FirstOrDefault(x => x.Status == OrderStatus.OrderProcessing)?.Count ?? 0,
                    OnDelivery = statusCounts.FirstOrDefault(x => x.Status == OrderStatus.OrderInTransit)?.Count ?? 0,
                    Returned = statusCounts.FirstOrDefault(x => x.Status == OrderStatus.OrderReturned)?.Count ?? 0
                };

                var topCustomersRaw = await qOrders
                    .Where(o => o.OrderDate >= since90)
                    .Select(o => new
                    {
                        o.CustomerId,
                        o.TotalPrice,
                        o.Tax,
                        Details = o.OrderDetails.Select(od => new { od.Amount, od.Stock.Price }).ToList()
                    })
                    .ToListAsync();

                var topCustomersGrouped = topCustomersRaw
                    .GroupBy(x => x.CustomerId)
                    .Select(g => new
                    {
                        CustomerId = g.Key,
                        Orders = g.Count(),
                        Revenue = g.Sum(x => x.TotalPrice > 0
                            ? (x.TotalPrice + x.Tax)
                            : (x.Details.Sum(d => d.Amount * d.Price) + x.Tax))
                    })
                    .OrderByDescending(x => x.Revenue)
                    .Take(10)
                    .ToList();

                var topCustomers = topCustomersGrouped
                    .Join(_customerRepo.Query(asNoTracking: true),
                        g => g.CustomerId,
                        c => c.Id,
                        (g, c) => new TopCustomerItem
                        {
                            CustomerId = c.Id,
                            CustomerName = c.CustomerName,
                            Revenue = g.Revenue,
                            Orders = g.Orders
                        })
                    .ToList();

                var qOrderDetails = _orderDetailRepo.Query(asNoTracking: true);

                var recentOrderIds = await _orderRepo.Query(asNoTracking: true)
                    .Where(o => o.OrderDate >= since90)
                    .Select(o => o.Id)
                    .ToListAsync();

                var topStocks = await qOrderDetails
                    .Where(od => recentOrderIds.Contains(od.OrderId))
                    .GroupBy(od => od.StockId)
                    .Select(g => new
                    {
                        StockId = g.Key,
                        Quantity = g.Sum(x => x.Amount),
                        Revenue = g.Sum(x => x.Amount * x.Stock.Price)
                    })
                    .OrderByDescending(x => x.Quantity)
                    .Take(10)
                    .Join(_stockRepo.Query(asNoTracking: true),
                          g => g.StockId,
                          s => s.Id,
                          (g, s) => new TopStockItem
                          {
                              StockId = s.Id,
                              StockName = s.StockName,
                              QuantitySold = g.Quantity,
                              Revenue = g.Revenue
                          })
                    .ToListAsync();

                var ordersByCity = await qOrders
                    .Where(o => o.OrderDate >= since90)
                    .Select(o => new
                    {
                        City = o.DeliveryAddress != null
                            ? o.DeliveryAddress.City
                            : (o.InvoiceAddress != null ? o.InvoiceAddress.City : null)
                    })
                    .Where(x => x.City != null && x.City != "")
                    .GroupBy(x => x.City!)
                    .Select(g => new OrdersByCityItem { City = g.Key, OrderCount = g.Count() })
                    .OrderByDescending(x => x.OrderCount)
                    .ToListAsync();

                resp.Success = true;
                resp.Data = new DashboardResponse
                {
                    Summary = summary,
                    SalesLast12 = salesLast12,
                    OrdersLast7 = ordersLast7,
                    OrdersByStatus = ordersByStatus,
                    TopCustomers = topCustomers,
                    TopStocks = topStocks,
                    OrdersByCity = ordersByCity
                };
            }
            catch (Exception ex)
            {
                resp.Success = false;
                resp.Response = $"Dashboard verileri alınamadı: {ex.Message}";
                resp.Errors.Add(ex.ToString());
            }

            return resp;
        }
    }
}
