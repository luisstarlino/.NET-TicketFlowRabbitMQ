using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicketFlowRabbitMQ.Order.Data.Context
{
    public class TicketFlowContextFactory : IDesignTimeDbContextFactory<TicketFlowContext>
    {
        public TicketFlowContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<TicketFlowContext>();

            optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=ticketflow;Username=user;Password=password");

            return new TicketFlowContext(optionsBuilder.Options);
        }
    }
}
