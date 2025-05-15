using Xeovis.Domain;

namespace Xeovis.Persistence.Proxies;

public class xeosVariable01
{
    public double ObsVal { get; set; }
    public Guid Id { get; set; }
    public Dictionary<int, double> CalcVals { get; set; }
    public Guid BatchId { get; set; }
    public Guid mctcBundleId { get; set; }
    public PhaseType PhaseType { get; set; }
    public ModelVariableType VariableType { get; set; }
    public double DefaultValue { get; set; }

    public xeosVariable01()
    {
        
    }
    
    public xeosVariable01(xeosVariable realObject)
    {
        ObsVal = realObject.ObsVal;
        Id = realObject.Id;
        CalcVals = new Dictionary<int, double>(realObject.CalcVals);
        BatchId = realObject.BatchId;
        mctcBundleId = realObject.mctcBundleId;
        PhaseType = realObject.PhaseType;
        VariableType = realObject.VariableType;
        DefaultValue = realObject.DefaultValue;
    }

    public xeosVariable ConvertToRealObject()
    {
        return new xeosVariable()
        {
            ObsVal = ObsVal,
            Id = Id,
            CalcVals = new Dictionary<int, double>(CalcVals),
            BatchId = BatchId,
            mctcBundleId = mctcBundleId,
            PhaseType = PhaseType,
            VariableType = VariableType,
            DefaultValue = DefaultValue
        };
    }
}