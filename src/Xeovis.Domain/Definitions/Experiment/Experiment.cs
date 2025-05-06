namespace Xeovis.Domain
{
    public class Experiment
    {
        public string Name { get; set; }
        public string ShortName { get; }
        public Guid Id { get; set; }
        public List<ExpRun> Runs { get; set; }
        public string Path { get; set; }
        public List<string> FilesPath { get; set; }
        public List<PhaseType> PhasesWithData { get; set; }

        public Experiment()
        {
            Name = "string.Empty";
            ShortName = string.Empty;
            Path = string.Empty;
            Id = Guid.NewGuid();
            Runs = new List<ExpRun>();
            FilesPath = new List<string>();
            PhasesWithData = new List<PhaseType>();
        }

        public Experiment(string name, string path) : this()
        {
            Name = name;
            ShortName = GetShortName();
            Path = path;
        }

        private string GetShortName()
        {
            string shortName = new string(Name.Where(c => char.IsUpper(c)).ToArray());
            shortName += Name.Substring(Name.Length - 2);
            return shortName;
        }

        public List<PhaseType> GetAllPhaseTypes()
        {
            var phaseTypes = Runs.SelectMany(r => r.Phases.Select(p => p.Type));
            var distinctPhases = phaseTypes.Distinct().ToList();
            distinctPhases.Sort();
            return distinctPhases;
        }

        public List<RockType> GetAllRockTypes()
        {
            var rockTypes = Runs.Select(r => r.Rocktype);
            var distinctRockTypes = rockTypes.Distinct().ToList();
            distinctRockTypes.Sort();
            return distinctRockTypes;
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
