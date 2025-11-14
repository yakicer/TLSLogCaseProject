using Contracts.Enum;

namespace Contracts.DTO.Orders
{
    public class OrderUpdateRequestDto
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public OrderStatusDto Status { get; set; } = OrderStatusDto.OrderProcessing;
        public int? DeliveryAddressId { get; set; }
        public int? InvoiceAddressId { get; set; }
        public string OrderNo { get; set; } = null!;
        public decimal Tax { get; set; }
        public List<OrderLineDto> Lines { get; set; } = new();
    }

    public class OrderLineDto
    {
        public int StockId { get; set; }
        public decimal Amount { get; set; }
    }
}
