using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Xeovis.Domain
{
    public class Experiment
    {
        public string Name { get; set; }
        public string ShortName { get; }
        public List<ExperimentalRun> Runs { get; set; }
        public DirectoryInfo DirectoryInfo { get; set; }
        public List<FileInfo> Files { get; set; }
        public List<PhaseType> PhasesWithData { get; set; }

        public Experiment(string name)
        {
            Name = name;
            ShortName = GetShortName();
            Runs = new List<ExperimentalRun>();
            DirectoryInfo = new DirectoryInfo(name);
            Files = new List<FileInfo>();
            PhasesWithData = new List<PhaseType>();
        }

        private string GetShortName()
        {
            string shortName = new string(Name.Where(c => char.IsUpper(c)).ToArray());
            shortName += Name.Substring(Name.Length - 2);
            return shortName;
        }

        public List<PhaseType> GetAllPhaseTypes()
        {
            var phaseTypes = Runs.SelectMany<ExperimentalRun, PhaseType>(r => r.Phases.Select(p => p.Type));
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
