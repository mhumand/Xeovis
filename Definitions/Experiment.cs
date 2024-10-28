using DatabaseReader.Enums;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace DatabaseReader.Definitions
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

        public override string ToString()
        {
            return Name;
        }
    }
}
