using BlockChain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlockChain.Services
{
    public static class BlockChainDisplayService
    {
        public static void DisplayBlockChain(List<Block> chain)
        {
            foreach (var block in chain)
            {
                Console.WriteLine($"Index: {block.Index}");
                Console.WriteLine($"Timestamp: {block.TimeStamp:o}");
                Console.WriteLine($"Data: {block.Data}");
                Console.WriteLine($"Author: {block.Author}");
                Console.WriteLine($"Previous Hash: {block.PrevHash}");
                Console.WriteLine($"Difficulty: {block.Difficulty}");
                Console.WriteLine($"Nonce: {block.Nonce}");
                Console.WriteLine($"Hash: {block.Hash}");
                Console.WriteLine(new string('-', 50) + Environment.NewLine);
            }
        }

        public static void DisplayValidationResult(bool isValid)
        {
            if (isValid) Console.WriteLine("The blockchain is valid.");
            else Console.WriteLine("The blockchain is NOT valid.");
        }
    }
}
