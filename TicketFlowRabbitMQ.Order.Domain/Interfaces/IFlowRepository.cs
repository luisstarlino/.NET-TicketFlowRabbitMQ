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
        #region USER CRUD
        Task<Guid> AddUserAsync(User user);
        Task <User?> UpdateUserAsync(User user);
        Task <User?> DeleteUserAsync(Guid idUser);
        Task<IEnumerable<User>> GetAllUsers();
        Task<User?> GetUserByMail(string mail);
        Task<User?> GetUserByIdAsync(Guid id);
        #endregion

        #region EVENTS CRUD
        Task<Guid> AddEventAsync(Event @event);
        Task <Event?> UpdateEventAsync(Event @event);
        Task<Event?> DeleteEventAsync(Guid idEvent);
        Task<IEnumerable<Event>> GetAllEvents();
        Task<Event?> GetEventByIdAsync(Guid id);
        #endregion

        #region ORDER CRUD
        Task<IEnumerable<Models.Order>> GetAllOrders();
        Task<Models.Order> AddOrderAsync(Models.Order m);
        Task<Models.Order?> UpdateOrderAsync(Models.Order m);
        Task<Models.Order?> GetOrderByIdAsync(Guid id);
        #endregion



    }
}
