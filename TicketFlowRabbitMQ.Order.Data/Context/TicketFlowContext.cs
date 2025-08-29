using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicketFlowRabbitMQ.Order.Domain.Models;

namespace TicketFlowRabbitMQ.Order.Data.Context
{
    public class TicketFlowContext : DbContext
    {
        public TicketFlowContext(DbContextOptions<TicketFlowContext> options) : base(options) { }

        // --- DB Sets
        public DbSet<Domain.Models.Order> Orders { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Event> Events { get; set; }        

    }
}
