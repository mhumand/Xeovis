using System.Collections.Generic;

namespace DatabaseReader.Definitions.Persistence
{
    public class DataFile
    {
        public string Name { get; set; }
        public string Notes { get; set; }
        public List<Batch> Batches { get; set; }
        public List<ModelVariable> AllModelVariables { get; set; }
        public Dictionary<ModelVariable, List<ModelVariable>> SelectedModelVariables;
        public MctcPlotSettings MctcPlotSettings;
        public List<MctcVariable> MctcVariables;
        public int MctcRunIndex;
        public ProjectSettings ProjectSettings { get; set; }

        public DataFile()
        {
            Batches = new List<Batch>();
            AllModelVariables = new List<ModelVariable>();
            SelectedModelVariables = new Dictionary<ModelVariable, List<ModelVariable>>();
            MctcPlotSettings = new MctcPlotSettings();
            MctcVariables = new List<MctcVariable>();
            ProjectSettings = new ProjectSettings();
        }
    }
}
