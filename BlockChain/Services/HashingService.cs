using BlockChain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlockChain.Services
{
    public class HashingService
    {
        public string ComputeHash(Block block)
        {
            string input = $"{block.Index}{block.TimeStamp.ToString("o")}{block.Data}{block.Author}{block.PrevHash}";
            return ComputeHash(input);
        }

        public string ComputeHash(string input)
        {
            using (var sha256 = System.Security.Cryptography.SHA256.Create())
            {
                var inputBytes = Encoding.UTF8.GetBytes(input);
                byte[] hashBytes = sha256.ComputeHash(inputBytes);
                return BitConverter.ToString(hashBytes).Replace("-", "").ToLowerInvariant();
            }
        }
    }
}
