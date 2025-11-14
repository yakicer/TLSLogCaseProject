namespace Businness.Application
{
    public sealed class MonthlySalesPoint
    {
        public string Month { get; set; } = "";
        public decimal Revenue { get; set; }
    }

    public sealed class WeeklyOrdersPoint
    {
        public string Day { get; set; } = "";
        public int Count { get; set; }
    }

    public sealed class OrdersByStatusDto
    {
        public int Cancelled { get; set; }
        public int Delivered { get; set; }
        public int InProgress { get; set; }
        public int OnDelivery { get; set; }
        public int Returned { get; set; }
    }

    public sealed class TopCustomerItem
    {
        public int CustomerId { get; set; }
        public string CustomerName { get; set; } = "";
        public int Orders { get; set; }
        public decimal Revenue { get; set; }
    }

    public sealed class TopStockItem
    {
        public int StockId { get; set; }
        public string StockName { get; set; } = "";
        public decimal QuantitySold { get; set; }
        public decimal Revenue { get; set; }
    }

    public sealed class OrdersByCityItem
    {
        public string City { get; set; } = "";
        public int OrderCount { get; set; }
    }

    public sealed class SummaryCardsDto
    {
        public int NewOrdersThisWeek { get; set; }
        public decimal RevenueThisMonth { get; set; }
        public int OrdersThisMonth { get; set; }
        public decimal AvgOrderValueThisMonth { get; set; }
        public int ActiveCustomers { get; set; }
        public int ActiveStocks { get; set; }
    }

    public sealed class DashboardResponse
    {
        public SummaryCardsDto Summary { get; set; } = new();
        public List<MonthlySalesPoint> SalesLast12 { get; set; } = new();
        public List<WeeklyOrdersPoint> OrdersLast7 { get; set; } = new();
        public OrdersByStatusDto OrdersByStatus { get; set; } = new();
        public List<TopCustomerItem> TopCustomers { get; set; } = new();
        public List<TopStockItem> TopStocks { get; set; } = new();
        public List<OrdersByCityItem> OrdersByCity { get; set; } = new();
    }
}
