using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;
internal class Program
{
    private static async Task Main(string[] args)
    {
        Console.Write("Адрес сервера (Enter = localhost): ");
        string host = Console.ReadLine() is { Length: > 0 } h ? h : "localhost";
        Console.Write("Ваш ник: ");
        string nick = Console.ReadLine() ?? "anon";

        using var client = new TcpClient();
        await client.ConnectAsync(host, 5555);
        var stream=client.GetStream();
        var reader = new StreamReader(stream);
        var writer = new StreamWriter(stream) { AutoFlush = true };

        await writer.WriteLineAsync(nick);

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