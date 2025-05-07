using Xeovis.Domain;

namespace Xeovis.Persistence.Old
{
    public class ProjectFile_Old
    {
        public string? Name { get; set; }
        public string? Notes { get; set; }
        public Guid Id { get; set; }
        public List<Batch> Batches { get; set; }
        public List<xeosVariable> AllxeosVariables { get; set; }
        public Dictionary<xeosVariable, List<xeosVariable>> SelectedxeosVariables;
        public mctcPlotSettings_Old mctcPlotSettings;
        public List<mctcVariable> mctcVariables;
        public int MctcRunIndex;
        public ProjectSettings_Old ProjectSettings { get; set; }

        public ProjectFile_Old()
        {
            Id = Guid.NewGuid();
            Batches = new List<Batch>();
            AllxeosVariables = new List<xeosVariable>();
            SelectedxeosVariables = new Dictionary<xeosVariable, List<xeosVariable>>();
            mctcPlotSettings = new mctcPlotSettings_Old();
            mctcVariables = new List<mctcVariable>();
            ProjectSettings = new ProjectSettings_Old();
        }
    }
}
