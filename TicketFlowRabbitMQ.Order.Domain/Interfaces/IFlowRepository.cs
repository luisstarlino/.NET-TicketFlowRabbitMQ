using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicketFlowRabbitMQ.Order.Domain.Models;

namespace TicketFlowRabbitMQ.Order.Domain.Interfaces
{
    public interface IFlowRepository
    {
        Task<IEnumerable<User>> GetAllUsers();
        Task<Guid> Add(User user);
        Task<List<User>>? GetByMail(string mail);
    }
}
