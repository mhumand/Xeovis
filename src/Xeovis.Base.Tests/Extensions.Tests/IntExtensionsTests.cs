namespace Xeovis.Base.Tests.Extensions.Tests;

public class IntExtensionsTests
{
    [TestCase(8, 6, 3, true)]
    [TestCase(8, 6, 2, true)]
    [TestCase(8, -6, 2, false)]
    [TestCase(-8, 6, 2, false)]
    [TestCase(8, 6, 1, false)]
    [TestCase(8, 6, -1, false)]
    [TestCase(6, 6, -1, false)]
    public void IsAlmostEqual(int a, int b, int threshold, bool expected)
    {
        Assert.That(a.IsAlmostEqual(b, threshold), Is.EqualTo(expected));
    }
    
    [TestCase(5, 8, 12, false, false)]
    [TestCase(5, 8, 12, true, false)]
    [TestCase(5, 18, 12, false, false)]
    [TestCase(5, 18, 12, true, false)]
    [TestCase(5, 1, 12, true, true)]
    [TestCase(5, 1, 12, false, true)]
    [TestCase(5, 1, 5, false, false)]
    [TestCase(5, 1, 5, true, true)]
    public void IsBetween(int value, int lowerBound, int upperBound, bool inclusive, bool expected)
    {
        Assert.That(value.IsBetween(lowerBound, upperBound, inclusive), Is.EqualTo(expected));
    }
}