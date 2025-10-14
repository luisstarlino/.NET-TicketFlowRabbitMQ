using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicketFlowRabbitMQ.Order.Application.Interfaces;
using TicketFlowRabbitMQ.Order.Domain.Events;
using TicketFlowRabbitMQ.Order.Domain.Interfaces;

namespace TicketFlowRabbitMQ.Order.Application.Services
{
    public class OrderService : IOrderService
    {
        private readonly IFlowRepository _flowRepository;
        private readonly IEventBus _eventBus;

        public OrderService(IFlowRepository flowRepository, IEventBus eventBus)
        {
            _flowRepository = flowRepository;
            _eventBus = eventBus;
        }

        async public Task<Domain.Models.Order> CreateNewOrderProcessing(Domain.Models.Order model)
        {
            //------------------------------------------------------------------------------------------------
            // R1. TODO Regra de Negócio:
            //    * Buscar o Evento para garantir que há ingressos disponíveis.
            //    * Diminui a contagem de ingressos no Evento.
            //------------------------------------------------------------------------------------------------

            //------------------------------------------------------------------------------------------------
            // R2. New Order Flow Transaction
            //     * Add a new order
            //     * Update Event Avalable Tickets 
            //------------------------------------------------------------------------------------------------
            var order = await _flowRepository.AddOrderAsync(model);

            //------------------------------------------------------------------------------------------------
            // R3. Event Publish 
            //     * 3.1 Create a Model to send
            //     * 3.2 Send to qeue
            //------------------------------------------------------------------------------------------------
            var orderEvent = new OrderCreatedEvent(
                idOrder: order.Id,
                orderNumber: order.OrderNumber,
                userId: order.UserId,
                eventId: order.EventId,
                quantity: order.Quantity,
                total: order.TotalPrice
            );

            _eventBus.Publish(orderEvent);

            return order;
        }

        public async Task<IEnumerable<Domain.Models.Order>> GetAll()
        {
            return await _flowRepository.GetAllOrders();
        }

        public async Task<Domain.Models.Order> GetUniqueById(Guid id)
        {
            return await _flowRepository.GetOrderByIdAsync(id);
        }

        public async Task<Domain.Models.Order> UpdateOrderProcessing(Domain.Models.Order model)
        {
            return await _flowRepository.UpdateOrderAsync(model);
        }
    }
}
