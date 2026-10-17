using System.Diagnostics;
internal class Program
{
    private static void Main(string[] args)
    {
        const int N = 3_000_000;

        var sw = Stopwatch.StartNew();

        int seq = 0;

        for (int i = 2; i < N; i++)
            if (IsPrime(i)) seq++;
        sw.Stop();
        Console.WriteLine($"последовательно: {seq} простых, {sw.ElapsedMilliseconds} мс");

        sw.Restart();
        int par = 0;
        Parallel.For(2, N, i =>
        {
            if (IsPrime(i)) Interlocked.Increment(ref par);
        });
        sw.Stop();
        Console.WriteLine($"Паралельно: {par} простых, {sw.ElapsedMilliseconds} мс");
        Console.WriteLine($"Ядер доступно: {Environment.ProcessorCount}");

        static bool IsPrime(int n)
        {
            if (n < 2) return false;
            for (int d = 2; d * d <= n; d++)
                if (n % d == 0) return false;
            return true;
        }
    }
}