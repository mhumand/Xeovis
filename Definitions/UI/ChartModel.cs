using System;
using System.Collections.Generic;
using System.Linq;

namespace DatabaseReader.Definitions.UI
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
        public bool IsLegendVisible { get; set; }
        public bool IsTitleVisible { get; set; }
        public bool IsXAxisLabelVisible { get; set; }
        public bool IsYAxisLabelVisible { get; set; }

        public ChartModel()
        {
            Curves = new List<CurveModel>();
            ID = Guid.NewGuid();

            IsLegendVisible = true;
            IsXAxisLabelVisible = true;
            IsYAxisLabelVisible = true;
        }
    }
}
