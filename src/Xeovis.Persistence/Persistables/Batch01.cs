using Xeovis.Domain;

namespace Xeovis.Persistence.Proxies;

public class Batch01
{
    public List<PhaseType> Phases { get; set; }
    public int Index { get; set; }
    public Guid Id { get; set; }
    public Guid mctcBundleId { get; set; }
    public string ExperimentName { get; set; }
    public Guid ExperimentId { get; set; }
    public string ExpRunName { get; set; }
    public double ExpRunPressure { get; set; }
    public double ExpRunTemperature { get; set; }
    public Guid ExpRunId { get; set; }
    public Dictionary<OxideType, double> BulkComposition { get; set; }
    public Dictionary<int, bool> IsFailed { get; set; }
    public List<xeosVariable01> xeosVariables { get; set; }

    public Batch01()
    {
    }

    public Batch01(Batch realObject)
    {
        Phases = new List<PhaseType>(realObject.Phases);
        Index = realObject.Index;
        Id = realObject.Id;
        mctcBundleId = realObject.mctcBundleId;
        ExperimentName = realObject.ExperimentName;
        ExperimentId = realObject.ExperimentId;
        ExpRunName = realObject.ExpRunName;
        ExpRunPressure = realObject.ExpRunPressure;
        ExpRunTemperature = realObject.ExpRunTemperature;
        ExpRunId = realObject.ExpRunId;
        BulkComposition = new Dictionary<OxideType, double>(realObject.BulkComposition);
        IsFailed = new Dictionary<int, bool>(realObject.IsFailed);
        xeosVariables = realObject.xeosVariables.Select(x => new xeosVariable01(x)).ToList();
    }

    public Batch ConvertToRealObject()
    {
        return new Batch()
        {
            Phases = [..Phases],
            Index = Index,
            Id = Id,
            mctcBundleId = mctcBundleId,
            ExperimentName = ExperimentName,
            ExperimentId = ExperimentId,
            ExpRunName = ExpRunName,
            ExpRunPressure = ExpRunPressure,
            ExpRunTemperature = ExpRunTemperature,
            ExpRunId = ExpRunId,
            BulkComposition = new Dictionary<OxideType, double>(BulkComposition),
            IsFailed = new Dictionary<int, bool>(IsFailed),
            xeosVariables = xeosVariables.Select(x => x.ConvertToRealObject()).ToList()
        };
    }
}