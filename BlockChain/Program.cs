using BlockChain.Services;
using System.Text;
using System.Text.RegularExpressions;

static bool IsValidHashPrefix(string input)
{
    if (string.IsNullOrEmpty(input)) return false;
    return Regex.IsMatch(input, @"^[0-9a-fA-F]{1,64}$");
}



var hashingService = new HashingService();
var miningService = new MiningService(hashingService);
var blockChainService = new BlockChainService(hashingService, miningService);

Console.OutputEncoding = Encoding.UTF8;
while (true)
{
    Console.WriteLine("Block Management Menu:");
    Console.WriteLine("1. Add a new block");
    Console.WriteLine("2. Display the blockchain");
    Console.WriteLine("3. Validate the blockchain");
    Console.WriteLine("4. Change difficulty ++");
    Console.WriteLine("5. Change difficulty --");
    Console.WriteLine("6. Change target prefix");
    Console.WriteLine("7. Exit");
    Console.Write("Enter your choice: ");
    var selectedOption = Console.ReadLine();

    switch (selectedOption)
    {
        case "1":
            Console.Write("Enter block data: ");
            var data = Console.ReadLine();
            Console.Write("Enter block author: ");
            var author = Console.ReadLine();

            Console.WriteLine(Environment.NewLine + "Choose hashing method:");
            Console.WriteLine("1. Difficulty (zeros)");
            Console.WriteLine("2. Target prefix");
            Console.WriteLine("3. Cancel");
            Console.Write("Enter your choice: ");
            var methodChoice = Console.ReadLine();

            switch (methodChoice)
            {
                case "1":
                    blockChainService.AddBlock(data, author);
                    break;
                case "2":
                    blockChainService.AddBlock(data, author, true);
                    break;
                case "3":
                    Console.WriteLine("Block addition canceled.");
                    break;
                default:
                    Console.WriteLine("Invalid option. Please try again.");
                    break;
            }
            break;
        case "2":
            BlockChainDisplayService.DisplayBlockChain(blockChainService.Chain);
            break;
        case "3":
            BlockChainDisplayService.DisplayValidationResult(blockChainService.IsValid());
            break;
        case "4":
            blockChainService.Difficulty++;
            Console.WriteLine($"Difficulty increased to {blockChainService.Difficulty}");
            break;
        case "5":
            if (blockChainService.Difficulty > 1)
            {
                blockChainService.Difficulty--;
                Console.WriteLine($"Difficulty decreased to {blockChainService.Difficulty}");
            }
            else Console.WriteLine("Difficulty cannot be less than 1.");
            break;
        case "6":
            Console.Write("Enter target prefix (allowed symbols: a-f, A-F, 0-9): ");
            var prefix = Console.ReadLine();
            if (!IsValidHashPrefix(prefix))
            {
                Console.WriteLine("Invalid target prefix. It must be a hexadecimal string (0-9, a-f, A-F) with a maximum length of 64 characters.");
            }
            else
            {
                blockChainService.TargetPrefix = prefix;
                Console.WriteLine($"Target prefix changed to {blockChainService.TargetPrefix}");
            }
            break;
        case "7":
            return;
        default:
            Console.WriteLine("Invalid option. Please try again.");
            break;
    }
    Console.Write("Press any key to continue...");
    Console.ReadKey();
    Console.Clear();
}