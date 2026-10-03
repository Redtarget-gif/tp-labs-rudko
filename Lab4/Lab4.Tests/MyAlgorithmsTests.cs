using System;
using System.Collections.Generic;
using System.Text;
using Lab4.Core;
using Xunit;

namespace Lab4.Tests
{
    public class MyAlgorithmsTests
    {
        [Fact]
        public void Factorial_OfZero_ReturnsOne()
        {
            long result = MyAlgorithms.Factorial(0);
            Assert.Equal(1, result);
        }

        [Theory]
        [InlineData(1,1)]
        [InlineData(5,120)]
        [InlineData(10,3628800)]
        [InlineData(20, 2432902008176640000)]

        public void Factorial_ReturnsExpected(int n, long expected)
        {
            long result=MyAlgorithms.Factorial(n);
            Assert.Equal(expected,result);
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(21)]
        public void Factorial_OutOfRange_Throws(int n)
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => MyAlgorithms.Factorial(n));
        }

        [Fact]
        public void Fibonacci_UpTo5_ReturnsCorrectSequence()
        {
            var result = MyAlgorithms.Fibonacci(5);
            Assert.Equal(new[] { 0, 1, 1, 2, 3, 5 }, result);
        }

        [Fact]
        public void Fibonacci_Negative_Throws()
        {
            Assert.Throws<ArgumentOutOfRangeException>(()=> MyAlgorithms.Fibonacci(-1));
        }

        [Fact]
        public void VariantFunction_AtX1_ReturnsExpected()
        {
            double expected=Math.Sin(5)*Math.Cosh(0)+Math.Exp(5);

            double result=MyAlgorithms.VariantFunction(1);

            Assert.Equal(expected, result, 1e-5);
        }

        [Theory]
        [InlineData(2)]
        [InlineData(5)]
        [InlineData(10)]
        public void VariantFunction_AtValidX_ReturnsFiniteValue(double x)
        {
            double result=MyAlgorithms.VariantFunction(x);
            Assert.True(double.IsFinite(result));
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        [InlineData(0.5)]
        public void VariantFunction_InvalidX_Throws(double x)
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => MyAlgorithms.VariantFunction(x));
        }

        [Theory]
        [InlineData(0)]
        [InlineData(0.5)]
        [InlineData(Math.PI/2)]
        [InlineData(-1.2)]
        public void SinTaylor_MatchesMathSin(double x)
        {
            double result=MyAlgorithms.SinTaylor(x);
            Assert.Equal(Math.Sin(x), result, 1e-5);
        }

        [Fact]
        public void SinTaylor_InvalidEps_Throws()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => MyAlgorithms.SinTaylor(1, 0));
        }
    }
}
