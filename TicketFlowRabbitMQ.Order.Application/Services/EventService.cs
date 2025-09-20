using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicketFlowRabbitMQ.Order.Application.Interfaces;
using TicketFlowRabbitMQ.Order.Domain.Interfaces;
using TicketFlowRabbitMQ.Order.Domain.Models;

namespace TicketFlowRabbitMQ.Order.Application.Services
{
    public class EventService : IEventService
    {
        private readonly IFlowRepository _flowRepository;

        public EventService(IFlowRepository flowRepository)
        {
            _flowRepository = flowRepository;
        }

        async public Task<Event?> DeleteEvent(Event @event)
        {
            return await _flowRepository.DeleteEventAsync(@event.Id);
        }

        async public Task<IEnumerable<Event>> GetAll()
        {
            return await _flowRepository.GetAllEvents();
        }

        public async Task<Event?> GetUniqueEventById(Guid id)
        {
            return await _flowRepository.GetEventByIdAsync(id);
        }

        async public Task<Guid> ProcessAndSaveNewEvent(Event @event)
        {
            return await _flowRepository.AddEventAsync(@event);
        }

        async public Task<Event?> UpdateEvent(Event @event)
        {
            return await _flowRepository.UpdateEventAsync(@event);
        }
    }
}
