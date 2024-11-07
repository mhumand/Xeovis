using System.Collections.Generic;

namespace DatabaseReader.Definitions.Persistence
{
    public class DataFile
    {
        public string Name { get; set; }
        public string Notes { get; set; }
        public List<Batch> Batches { get; set; }
        public List<ModelVariable> ModelVariables { get; set; }

        public DataFile()
        {
            Batches = new List<Batch>();
            ModelVariables = new List<ModelVariable>();
        }
    }
}
