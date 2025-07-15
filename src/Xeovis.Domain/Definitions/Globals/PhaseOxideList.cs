namespace Xeovis.Domain.Definitions.Globals
{
    public static class PhaseOxideList
    {
        private static Dictionary<Guid, List<OxideType>> _oxideList { get; set; } = new();
        public static Dictionary<Guid, List<OxideType>> OxideList => _oxideList;

        private static Dictionary<Guid, List<OxideType>> _xeovisDefaults { get; set; } = new();
        public static Dictionary<Guid, List<OxideType>> XeovisDefaults => _xeovisDefaults;

        public static event EventHandler<PhaseOxideListChangedEventArgs>? PhaseOxideListChanged;

        public static void Initialize()
        {
            _oxideList = DefaultPhaseOxides.GetDefaultPhaseOxides();
            _oxideList.ToList().ForEach(x => _xeovisDefaults.Add(x.Key, x.Value.Select(x => x).ToList()));
        }

        public static void SetPhaseOxides(Dictionary<Guid, List<OxideType>> phaseOxides, bool invokeEvent)
        {
            if (phaseOxides == null || phaseOxides.Count == 0)
                phaseOxides = _xeovisDefaults;

            _oxideList.Clear();
            _oxideList = phaseOxides;
            if (invokeEvent)
                PhaseOxideListChanged?.Invoke(null, new PhaseOxideListChangedEventArgs(phaseOxides));
        }

        public static List<CationType> GetPhaseCations(Guid phaseNameId)
        {
            var res = _oxideList.TryGetValue(phaseNameId, out List<OxideType>? oxides);
            if (!res || oxides == null || oxides.Count == 0) return new();
            return oxides.Select(x => x.GetCationType()).ToList();
        }
    }
}
