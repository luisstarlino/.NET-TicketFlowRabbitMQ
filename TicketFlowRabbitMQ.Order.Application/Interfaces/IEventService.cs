using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicketFlowRabbitMQ.Order.Domain.Models;

namespace TicketFlowRabbitMQ.Order.Application.Interfaces
{
    public interface IEventService
    {
        Task<IEnumerable<Event>> GetAll();
        Task<Event?> UpdateEvent(Event @event);
        Task<Event?> GetUniqueEventById(Guid id);
        Task<Event?> DeleteEvent(Event @event);
        Task<Guid> ProcessAndSaveNewEvent(Event @event);
    }
}
