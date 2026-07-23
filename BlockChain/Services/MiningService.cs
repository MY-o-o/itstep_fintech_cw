using BlockChain.Models;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Transactions;

namespace BlockChain.Services;

public class MiningService
{
    private const long RangeSize = 50_000;
    private static readonly char[] LoaderFrames = ['⠋', '⠙', '⠹', '⠸', '⠼', '⠴', '⠦', '⠇', '⠏'];
    private static readonly char[] MeasureUnits = [' ', 'k', 'M', 'G', 'T', 'P', 'E'];
    private readonly HashingService _hashingService;

    public MiningService(HashingService hashingService)
    {
        _hashingService = hashingService;
    }

    public long MineBlock(Block block, int difficulty, bool showProgress = true)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(difficulty);
        ArgumentNullException.ThrowIfNull(block);

        string targetPrefix = new string('0', difficulty);

        string transactionsRow = string.Concat(block.Transactions.Select(t => t.ToRowString()));
        string blockPrefix = $"{block.Index}{block.TimeStamp:o}{transactionsRow}{block.PrevHash}{block.Difficulty}";

        long attempts = 0;
        long winningNonce = -1;
        string? winningHash = null;
        var stopwatch = Stopwatch.StartNew();
        using var statisticsCancellation = new CancellationTokenSource();
        var statisticsTask = showProgress
            ? ShowStatisticsAsync(() => Interlocked.Read(ref attempts), stopwatch, statisticsCancellation.Token)
            : Task.CompletedTask;

        var options = new ParallelOptions
        {
            MaxDegreeOfParallelism = Environment.ProcessorCount
        };

        try
        {
            Parallel.ForEach(
                Partitioner.Create(0L, long.MaxValue, RangeSize),
                options,
                (range, loopState) =>
                {
                    // Publish batches instead of incrementing one contended global counter
                    // for every hash. The monitor only needs an approximate live total.
                    long localAttempts = 0;
                    try
                    {
                        for (var nonce = range.Item1; nonce < range.Item2; nonce++)
                        {
                            if (loopState.ShouldExitCurrentIteration)
                            {
                                return;
                            }

                            var hash = _hashingService.ComputeHash(blockPrefix + nonce);
                            localAttempts++;

                            if (!hash.StartsWith(targetPrefix, StringComparison.Ordinal))
                            {
                                continue;
                            }

                            if (Interlocked.CompareExchange(ref winningNonce, nonce, -1) == -1)
                            {
                                winningHash = hash;
                                loopState.Stop();
                            }

                            return;
                        }
                    }
                    finally
                    {
                        Interlocked.Add(ref attempts, localAttempts);
                    }
                });
        }
        finally
        {
            stopwatch.Stop();
            statisticsCancellation.Cancel();
            statisticsTask.GetAwaiter().GetResult();
        }

        // A completed parallel loop can only occur without a winner on nonce overflow.
        if (winningNonce < 0 || winningHash is null)
        {
            throw new InvalidOperationException("No matching nonce was found.");
        }

        block.Nonce = winningNonce;
        block.Hash = winningHash;

        if (showProgress)
        {
            WriteMiningResult(winningNonce, attempts, stopwatch.Elapsed);
        }

        return winningNonce;
    }

    private static (double hashRate, short measureUnitIndex, string timeTaken) CalculateStatistics(long attempts, TimeSpan elapsed)
    {
        short measureUnitIndex = 0;
        double hashRate = attempts / Math.Max(elapsed.TotalSeconds, double.Epsilon);
        for (measureUnitIndex = 0; hashRate >= 1000 && measureUnitIndex < MeasureUnits.Length - 1; measureUnitIndex++)
        {
            hashRate /= 1000;
        }

        string daysTaken = elapsed.Days != 0 ? elapsed.Days + "d " : string.Empty;
        string hoursTaken = elapsed.Hours != 0 || elapsed.Days != 0 ? elapsed.Hours + "h " : string.Empty;
        string minutesTaken = elapsed.Minutes != 0 || elapsed.Hours != 0 || elapsed.Days != 0 ? elapsed.Minutes + "m " : string.Empty;
        string timeTaken = $"{daysTaken}{hoursTaken}{minutesTaken}{elapsed.Seconds}.{elapsed.Milliseconds:000}s";

        return (hashRate, measureUnitIndex, timeTaken);
    }

    private static void WriteMiningResult(long nonce, long attempts, TimeSpan elapsed)
    {
        var (hashRate, measureUnitIndex, timeTaken) = CalculateStatistics(attempts, elapsed);
        Console.WriteLine($"Nonce: {nonce}, Time taken: {timeTaken}, Hashrate: {hashRate:F2} {MeasureUnits[measureUnitIndex]}H/s");
    }

    private static async Task ShowStatisticsAsync(
        Func<long> getAttempts,
        Stopwatch stopwatch,
        CancellationToken cancellationToken)
    {
        short frameIndex = 0;
        TimeSpan el = TimeSpan.Zero;
        try
        {
            using var timer = new PeriodicTimer(TimeSpan.FromMilliseconds(300));
            while (await timer.WaitForNextTickAsync(cancellationToken))
            {
                Console.Clear();
                long attempts = getAttempts();
                var (hashRate, measureUnitIndex, timeTaken) = CalculateStatistics(attempts, stopwatch.Elapsed);

                Console.Write($"{LoaderFrames[frameIndex++ % LoaderFrames.Length]}");
                string textToRight = $"{hashRate:F2} {MeasureUnits[measureUnitIndex]}H/s | {attempts:N0} attempts | {timeTaken}\r";
                Console.SetCursorPosition(textToRight.Length > Console.WindowWidth ? 0 : Console.WindowWidth - textToRight.Length, Console.CursorTop);
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write(textToRight);
                Console.ResetColor();
            }
        }
        catch (OperationCanceledException)
        {
            Console.Clear();
            Console.WriteLine("\rMining completed.");
        }
    }
}
