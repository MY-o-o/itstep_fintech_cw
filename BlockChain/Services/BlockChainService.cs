using BlockChain.Models;

namespace BlockChain.Services
{
    public class BlockChainService
    {
        private readonly HashingService _hashingService;
        private readonly MiningService _miningService;
        public List<Block> Chain { get; set; }
        public int Difficulty { get; set; } = 4;
        public string TargetPrefix { get; set; } = "cafe"; //cafe, dead, beef, face, bad, bead, feed, 1234, c0de

        public BlockChainService(HashingService hashingService, MiningService miningService)
        {
            _hashingService = hashingService;
            _miningService = miningService;
            Chain = new List<Block>();
            CreateGenesisBlock();
        }

        private void CreateGenesisBlock()
        {
            var genesisBlock = new Block(0, DateTime.UtcNow, "Genesis Block", "Genesis Author", "0");

            _miningService.MineBlock(genesisBlock, Difficulty, showProgress: false);
            Chain.Add(genesisBlock);
        }

        public void AddBlock(string data, string author, bool useTargetPrefix = false)
        {
            var lastBlock = Chain.Last();
            var newBlock = new Block(lastBlock.Index + 1, DateTime.UtcNow, data, author, lastBlock.Hash);
            if (useTargetPrefix) _miningService.MineBlock(newBlock, TargetPrefix);
            else _miningService.MineBlock(newBlock, Difficulty);
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
                if (!currentBlock.Hash.StartsWith(new string('0', Difficulty))) return false;
            }
            return true;
        }
    }
}
