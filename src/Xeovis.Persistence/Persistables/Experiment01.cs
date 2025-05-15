using Xeovis.Domain;

namespace Xeovis.Persistence.Proxies;

public class Experiment01
{
    public string Name { get; set; }
    public Guid Id { get; set; }
    public List<ExpRun01> Runs { get; set; }
    public string Path { get; set; }
    public List<string> FilesPath { get; set; }
    public List<PhaseType> PhasesWithData { get; set; }

    public Experiment01()
    {
        
    }
    
    public Experiment01(Experiment realObject)
    {
        Name = realObject.Name;
        Id = realObject.Id;
        Runs = realObject.Runs.Select(x => new ExpRun01(x)).ToList();
        Path = realObject.Path;
        FilesPath = new List<string>(realObject.FilesPath);
        PhasesWithData = new List<PhaseType>(realObject.PhasesWithData);
    }

    public Experiment ConvertToRealObject()
    {
        return new Experiment()
        {
            Name = Name,
            Id = Id,
            Runs = Runs.Select(x => x.ConvertToRealObject()).ToList(),
            Path = Path,
            FilesPath = [..FilesPath],
            PhasesWithData = [..PhasesWithData]
        };
    }
}