using Xeovis.Presentation;

namespace Xeovis.Persistence.Proxies;

public class mctcChartSettings01
{
    public bool ShowAxesTitle { get; set; }
    public bool ShowChartTitle { get; set; }
    public bool LockAxesLimits { get; set; }
    public bool UseAxesLimits { get; set; }
    public double AxesMinLimit { get; set; }
    public double AxesMaxLimit { get; set; }

    public mctcChartSettings01()
    {
        
    }
    
    public mctcChartSettings01(mctcChartSettings realObject)
    {
        ShowAxesTitle = realObject.ShowAxesTitle;
        ShowChartTitle = realObject.ShowChartTitle;
        LockAxesLimits = realObject.LockAxesLimits;
        UseAxesLimits = realObject.UseAxesLimits;
        AxesMinLimit = realObject.AxesMinLimit;
        AxesMaxLimit = realObject.AxesMaxLimit;
    }

    public mctcChartSettings ConverToRealObject()
    {
        return new mctcChartSettings
        {
            ShowAxesTitle = ShowAxesTitle,
            ShowChartTitle = ShowChartTitle,
            LockAxesLimits = LockAxesLimits,
            UseAxesLimits = UseAxesLimits,
            AxesMinLimit = AxesMinLimit,
            AxesMaxLimit = AxesMaxLimit
        };
    }
}