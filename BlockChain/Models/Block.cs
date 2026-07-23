using System;
using System.Collections.Generic;
using System.Text;

namespace BlockChain.Models
{
    public class Block
    {
        public int Index { get; set; }
        public DateTime TimeStamp { get; set; }
        public List<Transaction> Transactions { get; set; } = new List<Transaction>();
        public string PrevHash { get; set; }
        public int Difficulty { get; set; }
        public long Nonce { get; set; } = 0;
        
        public string Hash { get; set; } = string.Empty;

        public Block(int index, List<Transaction> transactions, string prevHash, int difficulty)
        {
            Index = index;
            TimeStamp = DateTime.UtcNow;
            Transactions = transactions;
            PrevHash = prevHash;
            Difficulty = difficulty;
        }

        public string ToRowString()
        {
            string transactionsRow = string.Concat(Transactions.Select(t => t.ToRowString()));

            return $"{Index}{TimeStamp:o}{transactionsRow}{PrevHash}{Difficulty}{Nonce}";
        }
    }
}
