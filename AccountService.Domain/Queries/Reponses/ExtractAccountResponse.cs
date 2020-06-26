using System;

namespace AccountService.Domain.Queries.Reponses
{
    public class ExtractAccountResponse
    {
        public ExtractAccountResponse()
        {
            Id = Guid.NewGuid();
        }

        public ExtractAccountResponse(DateTime datTransaction, string historic, double value, long numberAccount, double balance)
        {
            DatTransaction = datTransaction;
            Historic = historic;
            Value = value;
            Balance = balance;
            NumberAccount = numberAccount;
        }

        public Guid Id { get; set; }
        public DateTime DatTransaction { get; set; }
        public string Historic { get; set; }
        public double Value { get; set; }
        public long NumberAccount { get; set; }
        public double Balance { get; set; }
    }
}
