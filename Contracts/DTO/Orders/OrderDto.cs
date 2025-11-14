using Contracts.Enum;

namespace Contracts.DTO.Orders
{
    public class OrderDto
    {
        public int Id { get; set; }
        public string OrderNo { get; set; } = null!;
        public string CustomerName { get; set; } = null!;
        public OrderStatusDto Status { get; set; } = OrderStatusDto.OrderProcessing;
        public DateTime OrderDate { get; set; }
        public decimal TotalPrice { get; set; }
        public decimal Tax { get; set; }
    }
}
