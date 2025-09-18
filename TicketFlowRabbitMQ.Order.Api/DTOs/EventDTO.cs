namespace TicketFlowRabbitMQ.Order.Api.DTOs
{
    public class EventDTO
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string Location { get; set; }
        public string Date { get; set; }
        public decimal TicketPrice { get; set; }
        public int AvailableTickets { get; set; }
    }

    public class UpdateEventDTO
    {
        public string? Title { get; set; } = string.Empty;
        public string? Description { get; set; } = string.Empty;
        public string? Location { get; set; } = string.Empty;
        public string? Date { get; set; } = string.Empty;
        public decimal TicketPrice { get; set; } = 0;
        public int AvailableTickets { get; set; } = 0;
    }
}
