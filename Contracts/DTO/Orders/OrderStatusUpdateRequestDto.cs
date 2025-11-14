using Contracts.Enum;

namespace Contracts.DTO.Orders
{
    public class OrderStatusUpdateRequestDto
    {
        public int Id { get; set; }
        public OrderStatusDto Status { get; set; }
    }
}
