using System;
using System.Collections.Generic;
using System.Text;

namespace Lab4.Core
{
    public static class MyAlgorithms
    {
        public static long Factorial(int n)
        {
            if (n<0)
                throw new ArgumentOutOfRangeException(nameof(n), "n не может быть отрицательным");
            if (n > 20)
                throw new ArgumentOutOfRangeException(nameof(n), "n > 20 вызывает переполнение long");

            long result = 1;
            for (int i = 2; i <= n; i++)
                result *= i;
            return result;
        }

        public static IReadOnlyList<int> Fibonacci(int n)
        {
            if (n < 0) throw new ArgumentOutOfRangeException(nameof(n), "n не может быть отрицательным");

            var result =new List<int>();
            int a = 0, b = 1;
            while (a <= n)
            {
                result.Add(a);
                int next = a + b;
                a = b;
                b = next;
            }

            return result;
        }

        public static double VariantFunction(double x)
        {
            if (x == 0)
                throw new ArgumentOutOfRangeException(nameof(x), "Деление на ноль");
            if (x < 1)
                throw new ArgumentOutOfRangeException(nameof(x), "Функция не определена при x < 1");
            return Math.Sin(5 / x) * Math.Cosh(Math.Sqrt(x - 1)) + Math.Exp(5 * x);
        }

        public static double SinTaylor(double x, double eps = 1e-6)
        {
            if (eps <= 0)
                throw new ArgumentOutOfRangeException(nameof(eps), "Точность должна быть > 0");

            double sum = 0;
            double term = x;
            int k = 0;

            while (Math.Abs(term) > eps)
            {
                sum += term;
                k++;
                term = -term * x * x / ((2 * k ) * (2 * k+1));
            }
            return sum;
        }
    }
}
