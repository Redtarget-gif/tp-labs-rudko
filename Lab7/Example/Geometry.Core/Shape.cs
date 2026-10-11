using System;
using System.Collections.Generic;
using System.Text;

namespace Geometry.Core;

public abstract class Shape
{
    public abstract double Area();
    public abstract double Perimeter();
    public override string ToString() => $"{GetType().Name}: S={Area():F2}, P={Perimeter():F2}";
}

public class Circle : Shape
{
    public double Radius { get; }

    public Circle(double radius)
    {
        if (radius <= 0) throw new ArgumentOutOfRangeException(nameof(radius));
        Radius = radius;
    }

    public override double Area() => Math.PI * Radius * Radius;
    public override double Perimeter() => 2 * Math.PI * Radius;
}

public class Rectangle : Shape
{
    public double Width { get; }
    public double Height { get; }

    public Rectangle(double width, double height)
    {
        if (width <= 0 || height <= 0) throw new ArgumentOutOfRangeException();
        Width = width;
        Height = height;
    }

    public override double Area() => Width * Height;
    public override double Perimeter() => 2 * (Width + Height);
}

public class Square : Rectangle
{
    public Square(double side) : base(side, side) { }
}

public class Triangle : Shape
{
    public double A { get; }
    public double B { get; }
    public double C { get; }

    public Triangle(double a, double b, double c)
    {
        if (a <= 0 || b <= 0 || c <= 0) throw new ArgumentOutOfRangeException();
        if (a + b <= c || a + c <= b || b + c <= a) throw new ArgumentException("Треугольник не существует");
        A = a; B = b; C = c;
    }

    public override double Area()
    {
        double p = (A + B + C) / 2;
        return Math.Sqrt(p * (p - A) * (p - B) * (p - C));
    }

    public override double Perimeter() => A + B + C;
}
