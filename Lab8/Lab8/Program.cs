using System.Diagnostics;
using System.Text;

const int N = 30_000;

var swTotal = Stopwatch.StartNew();

// --- 1. Генерация данных ---
var rnd = new Random(42);
var buyers = new List<string>();
var amounts = new List<int>();
for (int i = 0; i < N; i++)
{
    buyers.Add("user" + rnd.Next(0, 5000));
    amounts.Add(rnd.Next(1, 10_000));
}

// --- 2. Список уникальных покупателей ---
var sw = Stopwatch.StartNew();
var uniqueSet = new HashSet<string>(buyers);
var unique = new List<string>(uniqueSet);
Console.WriteLine($"Уникальных покупателей: {unique.Count}  ({sw.ElapsedMilliseconds} мс)");

// --- 3. Текстовый отчёт ---
sw.Restart();
var sb = new StringBuilder();
for (int i = 0; i < buyers.Count; i++)
{
    sb.Append(buyers[i]).Append(';').Append(amounts[i]).AppendLine();
}
string report=sb.ToString();
Console.WriteLine($"Отчёт: {report.Length} символов  ({sw.ElapsedMilliseconds} мс)");

// --- 4. Сортировка сумм ---
sw.Restart();
var sorted = new List<int>(amounts);
sorted.Sort();
Console.WriteLine($"Медианная сумма: {sorted[sorted.Count / 2]}  ({sw.ElapsedMilliseconds} мс)");

// --- 5. Сумма покупок каждого покупателя ---
sw.Restart();
var totalDict = new Dictionary<string,long>();
for(int i=0; i<buyers.Count; i++)
{
    if (totalDict.TryGetValue(buyers[i], out long current))
        totalDict[buyers[i]] = current + amounts[i];
    else
        totalDict[buyers[i]] = amounts[i];
}
var leader = totalDict.MaxBy(kv => kv.Value);
Console.WriteLine($"Максимум потратил: {leader.Key}  ({sw.ElapsedMilliseconds} мс)");

Console.WriteLine($"ИТОГО: {swTotal.ElapsedMilliseconds} мс");