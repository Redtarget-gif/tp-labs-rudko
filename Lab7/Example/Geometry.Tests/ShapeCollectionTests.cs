using System;
using System.Collections.Generic;
using System.Text;
using Geometry.Core;
using Xunit;

public class ShapeCollectionTests
{
    [Fact]
    public void TotalArea_TwoShapes_IsSum()
    {
        var c = new ShapeCollection();
        c.Add(new Rectangle(2, 3));
        c.Add(new Square(4));
        Assert.Equal(22,c.TotalArea(),1e-9);
    }
    [Fact]
    public void Largest_TwoShapes_ReturnsBiggest()
    {
        var c = new ShapeCollection();
        c.Add(new Rectangle(2, 3));   
        c.Add(new Square(4));        
        var largest = c.Largest();
        Assert.NotNull(largest);
        Assert.Equal(16, largest!.Area(), 1e-9);
    }
}
    
