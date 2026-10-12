using PhoneBook.Core;
internal class Program
{
    private static void Main(string[] args)
    {
        var storage = new FileContactStorage("contacts.csv");
        var phoneBook=new PhoneBookService(storage);
        phoneBook.Load();

        while (true)
        {
            Console.WriteLine("Телефонная книга");
            Console.WriteLine("1-добавить 2-удалить 3-поиск 4-сортировка по имени 5-сортировка по телефону 6-все контакты 7-сохранить 0-выход");
            Console.Write("Выбор: ");
            string? choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    Console.Write("Имя: ");
                    string name =Console.ReadLine()?? "";
                    Console.Write("Телефон: ");
                    string phone = Console.ReadLine() ?? "";
                    try { phoneBook.Add(new Contact { Name = name, Phone = phone }); }
                    catch(ArgumentException ex) { Console.WriteLine($"Ошибка: {ex.Message}"); }
                    break;

                case "2":
                    Console.Write("Имя для удаления: ");
                    phoneBook.Remove(Console.ReadLine() ?? "");
                    break;

                case "3":
                    Console.Write("Поиск (имя/номер): ");
                    var results = phoneBook.Search(Console.ReadLine()?? "");
                    foreach (var c in results) Console.WriteLine(c);
                    break;

                case "4":
                    foreach (var c in phoneBook.SortByName()) Console.WriteLine(c);
                    break;

                case "5":
                    foreach (var c in phoneBook.SortByPhone()) Console.WriteLine(c);
                    break;

                case "6":
                    foreach (var c in phoneBook.Contacts) Console.WriteLine(c);
                    break;

                case "7":
                    phoneBook.Save();
                    Console.WriteLine("Сохранено в contacts.csv");
                    break;

                case "0":
                    return;

                default:
                    Console.WriteLine("Неверный выбор");
                    break;

            }
        }
    }
}