using System;
using System.Collections.Generic;

namespace Xeovis.Domain
{
    public class SubsetSettings
    {
        public int MaxPhaseCount { get; set; }

        private int _minPhaseCount;
        public int MinPhaseCount
        {
            get => Math.Min(3, _minPhaseCount);
            set => _minPhaseCount = value;
        }

        public List<PhaseType> MustContainAll { get; set; }
        public List<PhaseType> MustContainAny { get; set; }
        public List<PhaseType> MustContainUnused { get; set; }
        public List<PhaseType> MustNotContainAll { get; set; }

        public List<Experiment> IncludedStudies { get; set; }
        public List<Experiment> ExcludedStudies { get; set; }

        public double[] PressureBound = new double[2];
        public double[] TemperatureBound = new double[2];

    }
}
