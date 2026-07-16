using BlockChain.Services;

var blockChainService = new BlockChainService();

blockChainService.AddBlock("Mark sent 10 coins to John");
blockChainService.AddBlock("John sent 5 coins to Alice");

BlockChainDisplayService.DisplayBlockChain(blockChainService.Chain);
BlockChainDisplayService.DisplayValidationResult(blockChainService.IsValid());
Console.WriteLine(Environment.NewLine);

blockChainService.Chain[2].Data = "John sent 50 coins to Alice"; // Tampering with the blockchain
BlockChainDisplayService.DisplayBlockChain(blockChainService.Chain);
BlockChainDisplayService.DisplayValidationResult(blockChainService.IsValid());