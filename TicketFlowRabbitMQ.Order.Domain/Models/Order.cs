using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicketFlowRabbitMQ.Order.Domain.Models
{
    public class Order
    {
        
        public Guid Id { get; private set; } // --- Internal Configuration
        public string OrderNumber { get; private set; } // --- Identification for the users
        public Guid UserId { get; private set; }
        public Guid EventId { get; private set; }
        public int Quantity { get; set; }
        public decimal TotalPrice { get; set; }
        public OrderStatus Status { get; set; }
        public DateTime CreatedAt { get; private set; }

        // --- Private Constructor to ensure that the only way to create it is using the factory.
        private Order() { }

        public static Order     Create(Guid userId, Guid eventId, int  quantity, decimal totalPrice)
        {
            var id = Guid.NewGuid();
            var createdAt = DateTime.UtcNow;
            var orderNumber = $"{createdAt:yyyyMMdd}-{eventId.ToString()[..3]}-{id.ToString()[..3]}";

            var newOrder = new Order
            {
                Id = id,
                UserId = userId,
                EventId = eventId,
                Quantity = quantity,
                TotalPrice = totalPrice,
                Status = OrderStatus.Pending,
                CreatedAt = createdAt,
                OrderNumber = orderNumber
            };

            return newOrder;
        }

        public void SetStatusToApproved()
        {
            if (Status != OrderStatus.Pending)
            {
                throw new InvalidOperationException("Order cannot be approved from its current status.");
            }
            Status = OrderStatus.Approved;
        }

        public void SetStatusToRejected()
        {
            if (Status != OrderStatus.Pending)
            {
                throw new InvalidOperationException("Order cannot be rejected from its current status.");
            }
            Status = OrderStatus.Rejected;
        }

    }
}
