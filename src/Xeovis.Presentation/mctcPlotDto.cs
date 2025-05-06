using System.Drawing;
using Xeovis.Domain;

namespace Xeovis.Presentation
{
    public class mctcPlotDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Note { get; set; }
        public Color Color { get; set; }
        public DateTime TimeCreated { get; set; }
        public bool IsVisible { get; set; }

        public Guid mctcBundleId { get; set; }
        public List<xeosVariable> SampleXeosVariables { get; set; }
        public bool IsGridView { get; set; }
        public mctcPlotPointsGroupingType PointsGroupingType { get; set; }
        public bool ExcludeIrrelevantBatches { get; set; }

        public mctcChartSettings ChartSettings { get; set; }

        public mctcPlotDto()
        {
            Id = Guid.NewGuid();
            Color = Color.Black;
            Name = string.Empty;
            Note = string.Empty;
            IsVisible = true;
            SampleXeosVariables = new List<xeosVariable>();
            IsGridView = false;
            PointsGroupingType = mctcPlotPointsGroupingType.Batch;
            ExcludeIrrelevantBatches = true;
            ChartSettings = new mctcChartSettings();
        }

        public mctcPlotDto(List<xeosVariable> SampleXeosVariables, Guid mctcBundleId) : this()
        {
            this.mctcBundleId = mctcBundleId;
            this.SampleXeosVariables = SampleXeosVariables;
        }

        public override string ToString()
        {
            return Name;
        }
        
        public mctcPlotDto Copy(bool copyId = false)
        {
            var copy = new mctcPlotDto();
            copy.Id = copyId ? Id : Guid.NewGuid();
            copy.Name = Name;
            copy.Note = Note;
            copy.TimeCreated = TimeCreated;
            copy.Color = Color;
            copy.mctcBundleId = mctcBundleId;
            copy.SampleXeosVariables = SampleXeosVariables;
            copy.IsGridView = IsGridView;
            copy.PointsGroupingType = PointsGroupingType;
            copy.ExcludeIrrelevantBatches = ExcludeIrrelevantBatches;
            copy.ChartSettings = ChartSettings.Copy();

            return copy;
        }
    }
}
