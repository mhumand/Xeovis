using Xeovis.Domain;

namespace Xeovis.Persistence.Proxies;

public class Experiment01
{
    public string Name { get; set; }
    public string ShortName { get; set; }
    public Guid Id { get; set; }
    public List<ExpRun01> Runs { get; set; }
    public string Path { get; set; }
    public List<string> FilesPath { get; set; }
    public List<Guid> PhaseIdsWithData { get; set; }

    public Experiment01()
    {
        
    }
    
    public Experiment01(Experiment realObject)
    {
        Name = realObject.Name;
        ShortName = realObject.ShortName;
        Id = realObject.Id;
        Runs = realObject.Runs.Select(x => new ExpRun01(x)).ToList();
        Path = realObject.Path;
        FilesPath = new List<string>(realObject.FilesPath);
        PhaseIdsWithData = new List<Guid>(realObject.PhaseIdsWithData);
    }

    public Experiment ConvertToRealObject()
    {
        return new Experiment()
        {
            Name = Name,
            ShortName = ShortName,
            Id = Id,
            Runs = Runs.Select(x => x.ConvertToRealObject()).ToList(),
            Path = Path,
            FilesPath = [..FilesPath],
            PhaseIdsWithData = [..PhaseIdsWithData]
        };
    }
}