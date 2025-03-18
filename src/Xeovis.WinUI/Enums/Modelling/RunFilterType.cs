using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseReader.Enums
{
    public enum RunFilterType
    {
        None,

        MustContainUnused,
        MustContainAll,
        MustNotContainAll,
        MustContainAny,

        MinPhaseCount,
        MaxPhaseCount,

        PressureBound,
        TemperatureBound,

        IncludedExperiments,
        ExcludedExperiments
    }
}
