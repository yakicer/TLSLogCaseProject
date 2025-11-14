namespace Contracts.DTO.Dashboard
{
    public class DashboardResponse
    {
        public SummaryCardsDto Summary { get; set; } = new();
        public List<MonthlySalesPoint> SalesLast12 { get; set; } = new();
        public List<WeeklyOrdersPoint> OrdersLast7 { get; set; } = new();
        public OrdersByStatusDto OrdersByStatus { get; set; } = new();
        public List<TopCustomerItem> TopCustomers { get; set; } = new();
        public List<TopStockItem> TopStocks { get; set; } = new();
        public List<OrdersByCityItem> OrdersByCity { get; set; } = new();
    }

    public class SummaryCardsDto
    {
        public int NewOrdersThisWeek { get; set; }
        public decimal RevenueThisMonth { get; set; }
        public int OrdersThisMonth { get; set; }
        public decimal AvgOrderValueThisMonth { get; set; }
        public int ActiveCustomers { get; set; }
        public int ActiveStocks { get; set; }
    }

    public class MonthlySalesPoint
    {
        public string Month { get; set; } = "";   // "2025-07"
        public decimal Revenue { get; set; }
    }

    public class WeeklyOrdersPoint
    {
        public string Day { get; set; } = "";     // "Mon"/"Tue" veya "2025-11-14"
        public int Count { get; set; }
    }

    public class OrdersByStatusDto
    {
        public int Cancelled { get; set; }     // 0
        public int Delivered { get; set; }     // 1
        public int InProgress { get; set; }    // 2
        public int OnDelivery { get; set; }    // 3
        public int Returned { get; set; }      // 4
    }

    public class TopCustomerItem
    {
        public int CustomerId { get; set; }
        public string CustomerName { get; set; } = "";
        public decimal Revenue { get; set; }
        public int Orders { get; set; }
    }

    public class TopStockItem
    {
        public int StockId { get; set; }
        public string StockName { get; set; } = "";
        public decimal QuantitySold { get; set; }
        public decimal Revenue { get; set; }
    }

    public class OrdersByCityItem
    {
        public string City { get; set; } = "";
        public int OrderCount { get; set; }
    }
}
