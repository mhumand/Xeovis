using System;
using System.Collections.Generic;
using System.Drawing;
using Xeovis.UI.Models.Enums;

namespace Xeovis.Domain
{
    public class mctcPlotDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public Color Color { get; set; }
        public DateTime TimeCreated { get; set; }

        public Guid mctcBundleId { get; set; }
        public List<xeosVariable> SampleXeosVariables { get; set; }
        public bool IsGridView { get; set; }
        public mctcPlotPointsGroupingType PointsGroupingType { get; set; }
        public bool ExcludeIrrelevantBatches { get; set; }

        public mctcPlotDto()
        {
            Id = Guid.NewGuid();
            Color = Color.Black;
            Name = "mctc Plot";
            SampleXeosVariables = new List<xeosVariable>();
            IsGridView = false;
            PointsGroupingType = mctcPlotPointsGroupingType.Batch;
            ExcludeIrrelevantBatches = true;
        }

        public mctcPlotDto(List<xeosVariable> SampleXeosVariables, Guid mctcBundleId) : this()
        {
            this.mctcBundleId = mctcBundleId;
            this.SampleXeosVariables = SampleXeosVariables;
        }
    }
}
