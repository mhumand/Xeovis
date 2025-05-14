using Xeovis.Base.Validations;

namespace Xeovis.Base.Tests.Validations.Tests;

public class DuplicateNameValidationTests
{
    private List<string> exisitingNames;

    [SetUp]
    public void Setup()
    {
        exisitingNames = new List<string>()
        {
            "Name 1",
            "name"
        };
    }

    [TestCase("Name ", true, true)]
    [TestCase("Name ", false, true)]
    [TestCase("Name", false, true)]
    [TestCase("Name", true, false)]
    [TestCase("Name 1", false, false)]
    [TestCase("Name 1", true, false)]
    [TestCase("name 1", true, false)]
    [TestCase("name 1", false, true)]
    public void DuplicateValidation(string name, bool ignoreCase, bool expected)
    {
        var rule = new DuplicateNameRule(exisitingNames, ignoreCase);
        Assert.That(rule.Validate(name), Is.EqualTo(expected));
    }
}