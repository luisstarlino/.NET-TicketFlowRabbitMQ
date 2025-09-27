using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicketFlowRabbitMQ.Order.Domain.Interfaces;
using TicketFlowRabbitMQ.Order.Domain.Models;

namespace TicketFlowRabbitMQ.Order.Domain.Events
{
    public class OrderCreatedEvent : IDomainEvent
    {
        Guid EventRabbitId { get; }
        public DateTime OccurredOn { get; }
        Guid IDomainEvent.EventRabbitId => EventRabbitId;


        public Guid IdOrder { get; private set; }
        public Guid EventId { get; private set; }
        public string OrderNumber { get; private set; }
        public Guid UserId { get; private set; }
        public int Quantity { get; set; }
        public decimal TotalPrice { get; set; }


        public OrderCreatedEvent(Guid idOrder, string orderNumber, Guid userId, Guid eventId, int quantity, decimal total)
        {
            EventRabbitId = Guid.NewGuid();
            OccurredOn = DateTime.Now;

            EventId = eventId;
            IdOrder = idOrder;
            OrderNumber = orderNumber;
            UserId = userId;
            Quantity = quantity;
            TotalPrice = total;
        }
    }
}
