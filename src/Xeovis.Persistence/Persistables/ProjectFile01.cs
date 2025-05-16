namespace Xeovis.Persistence.Proxies;

public class ProjectFile01
{
    public int Version { get; }
    public string ProjectName { get; set; }
    public string Note { get; set; }
    public DateTime CreatedAt { get; set; }
    public List<Experiment01> Experiments { get; set; }
    public List<mctcBundle01> mctcBundles { get; set; }
    public List<mctcPlotDto01> mctcPlotDtos { get; set; }

    public ProjectFile01()
    {
    }

    public ProjectFile01(ProjectFile realObject)
    {
        Version = 1;
        ProjectName = realObject.ProjectName;
        Note = realObject.Note;
        CreatedAt = realObject.CreatedAt;
        Experiments = realObject.Experiments.Select(x => new Experiment01(x)).ToList();
        mctcBundles = realObject.mctcBundles.Select(x => new mctcBundle01(x)).ToList();
        mctcPlotDtos = realObject.mctcPlotDtos.Select(x => new mctcPlotDto01(x)).ToList();
    }

    public ProjectFile ConvertToRealObject()
    {
        return new ProjectFile()
        {
            ProjectName = ProjectName,
            Note = Note,
            CreatedAt = CreatedAt,
            Experiments = Experiments.Select(x => x.ConvertToRealObject()).ToList(),
            mctcBundles = mctcBundles.Select(x => x.ConvertToRealObject()).ToList(),
            mctcPlotDtos = mctcPlotDtos.Select(x => x.ConvertToRealObject()).ToList()
        };
    }
}