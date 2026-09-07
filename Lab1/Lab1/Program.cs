using System.Runtime;

internal class Program
{
    private static void Main(string[] args)
    {
        while (true)
        {
            Console.WriteLine("Выберите задание: ");
            Console.WriteLine("1-Факториал от n 2-числа Фиббоначчи от 0 до n 3-функция 4-ряд Тейлора 0-выход");
            Console.Write("Ваш выбор: ");
            if (!int.TryParse(Console.ReadLine(), out int choice))
            {
                Console.WriteLine("Ошибка: введите число.");
                continue;
            }

            if (choice == 0)
            {
                Console.WriteLine("Завершение работы");
                break;
            }

            switch (choice)
            {
                case 1: Task1(); break;
                case 2: Task2(); break;
                case 3: Task3(); break;
                case 4: Task4(); break;
                default:
                    Console.WriteLine("Неверный выбор");
                    continue;
            }
        }
        static void Task1()
        {
            Console.Write("Введите n (0..20): ");

            if (!int.TryParse(Console.ReadLine(), out int n) || n < 0 || n > 20)
            {
                Console.WriteLine("Ошибка: нужно целое число от 0 до 20.");
                return;
            }
            Console.WriteLine($"{n}!={Factorial(n)}");

            static long Factorial(int n)
            {
                long result = 1;
                for (int i = 2; i <= n; i++)
                    result = result * i;
                return result;
            }
        }

        static void Task2() { }
        static void Task3() { }
        static void Task4() { }
    }
}