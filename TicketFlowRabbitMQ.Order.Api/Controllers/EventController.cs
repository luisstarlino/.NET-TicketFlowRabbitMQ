using Microsoft.AspNetCore.Mvc;
using System.Globalization;
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
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

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetEventByUUID(Guid id)
        {

            try
            {
                //------------------------------------------------------------------------------------------------
                // R1. App Service
                //------------------------------------------------------------------------------------------------
                var eventDB = await _eventService.GetUniqueEventById(id);
                if (eventDB is null ) return NotFound("Event not found!");

                //------------------------------------------------------------------------------------------------
                // R2. Return
                //------------------------------------------------------------------------------------------------
                return Ok(eventDB);
            }
            catch (Exception ex)
            {
                return Problem(
                    detail: $"ERR05-Internal server error. Can't fetch event right now.{ex.Message[..150]}",
                    statusCode: StatusCodes.Status500InternalServerError
                );
            }

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPut("{id:guid}")]
        public async Task<IActionResult> UpdateEvent(Guid id, [FromBody] UpdateEventDTO model)
        {
            try
            {
                //------------------------------------------------------------------------------------------------
                // R1. Check all parameters
                //------------------------------------------------------------------------------------------------
                if (model is null) return BadRequest(StringHelper.MISSING_PARAMETERS);

                //------------------------------------------------------------------------------------------------
                // R2. Fetch current event
                //------------------------------------------------------------------------------------------------
                var existEvent = await _eventService.GetUniqueEventById(id);
                if (existEvent == null) return NotFound("Event not found!");

                //------------------------------------------------------------------------------------------------
                // R3. Update fields
                //------------------------------------------------------------------------------------------------
                existEvent!.Title = model.Title ?? existEvent.Title;
                existEvent.Description = model.Description ?? existEvent.Description;
                existEvent.Location = model.Location ?? existEvent.Location;

                if (!model!.Date!.IsEmpty())
                {
                    DateTime.TryParseExact(
                        model.Date,
                        "dd/MM/yyyy",
                        CultureInfo.InvariantCulture,
                        DateTimeStyles.AssumeUniversal | DateTimeStyles.AdjustToUniversal,
                    out DateTime dateObj);

                    existEvent.Date = dateObj;
                }

                //------------------------------------------------------------------------------------------------
                // R3. Call App Service to update
                //------------------------------------------------------------------------------------------------
                var hasUpdated = await _eventService.UpdateEvent(existEvent);

                if (hasUpdated is null) return BadRequest("Can't update user right now");

                //------------------------------------------------------------------------------------------------
                // R4. Mapping 
                //------------------------------------------------------------------------------------------------
                return Ok(hasUpdated);
            }
            catch (Exception ex)
            {
                return Problem(
                    detail: $"ERR05-Internal server error. Can't update event right now.{ex.Message[..150]}",
                    statusCode: StatusCodes.Status500InternalServerError
                );
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPut("{id:guid}/sales-info")]
        public async Task<IActionResult> UpdateSalesEventInfo(Guid id, [FromBody] UpdateEventSalesInfoDTO model)
        {
            try
            {
                //------------------------------------------------------------------------------------------------
                // R1. Check all parameters
                //------------------------------------------------------------------------------------------------
                if (model is null) return BadRequest(StringHelper.MISSING_PARAMETERS);

                //------------------------------------------------------------------------------------------------
                // R2. Fetch current event
                //------------------------------------------------------------------------------------------------
                var existEvent = await _eventService.GetUniqueEventById(id);
                if (existEvent == null) return NotFound("Event not found!");

                //------------------------------------------------------------------------------------------------
                // R3. Update fields
                //------------------------------------------------------------------------------------------------
                existEvent.UpdateSalesInfo(model.TicketPrice, model.AvailableTickets);

                //------------------------------------------------------------------------------------------------
                // R3. Call App Service to update
                //------------------------------------------------------------------------------------------------
                var hasUpdated = await _eventService.UpdateEvent(existEvent);

                if (hasUpdated is null) return BadRequest("Can't update event right now");

                //------------------------------------------------------------------------------------------------
                // R4. Mapping 
                //------------------------------------------------------------------------------------------------
                return Ok(hasUpdated);
            }
            catch (Exception ex)
            {
                return Problem(
                    detail: $"ERR05-Internal server error. Can't update event price right now.{ex.Message[..150]}",
                    statusCode: StatusCodes.Status500InternalServerError
                );
            }
        }


    }
}
