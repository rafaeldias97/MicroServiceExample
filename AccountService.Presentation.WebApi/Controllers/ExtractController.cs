using AccountService.Domain.Interfaces.Handlers;
using AccountService.Domain.Queries.Requests;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace AccountService.Presentation.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExtractController : ControllerBase
    {

        [HttpGet]
        public async Task<IActionResult> GetExtract(
            [FromServices] IGetExtractQueryHandler handler,
            [FromQuery] ExtractAccountRequest extract
            )
        {
            var _extract = await handler.Handle(extract);
            return Ok(_extract);
        }
    }
}