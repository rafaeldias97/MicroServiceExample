using System;

namespace AccountService.Domain.Queries.Reponses
{
    public class ExtractAccountResponse
    {
        public int Id { get; set; }
        public long NumberAccountFrom { get; set; }
        public long NumberAccountTo { get; set; }
        public double Value { get; set; }
        public DateTime CreationDate { get; set; }
    }
}
