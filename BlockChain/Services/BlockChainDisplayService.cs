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
                Console.WriteLine($"Previous Hash: {block.PrevHash}");
                Console.WriteLine($"Difficulty: {block.Difficulty}");
                Console.WriteLine($"Nonce: {block.Nonce}");
                Console.WriteLine($"Hash: {block.Hash}" + Environment.NewLine);

                Console.WriteLine("Transactions:");
                DisplayTransactions(block.Transactions);
                Console.WriteLine(new string('-', 50) + Environment.NewLine);
            }
        }

        public static void DisplayTransactions(List<Transaction> transactions)
        {
            foreach (var transaction in transactions)
            {
                Console.WriteLine(transaction);
                Console.WriteLine(new string('-', 30));
            }
        }

        public static void DisplayValidationResult(bool isValid)
        {
            if (isValid) Console.WriteLine("The blockchain is valid.");
            else Console.WriteLine("The blockchain is NOT valid.");
        }
    }
}
