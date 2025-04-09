namespace Xeovis.Domain.Definitions.OnHold
{
    public class SubsetRun
    {
        public ExpRun Run { get; set; }
        public string Name => Run.Name;
        public List<Phase> Phases { get; set; }
        public double Pressure => Run.Pressure;
        public double Temperature => Run.Temperature;
        public Experiment Experiment => Run.Experiment;
        public int Index { get; set; }

        public SubsetRun(List<Phase> phases, ExpRun expRun)
        {
            Phases = phases;
            Run = expRun;
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
