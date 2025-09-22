using Xeovis.Domain.Definitions;
using Xeovis.Domain.Definitions.Experiment;

namespace Xeovis.Domain
{
    public class Experiment
    {
        public string Name { get; set; }

        private string _shortName { get; set; }
        public string ShortName
        {
            get { return string.IsNullOrWhiteSpace(_shortName) ? GetShortName() : _shortName; }
            set { _shortName = value; }
        }

        public Guid Id { get; set; }
        public List<ExpRun> Runs { get; set; }
        public string Path { get; set; }
        public List<string> FilesPath { get; set; }
        public List<Guid> PhaseIdsWithData { get; set; }
        public List<PhaseNameSet> PhasesWithData => PhaseIdsWithData?.Select(x => PhaseNameList.GetNameSetById(x)).ToList();

        public Experiment()
        {
            Name = "No Name";
            ShortName = "No Name";
            Path = string.Empty;
            Id = Guid.NewGuid();
            Runs = new List<ExpRun>();
            FilesPath = new List<string>();
            PhaseIdsWithData = new List<Guid>();
        }

        public Experiment(string name, string path) : this()
        {
            Name = name;
            Path = path;
        }

        private string GetShortName()
        {
            if (string.IsNullOrWhiteSpace(Name)) return string.Empty;
            
            string shortName = new string(Name.Where(c => char.IsUpper(c)).ToArray());

            if (Name.Contains('_'))
            {
                var split = Name.Split('_');
                shortName += split[0].Substring(split[0].Length - 2) + "_" + split[1];
            }
            else
                shortName += Name.Substring(Name.Length - 2);

            return shortName;
        }

        public List<string> GetAllPhaseNames()
        {
            var phaseNames = Runs.SelectMany(r => r.Phases.Select(p => p.NameSet.KeyName));
            var distinctPhases = phaseNames.Distinct().ToList();
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

        public void ClearData(bool keepMainPath = true)
        {
            if (!keepMainPath) Path = string.Empty;
            Runs.Clear();
            FilesPath.Clear();
            PhasesWithData.Clear();
        }
    }
}
