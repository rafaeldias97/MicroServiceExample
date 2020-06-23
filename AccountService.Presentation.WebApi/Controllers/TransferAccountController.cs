using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AccountService.Domain.Commands.Requests;
using AccountService.Domain.Interfaces.Handlers;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AccountService.Presentation.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransferAccountController : ControllerBase
    {
        private readonly ITransferAccountHandler command;
        public TransferAccountController(ITransferAccountHandler command)
        {
            this.command = command;
        }

        [HttpPost]
        public IActionResult Transfer(
            [FromBody] TransferAccountRequest request
            )
        {
            var res = command.Handle(request);
            return Ok(res);
        }
    }
}