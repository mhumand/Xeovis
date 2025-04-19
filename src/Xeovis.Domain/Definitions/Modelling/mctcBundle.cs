namespace Xeovis.Domain
{
    public class mctcBundle
    {
        public string Name{ get; set; }
        public Guid Id { get; set; }
        public string FilesDirectory { get; set; }
        public List<Batch> Batches { get; set; }
        public List<mctcVariable> mctcVariables { get; set; }
        public List<xeosVariable> xeosVariables { get; set; }

        public mctcBundle()
        {
            Name = string.Empty;
            FilesDirectory = string.Empty;
            Id = Guid.NewGuid();
            Batches = new List<Batch>();
            mctcVariables = new List<mctcVariable>();
            xeosVariables = new List<xeosVariable>();
        }

        public mctcBundle(List<Batch> batches, List<mctcVariable> mctcVariables, List<xeosVariable> xeosVariables) : this()
        {
            Batches = batches;
            this.mctcVariables = mctcVariables;
            this.xeosVariables = xeosVariables;
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
