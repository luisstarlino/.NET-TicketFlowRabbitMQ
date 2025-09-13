using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicketFlowRabbitMQ.Order.Domain.Helpers
{
    public static class StringHelper
    {
        public static readonly string MISSING_PARAMETERS = "Parameters are missing. Please, send all!";
        public static bool IsEmpty(this string value)
        {
            return string.IsNullOrEmpty(value);
        }
    }
}
