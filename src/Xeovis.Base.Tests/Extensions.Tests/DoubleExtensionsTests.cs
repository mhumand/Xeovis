using NUnit.Framework.Constraints;

namespace Xeovis.Base.Tests.Extensions.Tests;

public class DoubleExtensionsTests
{
    [TestCase(5.6363259, 8, 5.63632590)]
    [TestCase(5.6363259, 7, 5.6363259)]
    [TestCase(5.6363259, 6, 5.636326)]
    [TestCase(5.6363259, 5, 5.63633)]
    [TestCase(5.6363259, 4, 5.6363)]
    [TestCase(5.6363259, 3, 5.636)]
    [TestCase(5.6363259, 2, 5.64)]
    [TestCase(5.6363259, 1, 5.6)]
    [TestCase(5.6363259, 0, 6.0)]
    [TestCase(5.6363259, -1, 5.6363259)]
    public void Round(double value, int digits, double expected)
    {
        Assert.That(value.Round(digits), Is.EqualTo(expected));
    }

    [TestCase("nonsense", double.NaN)]
    [TestCase("", double.NaN)]
    [TestCase(" ", double.NaN)]
    [TestCase("2e6", 2e6)]
    [TestCase("2e6", 2000000)]
    [TestCase("2e6", 2000000.0)]
    [TestCase("2e-6", 2e-6)]
    [TestCase("1/2", double.NaN)]
    [TestCase("1.9602", 1.9602)]
    [TestCase("1.9602 ", 1.9602)]
    [TestCase(" 1.9602", 1.9602)]
    [TestCase(" -1.9602", -1.9602)]
    public void GetDoubleValueFromText(string text, double expected)
    {
        Assert.That(DoubleExtensions.GetDoubleValueFromText(text), Is.EqualTo(expected));
    }
}