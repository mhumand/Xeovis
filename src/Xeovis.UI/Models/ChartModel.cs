using System;
using System.Collections.Generic;
using System.Linq;
using Xeovis.UI.Models.FileReaders;

namespace Xeovis.UI.Models
{
    public class ChartModel
    {
        public string Title { get; set; }
        public List<CurveModel> Curves { get; set; }
        public Guid ID { get; set; }
        public string XAxisLabel { get; set; }
        public string YAxisLabel { get; set; }
        public int MaxDataCount => Curves.Max(c => c.DataCount);
        public bool HasEqualDimensions { get; set; }
        public LegendModel Legend { get; set; }
        public bool IsTitleVisible { get; set; }
        public bool IsXAxisLabelVisible { get; set; }
        public bool IsYAxisLabelVisible { get; set; }
        public bool LockAxes {  get; set; }
        public bool IsGridVisible {  get; set; }

        public ChartModel()
        {
            Title = "ChartModelTitle";
            XAxisLabel = "X Axis";
            YAxisLabel = "Y Axis";
            Curves = new List<CurveModel>();
            Legend = new LegendModel();
            ID = Guid.NewGuid();

            IsXAxisLabelVisible = true;
            IsYAxisLabelVisible = true;
            IsTitleVisible = true;
            LockAxes = false;
            IsGridVisible = true;
        }

        public override string ToString()
        {
            return Title;
        }
    }
}
