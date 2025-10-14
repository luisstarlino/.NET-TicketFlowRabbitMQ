using Microsoft.AspNetCore.Mvc;
using TicketFlowRabbitMQ.Order.Api.DTOs;
using TicketFlowRabbitMQ.Order.Application.Interfaces;
using TicketFlowRabbitMQ.Order.Domain.Helpers;
using TicketFlowRabbitMQ.Order.Domain.Models;

namespace TicketFlowRabbitMQ.Order.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SalesController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public SalesController(IOrderService orderService)
        {
            _orderService = orderService;
        }
        [HttpPost]
        public async Task<IActionResult> CreateNewOrder([FromBody] OrderDTO model)
        {
            try
            {
                //------------------------------------------------------------------------------------------------
                // R1. Check all parameters
                //------------------------------------------------------------------------------------------------
                if (ModelState.IsValid is false) return BadRequest(StringHelper.MISSING_PARAMETERS);

                //------------------------------------------------------------------------------------------------
                // R2. Mapping | DTO M-> Model
                //------------------------------------------------------------------------------------------------
                var newUser = Domain.Models.Order.Create(
                    userId: model.UserId,
                    eventId: model.EventId,
                    quantity: model.Quantity,
                    totalPrice: model.TotalPrice
                );

                //------------------------------------------------------------------------------------------------
                // R3. Call App Service and create new user and send to RabbitMQ QUEUE
                //------------------------------------------------------------------------------------------------
                var orderObj = await _orderService.CreateNewOrderProcessing(newUser);
                if(orderObj.Id.Equals(Guid.Empty)) return BadRequest("ERR03-We can't create the order right now. Try Again Later");

                //------------------------------------------------------------------------------------------------
                // R4. User Response
                //------------------------------------------------------------------------------------------------
                return Accepted(new
                {
                    orderId = orderObj.Id,
                    message = "Order received and is being processed asynchronously."
                });

            }
            catch (Exception ex)
            {
                return Problem(
                   detail: $"ERR05-Internal server error. Can't create a order right now.{ex.Message[..150]}",
                   statusCode: StatusCodes.Status500InternalServerError
               );
            }
        }
    }
}
