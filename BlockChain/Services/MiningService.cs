using BlockChain.Models;
using System.Diagnostics;

namespace BlockChain.Services
{
    public class MiningService
    {
        private readonly HashingService _hashingService;

        public MiningService(HashingService hashingService)
        {
            _hashingService = hashingService;
        }

        public long MineBlock(Block block, int difficulty, bool showProgress = true)
        {
            var prefix = new string('0', difficulty);
            long nonce = 0;
            string[] loader = ["⠋", "⠙", "⠹", "⠸", "⠼", "⠴", "⠦", "⠧", "⠇", "⠏"];
            short loaderIndex = 0;
            double speed = 0;
            Stopwatch sw = Stopwatch.StartNew();
            while (true)
            {
                block.Nonce = nonce;
                block.Hash = _hashingService.ComputeHash(block);

                if (block.Hash.StartsWith(prefix))
                {
                    sw.Stop();
                    if (showProgress)
                    {
                        Console.Clear();
                        Console.WriteLine($"\nBlock mined successfully! Nonce: {nonce}, Time taken: {sw.ElapsedMilliseconds} ms");
                    }
                    return nonce;
                }
                if (showProgress && nonce % 80000 == 0)
                {
                    Console.Clear();

                    Console.Write(loader[loaderIndex]);
                    loaderIndex = (short)((loaderIndex + 1) % loader.Length);

                    speed = nonce / sw.Elapsed.TotalSeconds;
                    string textToRight = $"H/s: {speed:F2}  Total Seconds: {sw.Elapsed.TotalSeconds:F2}\r";

                    int leftPosition = Console.WindowWidth - textToRight.Length;
                    Console.SetCursorPosition(leftPosition, Console.CursorTop);
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.Write(textToRight);
                    Console.ResetColor();

                }
                nonce++;
            }
        }

        public long MineBlock(Block block, string targetPrefix, bool showProgress = true)
        {
            long nonce = 0;
            string[] loader = ["⠋", "⠙", "⠹", "⠸", "⠼", "⠴", "⠦", "⠧", "⠇", "⠏"];
            short loaderIndex = 0;
            double speed = 0;
            Stopwatch sw = Stopwatch.StartNew();
            while (true)
            {
                block.Nonce = nonce;
                block.Hash = _hashingService.ComputeHash(block);

                if (block.Hash.StartsWith(targetPrefix))
                {
                    sw.Stop();
                    if (showProgress)
                    {
                        Console.Clear();
                        Console.WriteLine($"\nBlock mined successfully! Nonce: {nonce}, Time taken: {sw.ElapsedMilliseconds} ms");
                    }
                    return nonce;
                }
                if (showProgress && nonce % 80000 == 0)
                {
                    Console.Clear();

                    Console.Write(loader[loaderIndex]);
                    loaderIndex = (short)((loaderIndex + 1) % loader.Length);

                    speed = nonce / sw.Elapsed.TotalSeconds;
                    string textToRight = $"H/s: {speed:F2}  Total Seconds: {sw.Elapsed.TotalSeconds:F2}\r";

                    int leftPosition = Console.WindowWidth - textToRight.Length;
                    Console.SetCursorPosition(leftPosition, Console.CursorTop);
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.Write(textToRight);
                    Console.ResetColor();

                }
                nonce++;
            }
        }
    }
}
