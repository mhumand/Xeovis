namespace Xeovis.Domain.Tests.MCTC.Tests;

public class BatchTests
{
    private Batch batch;

    [SetUp]
    public void Setup()
    {
        batch = new Batch();
        batch.Index = 2;
        batch.ExperimentName = "TestExperiment";
        batch.ExpRunName = "ExpRunName";
        //batch.Phases = new List<PhaseType>()
        //{
        //    PhaseType.phe,
        //    PhaseType.amph,
        //    PhaseType.q,
        //    PhaseType.g
        //};

        batch.IsFailed = new Dictionary<int, bool>()
        {
            { 1, false },
            { 2, false },
            { 3, false },
            { 4, false },
            { 5, true },
        };
    }

    [Test]
    public void BasicBatchTest()
    {
        Assert.That(batch.Name, Is.EqualTo("2 - TestExperiment - ExpRunName"));
        Assert.That(batch.GetPhaseAssemblageString("-"), Is.EqualTo("phe-amph-q-g"));
        Assert.That(batch.GetPhaseAssemblageString(), Is.EqualTo("phe amph q g"));
        Assert.That(batch.IsAllFailed, Is.False);
    }
}