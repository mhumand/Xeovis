using DatabaseReader.Enums;
using System.Collections.Generic;
using System.IO;

namespace DatabaseReader.Definitions
{
    public class Experiment
    {
        public string Name { get; set; }
        public List<ExperimentalRun> Runs { get; set; }
        public DirectoryInfo DirectoryInfo { get; set; }
        public List<FileInfo> Files { get; set; }
        public List<PhaseType> PhasesWithData { get; set; }
        public Experiment(string name)
        {
            Name = name;
            Runs = new List<ExperimentalRun>();
            DirectoryInfo = new DirectoryInfo(name);
            Files = new List<FileInfo>();
            PhasesWithData = new List<PhaseType>();
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
