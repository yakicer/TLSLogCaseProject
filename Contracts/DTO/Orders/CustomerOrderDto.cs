using Contracts.Enum;

namespace Contracts.DTO.Orders
{
    public class CustomerOrderDto
    {
        public int CustomerId { get; set; }
        public string CustomerName { get; set; } = null!;
        public int OrderId { get; set; }
        public string OrderNo { get; set; } = null!;
        public decimal TotalPrice { get; set; }
        public OrderStatusDto Status { get; set; } = OrderStatusDto.OrderProcessing;
        public DateTime OrderDate { get; set; }
        public List<OrderDetailLineDto> Lines { get; set; } = new();
    }
}
