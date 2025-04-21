using System.Drawing;

namespace Xeovis.Domain
{
    public class mctcPlotDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public Color Color { get; set; }
        public DateTime TimeCreated { get; set; }

        public Guid mctcBundleId { get; set; }
        public List<xeosVariable> xeosVariables { get; set; }
        public bool IsGridView { get; set; }

        public mctcPlotDto()
        {
            Id = Guid.NewGuid();
            Color = Color.Black;
            Name = "mctc Plot";
            xeosVariables = new List<xeosVariable>();
            IsGridView = false;
        }

        public mctcPlotDto(List<xeosVariable> xeosVariables, Guid mctcBundleId) : this()
        {
            this.mctcBundleId = mctcBundleId;
            this.xeosVariables = xeosVariables;
        }
    }
}
