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
    public class FlowRepository : IFlowRepository
    {
        private readonly TicketFlowContext _ctx;

        public FlowRepository(TicketFlowContext ctx)
        {
            _ctx = ctx;
        }
        async public Task<Guid> AddUserAsync(User user)
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

        async public Task<User?> DeleteUserAsync(Guid idUser)
        {
            try
            {
                var userToDelete = await _ctx.Users.FindAsync(idUser);
                if (userToDelete != null)
                {
                    _ctx.Users.Remove(userToDelete);
                    await _ctx.SaveChangesAsync();

                }
                return userToDelete;
            }
            catch (Exception)
            {
                return null;
            }
        }

        async public Task<IEnumerable<User>> GetAllUsers()
        {
            var users = await _ctx.Users.ToListAsync();
            return users;
        }

        async public Task<User?> GetUserByIdAsync(Guid id)
        {
            return await _ctx.Users.FindAsync(id);
        }

        async public Task<User?> UpdateUserAsync(User uptUser)
        {
            try
            {
                var userToUpdate = await _ctx.Users.FindAsync(uptUser.Id);
                if (userToUpdate is null) return null;
                else
                {
                    userToUpdate.Name = String.IsNullOrWhiteSpace(uptUser.Name) ? userToUpdate.Name : uptUser.Name;
                    userToUpdate.Email = String.IsNullOrWhiteSpace(uptUser.Email) ? userToUpdate.Email : uptUser.Email;
                    userToUpdate.Password = String.IsNullOrWhiteSpace(uptUser.Password) ? userToUpdate.Password : uptUser.Password;
                    userToUpdate.Phone = String.IsNullOrWhiteSpace(uptUser.Phone) ? userToUpdate.Phone : uptUser.Phone;
                    userToUpdate.BirthDate = String.IsNullOrWhiteSpace(uptUser.BirthDate.ToString()) ? userToUpdate.BirthDate : uptUser.BirthDate;

                    await _ctx.SaveChangesAsync();

                    return userToUpdate;
                }
            }
            catch
            {
                return null;
            }
        }

        async public Task<User?> GetUserByMail(string mail)
        {
            return await _ctx.Users.Where(u => u.Email.Equals(mail)).FirstOrDefaultAsync() ?? null;
        }
    }
}
