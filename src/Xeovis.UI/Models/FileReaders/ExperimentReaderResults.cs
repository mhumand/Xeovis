using System;
using System.Collections.Generic;
using Xeovis.Domain;

namespace Xeovis.UI.Models.FileReaders
{
    public class ExperimentReaderResults
    {
        public List<Experiment> Experiments { get; set; }
        public Dictionary<Guid, List<string>> UnknownPhaseNames { get; set; }
        public Dictionary<Guid, List<string>> UnknownPhaseFiles { get; set; }

        public bool HasUnknownPhaseNames => UnknownPhaseNames != null && UnknownPhaseNames.Count > 0;
        public bool HasUnknownPhaseFiles => UnknownPhaseFiles != null && UnknownPhaseFiles.Count > 0;

        public ExperimentReaderResults(List<Experiment> experiments, Dictionary<Guid, List<string>> unknownPhaseNames, Dictionary<Guid, List<string>> unknownPhaseFiles)
        {
            Experiments = experiments;
            UnknownPhaseNames = unknownPhaseNames;
            UnknownPhaseFiles = unknownPhaseFiles;
        }
    }
}
