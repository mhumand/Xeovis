using Xeovis.Domain;
using Xeovis.Presentation;

namespace Xeovis.Persistence
{
    public class ProjectFile
    {
        public string ProjectName { get; set; }
        public string Note { get; set; }
        public DateTime CreatedAt { get; set; }

        public List<Experiment> Experiments { get; set; }
        public List<mctcBundle> mctcBundles { get; set; }
        public List<mctcPlotDto> mctcPlotDtos { get; set; }

        public ProjectFile()
        {
            ProjectName = string.Empty;
            Note = string.Empty;
            CreatedAt = DateTime.MinValue;
            Experiments = new List<Experiment>();
            mctcBundles = new List<mctcBundle>();
            mctcPlotDtos = new List<mctcPlotDto>();
        }
    }
}
