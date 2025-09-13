using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;
using System.Net;
using TicketFlowRabbitMQ.Order.Api.DTOs;
using TicketFlowRabbitMQ.Order.Application.Interfaces;
using TicketFlowRabbitMQ.Order.Application.Services;
using TicketFlowRabbitMQ.Order.Domain.Helpers;
using TicketFlowRabbitMQ.Order.Domain.Models;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace TicketFlowRabbitMQ.Order.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _service;

        public UserController(IUserService service)
        {
            _service = service;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetAll()
        {
            try
            {
                var users = await _service.GetAll();
                if (!users.Any()) return NoContent();
                return Ok(users);
            }
            catch (Exception ex)
            {
                return BadRequest("Internal Server Error");
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddNewUser([FromBody] UserDTO model)
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
                var newUser = Domain.Models.User.Create(
                    model.Name,
                    model.Email,
                    model.Password,
                    model.Phone,
                    model.BirthDate
                );

                //------------------------------------------------------------------------------------------------
                // R3. Call App Service and create new user
                //------------------------------------------------------------------------------------------------
                var addedUser = await _service.ProcessAndSaveNewUser(newUser);
                if (addedUser.Equals(Guid.Empty)) return BadRequest("ERR03-We can't create the user right now. Try Again Later");

                return CreatedAtAction(nameof(AddNewUser), addedUser);
            }
            catch (Exception ex)
            {
                return Problem(
                    detail: $"ERR05-Internal server error. Can't create user right now.{ex.Message[..150]}",
                    statusCode: StatusCodes.Status500InternalServerError
                );
            }
        }

        [HttpGet("{id:guid}")] // Use a type constraint for UUIDs
        public async Task<IActionResult> GetUserByUUID(Guid id)
        {

            try
            {
                //------------------------------------------------------------------------------------------------
                // R1. App Service
                //------------------------------------------------------------------------------------------------
                var user = await _service.GetUniqueUserById(id);
                if (user == null) return NotFound("User not found!");

                //------------------------------------------------------------------------------------------------
                // R2. Mapping
                //------------------------------------------------------------------------------------------------
                var response = new UserDTOResponse
                {
                    Id = user.Id,
                    Name = user.Name,
                    Email = user.Email,
                    Phone = user.Phone,
                    BirthDate = user.BirthDate.ToString("dd/MM/yyyy")
                };


                return Ok(response);
            }
            catch (Exception ex)
            {
                return Problem(
                    detail: $"ERR05-Internal server error. Can't fetch user right now.{ex.Message[..150]}",
                    statusCode: StatusCodes.Status500InternalServerError
                );
            }

        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> UpdateUser(Guid id, [FromBody] UpdateUserDTO model)
        {
            try
            {
                //------------------------------------------------------------------------------------------------
                // R1. Check all parameters
                //------------------------------------------------------------------------------------------------
                if (model is null) return BadRequest(StringHelper.MISSING_PARAMETERS);

                //------------------------------------------------------------------------------------------------
                // R2. Fetch current user
                //------------------------------------------------------------------------------------------------
                var existUser = await _service.GetUniqueUserById(id);
                if (existUser == null) NotFound("User not found!");

                //------------------------------------------------------------------------------------------------
                // R3. Update fields
                //------------------------------------------------------------------------------------------------
                existUser!.Name  = model.Name  ?? existUser.Name;
                existUser.Email  = model.Email ?? existUser.Email;
                existUser.Phone  = model.Phone ?? existUser.Phone;

                if (!model!.BirthDate!.IsEmpty())
                {
                    DateTime.TryParseExact(
                        model.BirthDate,
                        "dd/MM/yyyy",
                        CultureInfo.InvariantCulture,
                        DateTimeStyles.AssumeUniversal | DateTimeStyles.AdjustToUniversal,
                    out DateTime newBirthDate);

                    existUser.BirthDate = newBirthDate;
                }

                //------------------------------------------------------------------------------------------------
                // R3. Call App Service to update
                //------------------------------------------------------------------------------------------------
                var hasUpdated = await _service.UpdateUser(existUser);
                if (hasUpdated is null) return BadRequest("Can't update user right now");

                //------------------------------------------------------------------------------------------------
                // R4. Mapping
                //------------------------------------------------------------------------------------------------
                var response = new UserDTOResponse
                {
                    Id = id,
                    Name = hasUpdated.Name,
                    Email = hasUpdated.Email,
                    Phone = hasUpdated.Phone,
                    BirthDate = hasUpdated.BirthDate.ToString("dd/MM/yyyy")
                };

                return Ok(response);
            }
            catch (Exception ex)
            {
                return Problem(
                    detail: $"ERR05-Internal server error. Can't update user right now.{ex.Message[..150]}",
                    statusCode: StatusCodes.Status500InternalServerError
                );
            }
        }



    }
}
