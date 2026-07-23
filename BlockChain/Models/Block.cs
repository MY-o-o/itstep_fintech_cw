using System;
using System.Collections.Generic;
using System.Text;

namespace BlockChain.Models
{
    public class Block
    {
        public int Index { get; set; }
        public DateTime TimeStamp { get; set; }
        public string Data { get; set; }
        public string Author { get; set; }
        public string PrevHash { get; set; }
        public int Difficulty { get; set; }
        public long Nonce { get; set; } = 0;
        
        public string Hash { get; set; } = string.Empty;

        public Block(int index, string data, string author, string prevHash, int difficulty)
        {
            Index = index;
            TimeStamp = DateTime.UtcNow;
            Data = data;
            Author = author;
            PrevHash = prevHash;
            Difficulty = difficulty;
        }
    }
}
