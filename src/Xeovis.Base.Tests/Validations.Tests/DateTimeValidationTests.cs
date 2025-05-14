using Xeovis.Base.Validations;

namespace Xeovis.Base.Tests.Validations.Tests;

public class DateTimeValidationTests
{
    [Test]
    public void BasicDateTimeValidation()
    {
        var rule = new DateTimeRule();
        Assert.That(rule.Validate(new DateTime()), Is.False);
        Assert.That(rule.Validate(new DateTime(1899, 12, 31)), Is.False);
        Assert.That(rule.Validate(new DateTime(1900, 01, 01)), Is.True);
        Assert.That(rule.Validate(new DateTime(2025, 05, 13)), Is.True);
        Assert.That(rule.Validate("22/09/2020"), Is.True);
    }

    [Test]
    public void BoundedDateTimeValidation()
    {
        var upperBound = new DateTime(2030, 1, 1);
        var lowerBound = new DateTime(2000, 1, 1);
        var rule = new DateTimeRule(lowerBound, upperBound);
        Assert.That(rule.Validate(new DateTime(2030, 1, 1)), Is.True);
        Assert.That(rule.Validate(new DateTime(2000, 1, 1)), Is.True);
        Assert.That(rule.Validate(new DateTime(2025, 05, 13)), Is.True);
        Assert.That(rule.Validate(new DateTime(1999, 04, 13)), Is.False);
        Assert.That(string.IsNullOrEmpty(rule.ErrorMessage), Is.False);
        Assert.That(rule.Validate("22/09/2020"), Is.True);
        Assert.That(rule.Validate("22/09/2020 23:05:22"), Is.True);
        Assert.That(string.IsNullOrEmpty(rule.ErrorMessage), Is.True);
    }
}