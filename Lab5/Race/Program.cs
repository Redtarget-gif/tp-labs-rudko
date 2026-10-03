using System.Diagnostics;
internal class Program
{
    private static void Main(string[] args)
    {
        int counter = 0;
        var threads = Enumerable.Range(0, 4).Select(_ => new Thread(() =>
        {
            for (int i = 0; i < 100_000; i++)
                counter++;
        })).ToList();

        threads.ForEach(t => t.Start());
        threads.ForEach(t => t.Join());

        Console.WriteLine($"Ожидаем 400000, получили: {counter}");
    }
}