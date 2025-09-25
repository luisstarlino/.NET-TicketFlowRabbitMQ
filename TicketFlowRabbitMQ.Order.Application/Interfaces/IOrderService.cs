using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicketFlowRabbitMQ.Order.Domain.Models;

namespace TicketFlowRabbitMQ.Order.Application.Interfaces
{
    public interface IOrderService
    {
        Task<IEnumerable<Domain.Models.Order>> GetAll();
        Task<Domain.Models.Order> GetUniqueById(Guid id);
        Task<Domain.Models.Order> CreateNewOrderProcessing(Domain.Models.Order model);
        Task<Domain.Models.Order> UpdateOrderProcessing(Domain.Models.Order model);

    }
}
