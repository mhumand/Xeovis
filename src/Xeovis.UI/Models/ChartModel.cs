using System;
using System.Collections.Generic;
using System.Linq;

namespace Xeovis.UI.Models
{
    public class ChartModel
    {
        public string Title { get; set; }
        public List<CurveModel> Curves { get; set; }
        public Guid ID { get; set; }
        public int MaxDataCount => Curves.Max(c => c.DataCount);
        public bool HasEqualDimensions { get; set; }
        public LegendModel Legend { get; set; }
        public bool IsTitleVisible { get; set; }
        public bool IsGridVisible {  get; set; }
        public AxisModel XAxis { get; set; }
        public AxisModel YAxis { get; set; }

        public ChartModel()
        {
            Title = "ChartModelTitle";
            XAxis = new AxisModel("X Axis");
            YAxis = new AxisModel("Y Axis");
            Curves = new List<CurveModel>();
            Legend = new LegendModel();
            ID = Guid.NewGuid();
            IsTitleVisible = true;
            IsGridVisible = true;
        }

        public CurveModel GerCurveById(Guid id)
        {
            return Curves.FirstOrDefault(c => c.Id == id);
        }

        public override string ToString()
        {
            return Title;
        }
    }
}
