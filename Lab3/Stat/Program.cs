using System;
using System.IO;
using System.Linq;

public static class TextStat
{
    public static void Run(string path)
    {
        if (!File.Exists(path))
        {
            Console.WriteLine("Ошибка: файл не найден!");
            return;
        }

        try
        {
            string[] lines = File.ReadAllLines(path);
            string text = string.Join(" ", lines);

            int totalChars = text.Length;
            int charsWithoutSpaces = text.Count(c => !char.IsWhiteSpace(c));

            string[] words = text.Split(new char[] { ' ', '\t', '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);
            string longestWord = words.OrderByDescending(w => w.Length).FirstOrDefault() ?? "";

            Console.WriteLine($"\n Статистика по файлу");
            Console.WriteLine($"Путь расположения файла: {path}");
            Console.WriteLine($"Количество строк: {lines.Length}");
            Console.WriteLine($"Количество слов: {words.Length}");
            Console.WriteLine($"Количество символов с пробелами: {totalChars}");
            Console.WriteLine($"Количество символов без пробелов: {charsWithoutSpaces}");
            Console.WriteLine($"Самое длинное слово: \"{longestWord}\" ({longestWord.Length} символов)");
        }
        catch (IOException ex)
        {
            Console.WriteLine($"Ошибка ввода-вывода: {ex.Message}");
        }

        catch (UnauthorizedAccessException)
        {
            Console.WriteLine("Ошибка: нет прав доступа к файлу!");
        }

    }
}
    class Program
    {
        static void Main()
        {
        Console.OutputEncoding = System.Text.Encoding.UTF8;

        Console.Write("Введите путь к текстовому файлу: ");
        string path = Console.ReadLine()!;

        TextStat.Run(path);
        }
    }
