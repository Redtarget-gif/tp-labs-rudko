using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;
internal class Program
{
    private static async Task Main(string[] args)
    {
        const int Port = 5555;

        var clients = new List<StreamWriter>();
        var lockobj = new object();

        var listener = new TcpListener(IPAddress.Any, Port);
        listener.Start();
        Console.WriteLine($"Сервер запущен на порту {Port}. Ctrl+C - остановка.");

        while (true)
        {
            TcpClient client = await listener.AcceptTcpClientAsync();
            _ = HandleClientAsync(client);
        }

        async Task HandleClientAsync(TcpClient client)
        {
            var endpoint = client.Client.RemoteEndPoint;
            var stream = client.GetStream();
            var reader = new StreamReader(stream);
            var writer = new StreamWriter(stream) { AutoFlush = true };

            string? nick = await reader.ReadLineAsync();
            if (string.IsNullOrWhiteSpace(nick)) { client.Close(); return; }

            lock (lockobj) clients.Add(writer);
            Console.WriteLine($"[{DateTime.Now:HH:mm:ss}] {nick} подключился ({endpoint})");
            await BroadcastAsync($"*** {nick} вошёл в чат ***");

            try
            {
                string? line;
                while ((line = await reader.ReadLineAsync()) != null)
                {
                    if (line == "/exit") break;
                    await BroadcastAsync($"[{DateTime.Now:HH:mm:ss}] {nick}: {line}");
                }
            }
            catch (IOException) { }
            finally
            {
                lock (lockobj) clients.Remove(writer);
                client.Close();
                Console.WriteLine($"[{DateTime.Now:HH:mm:ss}] {nick} отключился");
                await BroadcastAsync($"***{nick} покаинул чат ***");
            }
        }

        async Task BroadcastAsync(string message)
        {
            List<StreamWriter> snapshot;
            lock (lockobj) snapshot = clients.ToList();

            foreach (var w in snapshot)
            {
                try { await w.WriteLineAsync(message); }
                catch (IOException) { }
            }
        }
    }
}