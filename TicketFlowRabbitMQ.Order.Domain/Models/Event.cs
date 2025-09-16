using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicketFlowRabbitMQ.Order.Domain.Models
{
    public class Event
    {
        public Guid Id { get; private set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Location { get; set; }
        public DateTime Date { get; set; }
        public decimal TicketPrice { get; set; }
        public int AvailableTickets { get; set; }

        // --- Private Constructor to ensure that the only way to create it is using the factory.
        private Event() { }

        public static Event Create(string title, string description, string location, DateTime date, decimal ticketPrice, int availableTickets)
        {
            if (string.IsNullOrWhiteSpace(title) || ticketPrice <= 0 || availableTickets < 0)
            {
                throw new ArgumentException("Invalid event data.");
            }

            return new Event
            {
                Id = Guid.NewGuid(),
                Title = title,
                Description = description,
                Location = location,
                Date = date,
                TicketPrice = ticketPrice,
                AvailableTickets = availableTickets
            };
        }

        // --- Domain behavior: reduce ticket inventory
        public void DecreaseAvailableTickets(int quantity)
        {
            if (quantity <= 0)
            {
                throw new ArgumentException("Quantity must be greater than zero.");
            }

            if (AvailableTickets < quantity)
            {
                throw new InvalidOperationException("Not enough tickets available.");
            }

            AvailableTickets -= quantity;
        }


    }
}
