using System.Diagnostics;
using System.Collections.Concurrent;
internal class Program
{
    private static void Main(string[] args)
    {
        const int N = 100_000_000;

        Console.WriteLine($"Генерация массива {N:N0} элементов");
        var random = new Random(42);
        var array = new int[N];
        for (int i = 0;i<N; i++)
            array[i] = random.Next(1,1_000_000);
        Console.WriteLine($"Массив сгенерирован. Ядер:{Environment.ProcessorCount}\n");

        //Последовательно

        var sw = Stopwatch.StartNew();

        long seqSum = 0;
        int seqMin = int.MaxValue;
        int seqMax=int.MinValue;

        for (int i = 0; i < N; i++)
        {
            int v = array[i];
            seqSum += v;
            if (v < seqMin) seqMin = v;
            if (v > seqMax) seqMax = v;
        }


        double seqAvg = (double)seqSum / N;
        sw.Stop();

        Console.WriteLine("Последовательно");
        Console.WriteLine($"Сумма: {seqSum:N0}");
        Console.WriteLine($"Минимум: {seqMin:N0}");
        Console.WriteLine($"Максимум: {seqMax:N0}");
        Console.WriteLine($"Среднее: {seqAvg:F2}");
        Console.WriteLine($"Время: {sw.ElapsedMilliseconds} мс\n");

        long seqTime = sw.ElapsedMilliseconds;
    }
}