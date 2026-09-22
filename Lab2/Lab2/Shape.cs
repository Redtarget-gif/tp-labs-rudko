using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

public abstract class Shape
{
    public abstract double Area();
    public abstract double Perimeter();

    public override string ToString() => $"{GetType().Name}:S={Area():F2}, P={Perimeter():F2}";

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

namespace Lab2_shape
{
  
}
