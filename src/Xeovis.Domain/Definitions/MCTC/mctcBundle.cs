using System.Drawing;

namespace Xeovis.Domain
{
    public class mctcBundle
    {
        public string Name { get; set; }
        public Guid Id { get; set; }
        public DateTime TimeLoaded { get; set; }
        public string FilesDirectory { get; set; }
        public List<Batch> Batches { get; set; }
        public List<mctcVariable> mctcVariables { get; set; }
        public List<xeosVariable> xeosVariables => Batches?.SelectMany(b => b.xeosVariables).ToList();
        public Color Color { get; set; }

        public mctcBundle()
        {
            Name = string.Empty;
            FilesDirectory = string.Empty;
            Id = Guid.NewGuid();
            Batches = new List<Batch>();
            mctcVariables = new List<mctcVariable>();
            Color = Color.Transparent;
        }

        public mctcBundle(List<Batch> batches, List<mctcVariable> mctcVariables) : this()
        {
            Batches = batches;
            this.mctcVariables = mctcVariables;
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
