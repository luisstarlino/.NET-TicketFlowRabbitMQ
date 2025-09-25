using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicketFlowRabbitMQ.Order.Application.Interfaces;
using TicketFlowRabbitMQ.Order.Domain.Interfaces;

namespace TicketFlowRabbitMQ.Order.Application.Services
{
    public class OrderService : IOrderService
    {
        private readonly IFlowRepository _flowRepository;

        public OrderService(IFlowRepository flowRepository)
        {
            _flowRepository = flowRepository;
        }

        public Task<Domain.Models.Order> CreateNewOrderProcessing(Domain.Models.Order model)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Domain.Models.Order>> GetAll()
        {
            throw new NotImplementedException();
        }

        public Task<Domain.Models.Order> GetUniqueById(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<Domain.Models.Order> UpdateOrderProcessing(Domain.Models.Order model)
        {
            throw new NotImplementedException();
        }
    }
}
