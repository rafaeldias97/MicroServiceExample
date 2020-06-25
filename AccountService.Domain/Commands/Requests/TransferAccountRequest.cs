using RabbitMQ.EventBus.Interfaces;
using System;

namespace AccountService.Domain.Commands.Requests
{
    public class TransferAccountRequest : Event
    {
        public AccountRequest To { get; set; }
        public AccountRequest From { get; set; }
        public double Value { get; set; }
    }

    public class AccountRequest
    {
        public long Number { get; set; }
        public DateTime DueDate { get; set; }
        public short SecurityCode { get; set; }
    }
}
