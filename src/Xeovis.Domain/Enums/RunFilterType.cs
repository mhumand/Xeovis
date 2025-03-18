namespace Xeovis.Domain
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
