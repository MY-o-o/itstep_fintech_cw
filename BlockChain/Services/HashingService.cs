using BlockChain.Models;
using System.Text;

namespace BlockChain.Services
{
    public class HashingService
    {
        public string ComputeHash(Block block)
        {
            string input = $"{block.Index}{block.TimeStamp:o}{block.Data}{block.Author}{block.PrevHash}{block.Difficulty}{block.Nonce}";
            return ComputeHash(input);
        }

        public string ComputeHash(string input)
        {
            var inputBytes = Encoding.UTF8.GetBytes(input);
            var hashBytes = System.Security.Cryptography.SHA256.HashData(inputBytes);
            return Convert.ToHexString(hashBytes).ToLowerInvariant();
        }
    }
}
