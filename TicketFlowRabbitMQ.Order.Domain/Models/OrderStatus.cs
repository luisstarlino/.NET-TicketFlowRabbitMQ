using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicketFlowRabbitMQ.Order.Domain.Models
{
    public enum OrderStatus
    {
        Pending,
        Approved,
        Rejected,
        Canceled
    }

}
