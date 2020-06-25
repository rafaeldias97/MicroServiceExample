using AccountService.Domain.Commands.Requests;
using Microsoft.AspNetCore.Mvc;
using RabbitMQ.EventBus.Interfaces;

namespace AccountService.Presentation.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransferAccountController : ControllerBase
    {
        private readonly IEventBus @event;
        public TransferAccountController(IEventBus @event)
        {
            this.@event = @event;
        }

        [HttpPost]
        public IActionResult Transfer(
            [FromBody] TransferAccountRequest request
            )
        {
            @event.Publish(request);
            return Ok(request);
        }
    }
}