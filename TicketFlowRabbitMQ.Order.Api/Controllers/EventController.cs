using Microsoft.AspNetCore.Mvc;
using TicketFlowRabbitMQ.Order.Api.DTOs;
using TicketFlowRabbitMQ.Order.Application.Interfaces;
using TicketFlowRabbitMQ.Order.Domain.Helpers;
using TicketFlowRabbitMQ.Order.Domain.Models;

namespace TicketFlowRabbitMQ.Order.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class EventController : ControllerBase
    {
        private readonly IEventService _eventService;

        public EventController(IEventService eventService)
        {
            _eventService = eventService;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Event>>> GetAll()
        {
            try
            {
                var events = await _eventService.GetAll();
                if (!events.Any()) return NoContent();
                return Ok(events);
            }
            catch (Exception ex)
            {
                return BadRequest("Internal Server Error");
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddNewEvent([FromBody] EventDTO model)
        {
            try
            {
                //------------------------------------------------------------------------------------------------
                // R1. Check all parameters
                //------------------------------------------------------------------------------------------------
                if (ModelState.IsValid is false) return BadRequest(StringHelper.MISSING_PARAMETERS);

                //------------------------------------------------------------------------------------------------
                // R2. Mapping | DTO <--> Model
                //------------------------------------------------------------------------------------------------
                var newEvent = Event.Create(model.Title, model.Description, model.Location, model.Date, model.TicketPrice, model.AvailableTickets);

                //------------------------------------------------------------------------------------------------
                // R3. Call App Service and create new user
                //------------------------------------------------------------------------------------------------
                var eventAdded = await _eventService.ProcessAndSaveNewEvent(newEvent);
                if (eventAdded.Equals(Guid.Empty)) return BadRequest("ERR03-We can't create this event right now. Try Again Later");

                return CreatedAtAction(nameof(AddNewEvent), eventAdded);
            }
            catch (Exception ex)
            {
                return Problem(
                    detail: $"ERR05-Internal server error. Can't create this event right now.{ex.Message[..150]}",
                    statusCode: StatusCodes.Status500InternalServerError
                );
            }
        }
    }
}
