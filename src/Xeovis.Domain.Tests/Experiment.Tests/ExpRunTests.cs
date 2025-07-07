using Xeovis.Domain.Definitions;

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
                new Phase(PhaseNameList.GetNameSetIdByString("opx")),
                new Phase(PhaseNameList.GetNameSetIdByString("cpx")),
                new Phase(PhaseNameList.GetNameSetIdByString("g")),
            }
        };
        
        Assert.That(expRun.GetPhaseAssemblageString(), Is.EqualTo("opx+cpx+gt"));
    }
}