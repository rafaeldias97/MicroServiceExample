using AccountService.Domain.Commands.Reponses;
using MediatR;
using System;

namespace AccountService.Domain.Commands.Requests
{
    public class TransferAccountRequest : IRequest<TransferAccountResponse>
    {
        public AccountRequest To { get; set; }
        public AccountRequest From { get; set; }
    }

    public class AccountRequest
    {
        public long Number { get; set; }
        public DateTime DueDate { get; set; }
        public short SecurityCode { get; set; }
        public double Value { get; set; }
    }
}
