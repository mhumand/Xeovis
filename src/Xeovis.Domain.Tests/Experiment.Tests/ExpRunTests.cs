namespace Xeovis.Domain.Tests.Experiment.Tests;

public class ExpRunTests
{
    [Test]
    public void GetPhaseAssemblageString()
    {
        var expRun = new ExpRun()
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
        };
        
        Assert.That(expRun.GetPhaseAssemblageString(), Is.EqualTo("opx+cpx+g"));
    }
}