using System.Diagnostics;
using System.Collections.Concurrent;
using System.Linq;
internal class Program
{
    private static void Main(string[] args)
    {
        const int N = 100_000_000;

        Console.WriteLine($"Генерация массива {N:N0} элементов");
        var random = new Random(42);
        var array = new int[N];
        for (int i = 0; i < N; i++)
            array[i] = random.Next(1, 1_000_000);
        Console.WriteLine($"Массив сгенерирован. Ядер:{Environment.ProcessorCount}\n");

        Console.Write("Введите кол-во потоков: ");
        if (!int.TryParse(Console.ReadLine(), out int threadCount) || threadCount <= 0)
        {
            Console.WriteLine("Ошибка: введите положительное число.");
            return;
        }

        Console.Write("Введите степень параллелизма для PLINQ: ");
        if (!int.TryParse(Console.ReadLine(), out int degree) || degree <= 0)
        {
            Console.WriteLine("Ошибка: введите положительное число.");
            return;
        }

        //Последовательно

        var sw = Stopwatch.StartNew();

        long seqSum = 0;
        int seqMin = int.MaxValue;
        int seqMax = int.MinValue;

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

        //Вручную по потокам

        int chunkSize = N / threadCount;

        var threads = new Thread[threadCount];
        var sums = new long[threadCount];
        var mins = new int[threadCount];
        var maxs = new int[threadCount];

        sw.Restart();

        for (int t = 0; t < threadCount; t++)
        {
            int threadIndex = t;
            int start = threadIndex * chunkSize;
            int end = (threadIndex == threadCount - 1) ? N : start + chunkSize;

            threads[t] = new Thread(() =>
            {
                long localSum = 0;
                int localMin = int.MaxValue;
                int localMax = int.MinValue;

                for (int i = start; i < end; i++)
                {
                    int v = array[i];
                    localSum += v;
                    if (v < localMin) localMin = v;
                    if (v > localMax) localMax = v;

                }

                sums[threadIndex] = localSum;
                mins[threadIndex] = localMin;
                maxs[threadIndex] = localMax;
            });

            threads[t].Start();
        }

        foreach (var thread in threads)
            thread.Join();

        long parSum = sums.Sum();
        int parMin = mins.Min();
        int parMax = maxs.Max();
        double parAvg = (double)parSum / N;

        sw.Stop();

        Console.WriteLine($"Вручную по потокам ({threadCount} потока(-ов))");
        Console.WriteLine($"Сумма: {parSum:N0}");
        Console.WriteLine($"Минимум: {parMin:N0}");
        Console.WriteLine($"Максимум: {parMax:N0}");
        Console.WriteLine($"Среднее: {parAvg:F2}");
        Console.WriteLine($"Время: {sw.ElapsedMilliseconds} мс\n");

        long parTime = sw.ElapsedMilliseconds;

        //Через PLINQ

        sw.Restart();

        long plinqSum = array.AsParallel().WithDegreeOfParallelism(degree).Sum(x => (long)x);
        int plinqMin = array.AsParallel().WithDegreeOfParallelism(degree).Min();
        int plinqMax = array.AsParallel().WithDegreeOfParallelism(degree).Max();
        double plinqAvg = array.AsParallel().WithDegreeOfParallelism(degree).Average();

        sw.Stop();

        Console.WriteLine($"PLINQ (степень: {degree})");
        Console.WriteLine($"Сумма: {plinqSum:N0}");
        Console.WriteLine($"Минимум: {plinqMin:N0}");
        Console.WriteLine($"Максимум: {plinqMax:N0}");
        Console.WriteLine($"Среднее: {plinqAvg:F2}");
        Console.WriteLine($"Время: {sw.ElapsedMilliseconds} мс\n");

        long plinqTime = sw.ElapsedMilliseconds;

        Console.WriteLine("Таблица результатов");
        Console.WriteLine($"{"Способ",-25} {"Время (мс)",-15} {"Ускорение",-10}");
        Console.WriteLine($"{"Последовательно",-25} {seqTime,-15} {"1.00x",-10}");
        Console.WriteLine($"{"По потокам",-25} {parTime,-15} {(double)seqTime / parTime:F2}x");
        Console.WriteLine($"{"PLINQ",-25} {plinqTime,-15} {(double)seqTime / plinqTime:F2}x");

        Console.WriteLine("Проверка совпадения результатов");
        Console.WriteLine($"Смма: {(seqSum==parSum && seqSum==plinqSum? "ДА": "НЕТ")}");
        Console.WriteLine($"Смма: {(seqMin == parMin && seqMin == plinqMin ? "ДА" : "НЕТ")}");
        Console.WriteLine($"Смма: {(seqMax == parMax && seqMax == plinqMax ? "ДА" : "НЕТ")}");
    }
}