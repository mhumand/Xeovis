using System.Drawing;
using Xeovis.Domain;

namespace Xeovis.Persistence.Proxies;

public class mctcBundle01
{
    public string Name { get; set; }
    public Guid Id { get; set; }
    public DateTime TimeLoaded { get; set; }
    public string FilesDirectory { get; set; }
    public List<Batch01> Batches { get; set; }
    public List<mctcVariable01> mctcVariables { get; set; }
    public Color Color { get; set; }

    public mctcBundle01()
    {
        
    }
    
    public mctcBundle01(mctcBundle realObject)
    {
        Name = realObject.Name;
        Id = realObject.Id;
        TimeLoaded = realObject.TimeLoaded;
        FilesDirectory = realObject.FilesDirectory;
        Batches = realObject.Batches.Select(x => new Batch01(x)).ToList();
        mctcVariables = realObject.mctcVariables.Select(x => new mctcVariable01(x)).ToList();
        Color = realObject.Color;
    }

    public mctcBundle ConvertToRealObject()
    {
        return new mctcBundle()
        {
            Name = Name,
            Id = Id,
            TimeLoaded = TimeLoaded,
            FilesDirectory = FilesDirectory,
            Batches = Batches.Select(x => x.ConvertToRealObject()).ToList(),
            mctcVariables = mctcVariables.Select(x => x.ConvertToRealObject()).ToList(),
            Color = Color
        };
    }
}