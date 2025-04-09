using Xeovis.Domain;

namespace Xeovis.Persistence
{
    public class DataFile
    {
        public string Name { get; set; }
        public string Notes { get; set; }
        public List<Batch> Batches { get; set; }
        public List<xeosVariable> AllxeosVariables { get; set; }
        public Dictionary<xeosVariable, List<xeosVariable>> SelectedxeosVariables;
        public mctcPlotSettings mctcPlotSettings;
        public List<mctcVariable> mctcVariables;
        public int MctcRunIndex;
        public ProjectSettings ProjectSettings { get; set; }

        public DataFile()
        {
            Batches = new List<Batch>();
            AllxeosVariables = new List<xeosVariable>();
            SelectedxeosVariables = new Dictionary<xeosVariable, List<xeosVariable>>();
            mctcPlotSettings = new mctcPlotSettings();
            mctcVariables = new List<mctcVariable>();
            ProjectSettings = new ProjectSettings();
        }
    }
}
