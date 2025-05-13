using Xeovis.Base.Validations;

namespace Xeovis.Base.Tests.Validations.Tests;

public class NumericValidationTests
{
    [Test]
    public void BasicNumericValidation()
    {
        var rule = new NumericRule();
        Assert.That(rule.Validate("Name"), Is.False);
        Assert.That(rule.Validate("752"), Is.True);
        Assert.That(rule.Validate(2e6), Is.True);
    }

    [Test]
    public void BoundedNumericValidation()
    {
        var rule = new NumericRule(-6, 2020);
        Assert.That(rule.Validate("Name"), Is.False);
        Assert.That(rule.Validate("0"), Is.True);
        Assert.That(rule.Validate("405.5"), Is.True);
        Assert.That(rule.Validate(405.5), Is.True);
        rule.SetMinMaxLimits(500);
        Assert.That(rule.Validate(405.5), Is.False);
        Assert.That(string.IsNullOrEmpty(rule.ErrorMessage), Is.False);
        Assert.That(rule.Validate(5405.5), Is.True);
        Assert.That(string.IsNullOrEmpty(rule.ErrorMessage), Is.True);
    }
}