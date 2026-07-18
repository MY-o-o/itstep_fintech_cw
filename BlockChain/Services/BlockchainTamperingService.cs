using System;
using System.Collections.Generic;
using System.Text;

namespace BlockChain.Services
{
    public static class BlockchainTamperingService
    {
        private static readonly HashingService _hashingService = new HashingService();
        public static void TamperWithBlockChain(BlockChainService blockChainService, int blockIndex = 1)
        {
            if (blockIndex < 1 || blockIndex >= blockChainService.Chain.Count)
            {
                throw new ArgumentOutOfRangeException(nameof(blockIndex), "Invalid block index.");
            }

            // Recalculate the hash of the tampered block
            var tamperedBlock = blockChainService.Chain[blockIndex];
            tamperedBlock.Hash = _hashingService.ComputeHash(tamperedBlock);

            // Update the previous hash of the next block, if it exists
            if (blockIndex + 1 < blockChainService.Chain.Count)
            {
                blockChainService.Chain[blockIndex + 1].PrevHash = tamperedBlock.Hash;
                TamperWithBlockChain(blockChainService, blockIndex + 1);
            }
        }
    }
}
