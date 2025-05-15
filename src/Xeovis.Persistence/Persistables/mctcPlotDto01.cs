using System.Drawing;
using Xeovis.Presentation;

namespace Xeovis.Persistence.Proxies;

public class mctcPlotDto01
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Note { get; set; }
    public Color Color { get; set; }
    public DateTime TimeCreated { get; set; }
    public bool IsVisible { get; set; }
    public Guid mctcBundleId { get; set; }
    public List<xeosVariable01> SampleXeosVariables { get; set; }
    public bool IsGridView { get; set; }
    public mctcPlotPointsGroupingType PointsGroupingType { get; set; }
    public bool ExcludeIrrelevantBatches { get; set; }
    public mctcChartSettings01 ChartSettings { get; set; }

    public mctcPlotDto01()
    {
        
    }
    
    public mctcPlotDto01(mctcPlotDto realObject)
    {
        Id = realObject.Id;
        Name = realObject.Name;
        Note = realObject.Note;
        Color = realObject.Color;
        TimeCreated = realObject.TimeCreated;
        IsVisible = realObject.IsVisible;
        mctcBundleId = realObject.mctcBundleId;
        SampleXeosVariables = realObject.SampleXeosVariables.Select(x => new xeosVariable01(x)).ToList();
        IsGridView = realObject.IsGridView;
        PointsGroupingType = realObject.PointsGroupingType;
        ExcludeIrrelevantBatches = realObject.ExcludeIrrelevantBatches;
        ChartSettings = new mctcChartSettings01(realObject.ChartSettings);
    }

    public mctcPlotDto ConvertToRealObject()
    {
        return new mctcPlotDto()
        {
            Id = Id,
            Name = Name,
            Note = Note,
            Color = Color,
            TimeCreated = TimeCreated,
            IsVisible = IsVisible,
            mctcBundleId = mctcBundleId,
            SampleXeosVariables = SampleXeosVariables.Select(x => x.ConvertToRealObject()).ToList(),
            IsGridView = IsGridView,
            PointsGroupingType = PointsGroupingType,
            ExcludeIrrelevantBatches = ExcludeIrrelevantBatches,
            ChartSettings = ChartSettings.ConverToRealObject()
        };
    }
}