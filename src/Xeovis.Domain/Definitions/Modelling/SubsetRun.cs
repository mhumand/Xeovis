using System.Collections.Generic;

namespace Xeovis.Domain
{
    public class SubsetRun
    {
        public ExperimentalRun Run { get; set; }
        public string Name => Run.Name;
        public List<Phase> Phases { get; set; }
        public double Pressure => Run.Pressure;
        public double Temperature => Run.Temperature;
        public Experiment Experiment => Run.Experiment;
        public int Index { get; set; }

        public SubsetRun(List<Phase> phases, ExperimentalRun experimentalRun)
        {
            Phases = phases;
            Run = experimentalRun;
        }

        public override string ToString()
        {
            string phaseNames = "";
            foreach (var phase in Phases)
                phaseNames += phase.Name + "+";

            return $"{Experiment.Name} - {Name} - ({phaseNames.TrimEnd('+')})";
        }
    }
}
