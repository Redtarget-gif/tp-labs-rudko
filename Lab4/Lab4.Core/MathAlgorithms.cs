using System;
using System.Collections.Generic;
using System.Text;

namespace Lab4.Core
{
    public static class MathAlgorithms
    {
        public static long Factorial(int n)
        {
            if (n < 0) throw new ArgumentOutOfRangeException(nameof(n));
            if (n > 20) throw new ArgumentOutOfRangeException(nameof(n), "переполнение long");
            long result = 1;
            for (int i = 2; i <= n; i++) result *= i;
            return result;
        }

        public static IReadOnlyList<long> Fibonacci(int count)
        {
            if (count < 0) throw new ArgumentOutOfRangeException(nameof(count));
            var result = new List<long>();
            long a = 0, b = 1;
            for (int i=0;i<count;i++)
            {
                result.Add(a);
                (a, b) = (b, a + b);
            }

            return result;
        }

        public static double SinTaylor(double x, double eps = 1e-6)
        {
            double term = x, sum = x;
            for (int n=1; Math.Abs(term) > eps; n++)
            {
                term *= -x * x / ((2 * n) * (2 * n + 1));
                sum += term;
            }
            return sum;
        }
    }
}
