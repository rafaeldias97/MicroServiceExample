using System;

namespace AccountService.Domain.Queries.Reponses
{
    public class ExtractAccountResponse
    {
        public int Id { get; set; }
        public int IdFrom { get; set; }
        public int IdTo { get; set; }
        public double Value { get; set; }
        public DateTime CreationDate { get; set; }
    }
}
