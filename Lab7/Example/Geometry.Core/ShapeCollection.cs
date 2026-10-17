using System;
using System.Collections.Generic;
using System.Text;
namespace Geometry.Core
{
    public class ShapeCollection
    {
        private readonly List<Shape> _shapes = [];

        public IReadOnlyList<Shape> Shapes => _shapes;
        public void Add (Shape shape)=>_shapes.Add(shape);
        public double TotalArea() => _shapes.Sum(s => s.Area());
        public Shape? Largest()=>_shapes.MaxBy(s=>s.Area());
    }
}
