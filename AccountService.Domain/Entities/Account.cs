using System;

namespace AccountService.Domain.Entities
{
    public class Account
    {
        public Account()
        {
            CreationDate = DateTime.Now;
        }
        public Account(long number, DateTime dueDate, short securityCode)
        {
            Number = number;
            DueDate = dueDate;
            SecurityCode = securityCode;
            CreationDate = DateTime.Now;
        }
        public int Id { get; set; }
        public long Number { get; set; }
        public DateTime DueDate { get; set; }
        public short SecurityCode { get; set; }
        public double Value { get; set; }
        public DateTime CreationDate { get; set; }
    }
}
