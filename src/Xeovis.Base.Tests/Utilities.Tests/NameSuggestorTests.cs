using Xeovis.Base.Utilities;

namespace Xeovis.Base.Tests.Utilities.Tests;

public class NameSuggestorTests
{
    private List<string> existingNames;

    [SetUp]
    public void Setup()
    {
        existingNames = new List<string>()
        {
            "Name 1",
            "Name 2",
            "Name 3",
            "Name 4",
            "Name 5",
            "Name 6",
            "Name 7",
            "test 1"
        };
    }

    [TestCase("test", "-", "test-1")]
    [TestCase("test", " ", "test 2")]
    [TestCase("Name", " ", "Name 8")]
    [TestCase("Name", "-", "Name-1")]
    public void SuggestName_Numeric(string core, string spacer, string expected)
    {
        Assert.That(NameSuggestor.SuggestNewName_Numeric(existingNames, core, spacer), Is.EqualTo(expected));
    }
}