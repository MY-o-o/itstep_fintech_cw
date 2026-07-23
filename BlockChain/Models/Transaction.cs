using System;
using System.Collections.Generic;
using System.Text;

namespace BlockChain.Models
{
    public class Transaction
    {
        public Guid Id { get; set; }
        public string From { get; set; }
        public string To { get; set; }
        public decimal Amount { get; set; }
        public DateTime TimeStamp { get; set; }

        public Transaction(string from, string to, decimal amount)
        {
            Id = Guid.NewGuid();
            From = from;
            To = to;
            Amount = amount;
            TimeStamp = DateTime.UtcNow;
        }

        public string ToRowString()
        {
            return $"{Id}\t{From}\t{To}\t{Amount}\t{TimeStamp}";
        }

        public override string ToString()
        {
            return $"Transaction [Id={Id}, From={From}, To={To}, Amount={Amount}, TimeStamp={TimeStamp}]";
        }
    }
}
