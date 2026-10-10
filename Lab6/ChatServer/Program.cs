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
        var nicks = new List<string>();
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

            string? nick = null;

            while (true)
            {
                string? input = await reader.ReadLineAsync();
                if (input == null) { client.Close();return; }

                input = input.Trim();

                if (string.IsNullOrWhiteSpace(input))
                {
                    await writer.WriteLineAsync("Ник не может быть пустым");
                    continue;
                }
                bool isTaken;
                lock (lockobj)
                {
                    isTaken = nicks.Contains(input);
                    if (!isTaken)
                    {
                        nicks.Add(input);
                        clients.Add(writer);
                    }
                }

                if (isTaken)
                {
                    await writer.WriteLineAsync($"Ник '{input}' уже занят.");
                    continue;
                }

                nick = input;
                await writer.WriteLineAsync($"Добро пожаловать, {nick}!");
                break;
            }

            Console.WriteLine($"[{DateTime.Now:HH:mm:ss}] {nick} подключился ({endpoint})");
            await BroadcastAsync($"*** {nick} вошёл в чат ***");

            try
            {
                string? line;
                while ((line = await reader.ReadLineAsync()) != null)
                {
                    if (line == "/exit") break;

                    if (line == "/list")
                    {
                        string users;
                        lock (lockobj) users = string.Join(", ", nicks);
                        await writer.WriteLineAsync($"Пользователи онлайн: {users}");
                        continue;
                    }

                    if (line.StartsWith("/w "))
                    {
                        var parts = line.Split(' ', 3);
                            if (parts.Length < 3)
                        {
                            await writer.WriteLineAsync("Формат: /W ник текст");
                            continue;
                        }

                        string target = parts[1];
                        string text = parts[2];

                        StreamWriter? targetWriter = null;
                        lock (lockobj)
                        {
                            int index = nicks.IndexOf(target);
                            if (index >= 0)
                                targetWriter = clients[index];
                        }
                        if (targetWriter == null)
                        {
                            await writer.WriteLineAsync($"Пользователь '{target}' не найден.");
                            continue;
                        }
                        try
                        {
                            await targetWriter.WriteLineAsync($"[ЛС от {nick}] {text}");
                        }
                        catch (IOException) { }
                        continue;
                    }
                    await BroadcastAsync($"[{DateTime.Now:HH:mm:ss}] {nick}: {line}");
                }
            }
            catch (IOException) { }
            finally
            {
                lock (lockobj) clients.Remove(writer);
                client.Close();
                Console.WriteLine($"[{DateTime.Now:HH:mm:ss}] {nick} отключился");
                await BroadcastAsync($"***{nick} покинул чат ***");
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