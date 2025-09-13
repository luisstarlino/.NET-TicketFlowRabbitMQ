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
    public class UserService : IUserService
    {
        private readonly IFlowRepository _repository;

        public UserService(IFlowRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<User>> GetAll()
        {
            return await _repository.GetAllUsers();
        }

        public async Task<User?> GetUniqueUserById(Guid id)
        {
            return await _repository.GetUserByIdAsync(id);
        }

        public async Task<Guid> ProcessAndSaveNewUser(User model)
        {
            return await _repository.AddUserAsync(model);
        }
    }
}
