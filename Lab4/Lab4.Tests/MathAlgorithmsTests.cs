using System;
using System.Collections.Generic;
using System.Text;
using Lab4.Core;

namespace Lab4.Tests
{
    public class MathAlgorithmsTests
    {
        [Fact]
        public void Factorial_ofZero_ReturnsOne()
        {
            long result = MathAlgorithms.Factorial(0);
            Assert.Equal(1, result);
        }

        [Theory]
        [InlineData(1, 1)]
        [InlineData(5, 120)]
        [InlineData(10, 3628800)]
        [InlineData(20, 2432902008176640000)]

        public void Factorial_ReturnsExpected(int n, long expected) => Assert.Equal(expected, MathAlgorithms.Factorial(n));

        [Theory]
        [InlineData(-1)]
        [InlineData(21)]
        public void Factorial_OutOfRange_Throws(int n) => Assert.Throws<ArgumentOutOfRangeException>(() => MathAlgorithms.Factorial(n));

        [Fact]
        public void Fibonacci_First6_AreCorrect()
        {
            var seq = MathAlgorithms.Fibonacci(6);
            Assert.Equal(new long[] { 0, 1, 1, 2, 3, 5 }, seq);
        }

        [Fact]
        public void Fibonacci_ZeroCount_ReturnsEmpty() => Assert.Empty(MathAlgorithms.Fibonacci(0));

        [Theory]
        [InlineData(0)]
        [InlineData(0.5)]
        [InlineData(Math.PI / 2)]
        [InlineData(-1.2)]
        public void SinTaylor_MachinesMathSin(double x) => Assert.Equal(Math.Sin(x), MathAlgorithms.SinTaylor(x), 1e-5);
    }
}
