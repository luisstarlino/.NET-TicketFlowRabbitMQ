namespace TicketFlowRabbitMQ.Order.Api.DTOs
{
    public class OrderDTO
    {
        public Guid UserId { get; set; }
        public Guid EventId { get; set; }
        public int Quantity { get; set; }
        public decimal TotalPrice { get; set; }
    }
}
