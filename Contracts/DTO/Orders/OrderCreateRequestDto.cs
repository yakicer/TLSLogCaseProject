using Contracts.Enum;

namespace Contracts.DTO.Orders
{
    public class OrderCreateRequestDto
    {
        public int CustomerId { get; set; }
        public int? DeliveryAddressId { get; set; }
        public int? InvoiceAddressId { get; set; }
        public OrderStatusDto Status { get; set; } = OrderStatusDto.OrderProcessing;
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

}
