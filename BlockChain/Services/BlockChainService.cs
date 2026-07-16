using BlockChain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlockChain.Services
{
    public class BlockChainService
    {
        public List<Block> Chain { get; set; }
        private readonly HashingService _hashingService = new HashingService();

        public BlockChainService()
        {
            Chain = new List<Block>();
            CreateGenesisBlock();
        }

        private void CreateGenesisBlock()
        {
            var genesisBlock = new Block(0, DateTime.UtcNow, "Genesis Block", "0");

            genesisBlock.Hash = _hashingService.ComputeHash(genesisBlock);
            Chain.Add(genesisBlock);
        }

        public void AddBlock(string data)
        {
            var lastBlock = Chain.Last();
            var newBlock = new Block(lastBlock.Index + 1, DateTime.UtcNow, data, lastBlock.Hash);
            newBlock.Hash = _hashingService.ComputeHash(newBlock);
            Chain.Add(newBlock);
        }

        public bool IsValid()
        {
            for (int i = 1; i < Chain.Count; i++)
            {
                var currentBlock = Chain[i];
                var previousBlock = Chain[i - 1];
                if (currentBlock.Hash != _hashingService.ComputeHash(currentBlock)) return false;
                if (currentBlock.PrevHash != previousBlock.Hash) return false;
            }
            return true;
        }
    }
}
