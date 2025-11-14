namespace Entities.DTO
{
    public class OrderListDto
    {
        public int Id { get; set; }
        public string OrderNo { get; set; } = null!;
        public string CustomerName { get; set; } = null!;
        public DateTime OrderDate { get; set; }
        public decimal TotalPrice { get; set; }
        public decimal Tax { get; set; }
    }

    public class OrderDetailDto
    {
        public int Id { get; set; }
        public string OrderNo { get; set; } = null!;
        public int CustomerId { get; set; }
        public string CustomerName { get; set; } = null!;
        public DateTime OrderDate { get; set; }
        public decimal TotalPrice { get; set; }
        public decimal Tax { get; set; }
        public int? DeliveryAddressId { get; set; }
        public int? InvoiceAddressId { get; set; }
        public string? DeliveryAddressText { get; set; }
        public string? InvoiceAddressText { get; set; }
        public List<OrderDetailLineDto> Lines { get; set; } = new();
    }
    /// <summary>
    /// Line yapisi siparislerin kalemleri icin olusturdugum bir yapi 
    /// yani bir siparis girildiginde birden fazla detay kalemi gibisinden veriler gerekebilir diye dusundum
    /// </summary>
    public class OrderDetailLineDto
    {
        public int StockId { get; set; }
        public string StockName { get; set; } = null!;
        public decimal Amount { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal LineTotal => UnitPrice * Amount;
    }

    public class OrderCreateDto
    {
        public int CustomerId { get; set; }
        public int? DeliveryAddressId { get; set; }
        public int? InvoiceAddressId { get; set; }
        public string OrderNo { get; set; } = null!;
        public decimal Tax { get; set; }
        public List<OrderCreateLineDto> Lines { get; set; } = new();
    }
    /// <summary>
    /// Line yapisi siparislerin kalemleri icin olusturdugum bir yapi 
    /// yani bir siparis girildiginde birden fazla detay kalemi gibisinden veriler gerekebilir diye dusundum
    /// </summary>
    public class OrderCreateLineDto
    {
        public int StockId { get; set; }
        public decimal Amount { get; set; }
    }
    public class CustomerOrderDto
    {
        public int CustomerId { get; set; }
        public string CustomerName { get; set; } = null!;
        public int OrderId { get; set; }
        public string OrderNo { get; set; } = null!;
        public DateTime OrderDate { get; set; }
        public List<OrderDetailLineDto> Lines { get; set; } = new();
    }

}
