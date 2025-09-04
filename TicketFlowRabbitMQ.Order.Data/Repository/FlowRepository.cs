using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicketFlowRabbitMQ.Order.Data.Context;
using TicketFlowRabbitMQ.Order.Domain.Interfaces;
using TicketFlowRabbitMQ.Order.Domain.Models;

namespace TicketFlowRabbitMQ.Order.Data.Repository
{
    internal class FlowRepository : IFlowRepository
    {
        private readonly TicketFlowContext _ctx;

        public FlowRepository(TicketFlowContext ctx)
        {
            _ctx = ctx;
        }
        async public Task<Guid> Add(User user)
        {
            try
            {
                await _ctx.Users.AddAsync(user);
                await _ctx.SaveChangesAsync();
                return user.Id;
            }
            catch (Exception ex)
            {
                return Guid.Empty;
            }
        }

        async public Task<IEnumerable<User>> GetAllUsers()
        {
            var users = await _ctx.Users.ToListAsync();
            return users;
        }

        async public Task<List<User>>? GetByMail(string mail)
        {
            return await _ctx.Users.Where(u => u.Email.Equals(mail)).ToListAsync();
        }
    }
}
