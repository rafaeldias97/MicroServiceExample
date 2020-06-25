using RabbitMQ.EventBus.Interfaces;
using System;

namespace AccountService.Domain.Commands.Requests
{
    public class GenerateExtractRequest : Event
    {

        public GenerateExtractRequest(DateTime datTransaction, string historic, double value, long numberAccount, double balance)
        {
            DatTransaction = datTransaction;
            Historic = historic;
            Value = value;
            Balance = balance;
            NumberAccount = numberAccount;
        }

        public DateTime DatTransaction { get; set; }
        public string Historic { get; set; }
        public double Value { get; set; }
        public long NumberAccount { get; set; }
        public double Balance { get; set; }
    }
}
