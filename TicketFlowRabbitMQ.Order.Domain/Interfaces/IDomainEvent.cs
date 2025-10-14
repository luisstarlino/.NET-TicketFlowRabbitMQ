namespace TicketFlowRabbitMQ.Order.Domain.Interfaces
{
    public interface IDomainEvent
    {

        Guid EventRabbitId { get; }

        DateTime OccurredOn { get; }
    }
}