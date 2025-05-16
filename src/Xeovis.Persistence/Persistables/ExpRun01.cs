using Xeovis.Domain;

namespace Xeovis.Persistence.Proxies;

public class ExpRun01
{
    public string Name { get; set; }
    public Guid Id { get; set; }
    public List<Phase01> Phases { get; set; }
    public Dictionary<OxideType, double> BulkComposition { get; set; }
    public double Pressure { get; set; } //Kbar
    public double ErrorP { get; set; } // Kbar
    public double Temperature { get; set; } // °C
    public double ErrorT { get; set; } // °C
    public double Duration { get; set; }
    public double OxygenFugacity { get; set; }
    public Guid ExperimentId { get; set; }
    public string Laboratory { get; set; }
    public string Device { get; set; }
    public string Container { get; set; }
    public string Method { get; set; }
    public int Index { get; set; }
    public int ReadingIndex { get; set; }
    public RockType Rocktype { get; set; }

    public ExpRun01()
    {
    }

    public ExpRun01(ExpRun realObject)
    {
        Name = realObject.Name;
        Id = realObject.Id;
        Phases = realObject.Phases.Select(x => new Phase01(x)).ToList();
        BulkComposition = new Dictionary<OxideType, double>(realObject.BulkComposition);
        Pressure = realObject.Pressure;
        ErrorP = realObject.ErrorP;
        Temperature = realObject.Temperature;
        ErrorT = realObject.ErrorT;
        Duration = realObject.Duration;
        OxygenFugacity = realObject.OxygenFugacity;
        ExperimentId = realObject.ExperimentId;
        Laboratory = realObject.Laboratory;
        Device = realObject.Device;
        Container = realObject.Container;
        Method = realObject.Method;
        Index = realObject.Index;
        ReadingIndex = realObject.ReadingIndex;
        Rocktype = realObject.Rocktype;
    }

    public ExpRun ConvertToRealObject()
    {
        return new ExpRun()
        {
            Name = Name,
            Id = Id,
            Phases = Phases.Select(x => x.ConvertToRealObject()).ToList(),
            BulkComposition = new Dictionary<OxideType, double>(BulkComposition),
            Pressure = Pressure,
            ErrorP = ErrorP,
            Temperature = Temperature,
            ErrorT = ErrorT,
            Duration = Duration,
            OxygenFugacity = OxygenFugacity,
            ExperimentId = ExperimentId,
            Laboratory = Laboratory,
            Device = Device,
            Container = Container,
            Method = Method,
            Index = Index,
            ReadingIndex = ReadingIndex,
            Rocktype = Rocktype
        };
    }
}