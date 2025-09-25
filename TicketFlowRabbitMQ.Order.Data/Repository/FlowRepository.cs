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

        #region === USER
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

        #endregion

        #region === EVENT
        async public Task<Guid> AddEventAsync(Event @event)
        {
            try
            {
                await _ctx.Events.AddAsync(@event);
                await _ctx.SaveChangesAsync();
                return @event.Id;
            }
            catch (Exception ex)
            {
                return Guid.Empty;
            }
        }

        async public Task<Event?> UpdateEventAsync(Event @event)
        {
            try
            {
                var eventToUpdate = await _ctx.Events.FindAsync(@event.Id);
                if (eventToUpdate is null) return null;
                else
                {

                    eventToUpdate.Title = String.IsNullOrWhiteSpace(@event.Title) ? eventToUpdate.Title : @event.Title;
                    eventToUpdate.Description = String.IsNullOrWhiteSpace(@event.Description) ? eventToUpdate.Description : @event.Description;
                    eventToUpdate.Location = String.IsNullOrWhiteSpace(@event.Location) ? eventToUpdate.Location : @event.Location;
                    eventToUpdate.TicketPrice = @event.TicketPrice <= 0 ? eventToUpdate.TicketPrice : @event.TicketPrice;
                    eventToUpdate.AvailableTickets = @event.AvailableTickets == default ? eventToUpdate.AvailableTickets : @event.AvailableTickets;
                    eventToUpdate.Date = String.IsNullOrWhiteSpace(@event.Date.ToString()) ? eventToUpdate.Date : @event.Date;

                    await _ctx.SaveChangesAsync();

                    return eventToUpdate;
                }
            }
            catch
            {
                return null;
            }
        }

        async public Task<Event?> DeleteEventAsync(Guid idEvent)
        {
            try
            {
                var eventToDelete = await _ctx.Events.FindAsync(idEvent);
                if (eventToDelete != null)
                {
                    _ctx.Events.Remove(eventToDelete);
                    await _ctx.SaveChangesAsync();

                }
                return eventToDelete;
            }
            catch (Exception)
            {
                return null;
            }
        }

        async public Task<IEnumerable<Event>> GetAllEvents()
        {
            var events = await _ctx.Events.ToListAsync();
            return events;
        }

        async public Task<Event?> GetEventByIdAsync(Guid id)
        {
            return await _ctx.Events.FindAsync(id);
        }

        #endregion

        #region === ORDER

        #endregion

    }
}
