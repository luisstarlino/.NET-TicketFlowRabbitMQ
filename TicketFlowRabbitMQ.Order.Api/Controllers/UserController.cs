using Microsoft.AspNetCore.Mvc;
using TicketFlowRabbitMQ.Order.Application.Interfaces;
using TicketFlowRabbitMQ.Order.Domain.Models;

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

        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetAll()
        {
            var users = await _service.GetAll();
            if (!users.Any()) return NoContent();
            return Ok(users);
        }

    }
}
