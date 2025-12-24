using System.Diagnostics;

namespace GarbageExperiment;

class Program
{
    const int ObjectCount = 200_000;

    static void Main()
    {
        Console.WriteLine("=== Эксперимент: накладные расходы GC ===\n");
        RunExperiment();
        Console.ReadKey();
    }

    private static void RunExperiment()
    {
        HeavyObject.ResetStats();

        GC.Collect();
        GC.WaitForPendingFinalizers();
            
        var stopwatch = Stopwatch.StartNew();

        for (var i = 1; i <= ObjectCount; i++)
        {
            new HeavyObject(i);
            if (i % 50_000 == 0)
            {
                Console.WriteLine($"Создано объектов: {i:N0}");
            }
        }

        stopwatch.Stop();

        GC.Collect();
        GC.WaitForPendingFinalizers();

        PrintResults(stopwatch.Elapsed);
    }

    private static void PrintResults(TimeSpan elapsed)
    {
        Console.WriteLine();
        Console.WriteLine("Результаты:");
        Console.WriteLine("-----------------------------------------");

        Console.WriteLine($"Создано объектов: {ObjectCount:N0}");
        Console.WriteLine($"Время выполнения: {elapsed.TotalMilliseconds:F0} мс");
        Console.WriteLine($"Время на объект: {(elapsed.TotalMilliseconds / ObjectCount):F4} мс");
        Console.WriteLine();
        Console.WriteLine(
            $"Gen0: {GC.CollectionCount(0)}\n" +
            $"Gen1: {GC.CollectionCount(1)}\n" +
            $"Gen2: {GC.CollectionCount(2)}");
        if (!HeavyObject.GcStarted) return;
        Console.WriteLine();
        Console.WriteLine(
            $"GC начал финализацию примерно при объекте #{HeavyObject.FirstCollectedId}");
    }
}
