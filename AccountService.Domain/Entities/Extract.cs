using System;

namespace AccountService.Domain.Entities
{
    public class Extract
    {
        public Extract()
        {
            Id = Guid.NewGuid();
        }

        public Extract(DateTime datTransaction, string historic, double value, long numberAccount, double balance)
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
