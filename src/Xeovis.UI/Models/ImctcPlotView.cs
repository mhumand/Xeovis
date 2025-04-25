using System;
using Xeovis.Domain;
using Xeovis.UI.Models.Enums;

namespace Xeovis.UI.Models
{
    public interface ImctcPlotView
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public mctcPlotViewType ViewType { get; set; }

        public bool PopulateUI(mctcPlotDto mctcPlotDto, mctcBundle mctcBundle);
    }
}
