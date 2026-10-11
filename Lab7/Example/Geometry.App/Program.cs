using Geometry.Core;

internal class Program
{
    public static void Main(string[] args)
    {
        var collection = new ShapeCollection();

        while (true)
        {
            Console.WriteLine("\n1-круг 2-прямоугольник 3-итоги 0-выход");
            Console.Write("Выбор: ");
            string? choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    Console.Write("Радиус: ");
                    if (double.TryParse(Console.ReadLine(), out double r))
                        collection.Add(new Circle(r));
                    else
                        Console.WriteLine("Ошибка: введите число.");
                    break;

                case "2":
                    Console.Write("Ширина и высота через пробел: ");
                    var p = Console.ReadLine()!.Split();
                    if (p.Length == 2 &&
                        double.TryParse(p[0], out double w) &&
                        double.TryParse(p[1], out double h))
                        collection.Add(new Rectangle(w, h));
                    else
                        Console.WriteLine("Ошибка: введите два числа через пробел.");
                    break;

                case "3":
                    Console.WriteLine($"Фигур: {collection.Shapes.Count}, S общая: {collection.TotalArea():F2}");
                    Console.WriteLine($"Крупнейшая: {collection.Largest()}");
                    break;

                case "0":
                    return;

                default:
                    Console.WriteLine("Неверный выбор.");
                    break;
            }
        }
    }
}