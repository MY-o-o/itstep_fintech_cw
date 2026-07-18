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

        public string Hash { get; set; }

        public Block(int index, DateTime timeStamp, string data, string author, string prevHash)
        {
            Index = index;
            TimeStamp = timeStamp;
            Data = data;
            Author = author;
            PrevHash = prevHash;
        }
    }
}
