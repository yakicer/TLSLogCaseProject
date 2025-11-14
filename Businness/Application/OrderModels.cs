using Entities.Enums;

namespace Businness.Application
{
    public class OrderListItem
    {
        public int Id { get; set; }
        public string OrderNo { get; set; } = null!;
        public string CustomerName { get; set; } = null!;
        public OrderStatus Status { get; set; } = OrderStatus.OrderProcessing;
        public DateTime OrderDate { get; set; }
        public decimal TotalPrice { get; set; }
        public decimal Tax { get; set; }
    }

    public class OrderDetailItem
    {
        public int Id { get; set; }
        public string OrderNo { get; set; } = null!;
        public int CustomerId { get; set; }
        public string CustomerName { get; set; } = null!;
        public OrderStatus Status { get; set; } = OrderStatus.OrderProcessing;
        public DateTime OrderDate { get; set; }
        public decimal TotalPrice { get; set; }
        public decimal Tax { get; set; }
        public int? DeliveryAddressId { get; set; }
        public int? InvoiceAddressId { get; set; }
        public string? DeliveryAddressText { get; set; }
        public string? InvoiceAddressText { get; set; }
        public List<OrderDetailLineItem> Lines { get; set; } = new();
        public List<CustomerAddressItem> CustomerAddresses { get; set; } = new();

    }
    /// <summary>
    /// Line yapisi siparislerin kalemleri icin olusturdugum bir yapi 
    /// yani bir siparis girildiginde birden fazla detay kalemi gibisinden veriler gerekebilir diye dusundum
    /// </summary>
    public class OrderDetailLineItem
    {
        public int StockId { get; set; }
        public string StockName { get; set; } = null!;
        public decimal Amount { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal LineTotal => UnitPrice * Amount;
    }

    public class OrderCreateRequest
    {
        public int CustomerId { get; set; }
        public int? DeliveryAddressId { get; set; }
        public int? InvoiceAddressId { get; set; }
        public OrderStatus Status { get; set; } = OrderStatus.OrderProcessing;
        public string OrderNo { get; set; } = null!;
        public decimal Tax { get; set; }
        public List<OrderLineItem> Lines { get; set; } = new();
    }

    public class OrderUpdateRequest
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public OrderStatus Status { get; set; } = OrderStatus.OrderProcessing;
        public int? DeliveryAddressId { get; set; }
        public int? InvoiceAddressId { get; set; }
        public string OrderNo { get; set; } = null!;
        public decimal Tax { get; set; }
        public List<OrderLineItem> Lines { get; set; } = new();
    }
    public class OrderStatusUpdateRequest
    {
        public int Id { get; set; }
        public OrderStatus Status { get; set; }
    }
    /// <summary>
    /// Line yapisi siparislerin kalemleri icin olusturdugum bir yapi 
    /// yani bir siparis girildiginde birden fazla detay kalemi gibisinden veriler gerekebilir diye dusundum
    /// </summary>
    public class OrderLineItem
    {
        public int StockId { get; set; }
        public decimal Amount { get; set; }
    }
    public class CustomerOrderModel
    {
        public int CustomerId { get; set; }
        public string CustomerName { get; set; } = null!;
        public int OrderId { get; set; }
        public string OrderNo { get; set; } = null!;
        public decimal TotalPrice { get; set; }
        public OrderStatus Status { get; set; } = OrderStatus.OrderProcessing;
        public DateTime OrderDate { get; set; }
        public List<OrderDetailLineItem> Lines { get; set; } = new();
    }

}
