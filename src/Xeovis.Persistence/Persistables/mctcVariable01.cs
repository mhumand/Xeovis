using Xeovis.Domain;

namespace Xeovis.Persistence.Proxies;

public class mctcVariable01
{
    public string Name { get; set; }
    public int Index { get; set; }
    public Guid Id { get; set; }
    public Guid mctcBundleId { get; set; }
    public List<double> Values { get; set; }
    public double StartingValue { get; set; }
    public double Range { get; set; }

    public mctcVariable01()
    {
        
    }
    
    public mctcVariable01(mctcVariable realObject)
    {
        Name = realObject.Name;
        Index = realObject.Index;
        Id = realObject.Id;
        mctcBundleId = realObject.mctcBundleId;
        Values = new List<double>(realObject.Values);
        StartingValue = realObject.StartingValue;
        Range = realObject.Range;
    }

    public mctcVariable ConvertToRealObject()
    {
        return new mctcVariable()
        {
            Name = Name,
            Index = Index,
            Id = Id,
            mctcBundleId = mctcBundleId,
            Values = [..Values],
            StartingValue = StartingValue,
            Range = Range
        };
    }
}