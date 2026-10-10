using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;
internal class Program
{
    private static async Task Main(string[] args)
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;
        Console.InputEncoding = System.Text.Encoding.UTF8;
        Console.Write("Адрес сервера (Enter = localhost): ");
        string host = Console.ReadLine() is { Length: > 0 } h ? h : "localhost";

        using var client = new TcpClient();
        await client.ConnectAsync(host, 5555);
        var stream=client.GetStream();
        var reader = new StreamReader(stream, System.Text.Encoding.UTF8);
        var writer = new StreamWriter(stream, System.Text.Encoding.UTF8) { AutoFlush = true };

        string? nick = null;
        while (nick == null)
        {
            Console.Write("Ваш ник: ");
            string? input = Console.ReadLine();
            if (input == null) return;

            await writer.WriteLineAsync(input);

            string? responce = await reader.ReadLineAsync();
            if (responce == null) return;

            Console.WriteLine(responce);

            if (responce.StartsWith("Добро пожаловать"))
                nick = input;
        }

        _ = Task.Run(async () =>
        {
            try
            {
                string? line;
                while ((line = await reader.ReadLineAsync()) != null) Console.WriteLine(line);
            }
            catch (IOException)
            {
                Console.WriteLine("Соединение с сервером потеряно");
            }
        });
        Console.WriteLine("Подключено. Пишите сообщения, /exit - выход.");
        while (true)
        {
            string? msg = Console.ReadLine();
            if (msg == null) continue;
            await writer.WriteLineAsync(msg);
            if (msg == "/exit") break;
        }
    }
}