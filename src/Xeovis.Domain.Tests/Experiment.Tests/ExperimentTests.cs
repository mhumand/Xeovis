namespace Xeovis.Domain.Tests.Experiment.Tests;

public class ExperimentTests
{
    Domain.Experiment experiment;

    [SetUp]
    public void Setup()
    {
        experiment = new Domain.Experiment();
        experiment.Name = "TestName2009";
        
        experiment.Runs.Add(new ExpRun()
        {
            Name = "Run1",
            Temperature = 700,
            Pressure = 15,
            Rocktype = RockType.Metabasite,
            Phases = new List<Phase>()
            {
                new Phase()
                {
                    Name = "opx",
                    Type = PhaseType.opx,
                },
                new Phase()
                {
                    Name = "cpx",
                    Type = PhaseType.cpx,
                },
                new Phase()
                {
                    Name = "g",
                    Type = PhaseType.g,
                }
            }
        });
        
        experiment.Runs.Add(new ExpRun()
        {
            Name = "Run2",
            Temperature = 800,
            Pressure = 12,
            Rocktype = RockType.Eclogite,
            Phases = new List<Phase>()
            {
                new Phase()
                {
                    Name = "cpx",
                    Type = PhaseType.cpx,
                },
                new Phase()
                {
                    Name = "q",
                    Type = PhaseType.q,
                },
                new Phase()
                {
                    Name = "pl",
                    Type = PhaseType.pl,
                }
            }
        });
    }
    
    [Test]
    public void GetPhaseTypes()
    {
        var allPhaseTypes = experiment.GetAllPhaseTypes();
        
        Assert.That(experiment.ShortName, Is.EqualTo("TN09"));
        Assert.That(allPhaseTypes[0], Is.EqualTo(PhaseType.cpx));
        Assert.That(allPhaseTypes.Contains(PhaseType.cpx), Is.True);
        Assert.That(allPhaseTypes.Contains(PhaseType.opx), Is.True);
        Assert.That(allPhaseTypes.Contains(PhaseType.g), Is.True);
        Assert.That(allPhaseTypes.Contains(PhaseType.q), Is.True);
        Assert.That(allPhaseTypes.Contains(PhaseType.pl), Is.True);
    }

    [Test]
    public void GetRockTypes()
    {
        var rockTypes = experiment.GetAllRockTypes();
        
        Assert.That(rockTypes.Contains(RockType.Metabasite), Is.True);
        Assert.That(rockTypes.Contains(RockType.Eclogite), Is.True);
    }
}