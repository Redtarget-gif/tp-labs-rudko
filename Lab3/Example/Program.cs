using System;
using System.IO;
using System.Linq;

internal class Program
{
    private static void Main(string[] args)
    {
        Console.Write("Введите путь к папке: ");
        string folder = Console.ReadLine()!;

        if (!Directory.Exists(folder))
        {
            Console.WriteLine("Папка не существует.");
            return;
        }

        var files = Directory.EnumerateFiles(folder, "*", SearchOption.AllDirectories).Select(p=>new FileInfo(p)).ToList();

        Console.WriteLine($"Файлов: {files.Count}");
        Console.WriteLine($"Суммарный размер: {files.Sum(f=>f.Length)/ 1024.0/1024.0:F2} МБ");

        var top = files.GroupBy(f => f.Extension.ToLower()).OrderByDescending(g => g.Count()).Take(5);

        Console.WriteLine("Топ-5 расширений:");
        foreach (var g in top)
            Console.WriteLine($" {(g.Key == "" ?  "(без расширения))}" :  g.Key),-15} {g.Count()} шт.");
    }
}