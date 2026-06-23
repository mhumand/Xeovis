using Xeovis.Domain;
using Xeovis.Domain.Definitions.Globals;

namespace Xeovis.Persistence.Proxies;

public class Batch01
{
    public List<Guid> PhaseNameIds { get; set; }
    public int Index { get; set; }
    public Guid Id { get; set; }
    public Guid mctcBundleId { get; set; }
    public string ExperimentName { get; set; }
    public Guid ExperimentId { get; set; }
    public string ExpRunName { get; set; }
    public double ExpRunPressure { get; set; }
    public double ExpRunTemperature { get; set; }
    public Guid ExpRunId { get; set; }
    public Dictionary<string, double> BulkComposition { get; set; }
    public Dictionary<int, bool> IsFailed { get; set; }
    public List<xeosVariable01> xeosVariables { get; set; }

    public Batch01()
    {
    }

    public Batch01(Batch realObject)
    {
        PhaseNameIds = new List<Guid>(realObject.PhaseNameIds);
        Index = realObject.Index;
        Id = realObject.Id;
        mctcBundleId = realObject.mctcBundleId;
        ExperimentName = realObject.ExperimentName;
        ExperimentId = realObject.ExperimentId;
        ExpRunName = realObject.ExpRunName;
        ExpRunPressure = realObject.ExpRunPressure;
        ExpRunTemperature = realObject.ExpRunTemperature;
        ExpRunId = realObject.ExpRunId;
        BulkComposition = realObject.BulkComposition.ToDictionary(k => k.Key.Formula, v => v.Value);
        IsFailed = new Dictionary<int, bool>(realObject.IsFailed);
        xeosVariables = realObject.xeosVariables.Select(x => new xeosVariable01(x)).ToList();
    }

    public Batch ConvertToRealObject()
    {
        return new Batch()
        {
            PhaseNameIds = [..PhaseNameIds],
            Index = Index,
            Id = Id,
            mctcBundleId = mctcBundleId,
            ExperimentName = ExperimentName,
            ExperimentId = ExperimentId,
            ExpRunName = ExpRunName,
            ExpRunPressure = ExpRunPressure,
            ExpRunTemperature = ExpRunTemperature,
            ExpRunId = ExpRunId,
            BulkComposition = BulkComposition.ToDictionary(
                k => Component.GetOrRegister(k.Key, "Unknown"), 
                v => v.Value),
            IsFailed = new Dictionary<int, bool>(IsFailed),
            xeosVariables = xeosVariables.Select(x => x.ConvertToRealObject()).ToList()
        };
    }
}