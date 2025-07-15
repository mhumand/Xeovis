
using Xeovis.Domain.Definitions.Experiment;
using Xeovis.Domain.Definitions.Globals;

namespace Xeovis.Domain.Definitions
{
    public static class PhaseNameList
    {
        private static List<PhaseNameSet> _phaseNames { get; set; } = new();
        public static List<PhaseNameSet> PhaseNames => _phaseNames;
        private static bool _anyName => _phaseNames != null && _phaseNames.Count > 0;

        private static List<PhaseNameSet> _xeovisDefaults { get; set; } = new();
        public static List<PhaseNameSet> XeovisDefaults => _xeovisDefaults;

        public static event EventHandler<PhaseNameListChangedEventArgs>? PhaseNameListChanged;

        public static void Initialize()
        {
            _phaseNames = DefaultPhaseNames.GetDefaultPhaseNames();
            _xeovisDefaults = _phaseNames.Select(x => x.Copy(true)).ToList();
        }

        public static void SetPhaseNames(List<PhaseNameSet> phaseNameList, bool invokeEvent)
        {
            if (phaseNameList == null || phaseNameList.Count == 0)
                phaseNameList = _xeovisDefaults;

            _phaseNames.Clear();
            _phaseNames = phaseNameList;
            if (invokeEvent)
                PhaseNameListChanged?.Invoke(null, new PhaseNameListChangedEventArgs(phaseNameList));
        }

        public static PhaseNameSet GetNameSetByString(string phaseName, PhaseNameSet defaultValue = null)
        {
            if (!_anyName) return defaultValue;
            return _phaseNames.FirstOrDefault(x => x.HasName(phaseName), defaultValue);
        }

        public static PhaseNameSet GetNameSetById(Guid id, PhaseNameSet defaultValue = null)
        {
            if (!_anyName) return defaultValue;
            return _phaseNames.FirstOrDefault(x => x.Id == id, defaultValue);
        }

        public static Guid GetNameSetIdByString(string name)
        {
            if (!_anyName) return Guid.Empty;
            var nameSet = GetNameSetByString(name);
            return nameSet != null ? nameSet.Id : Guid.Empty;
        }

        public static void Refresh()
        {
            _phaseNames = DefaultPhaseNames.GetDefaultPhaseNames();
            _xeovisDefaults = _phaseNames.Select(x => x.Copy(true)).ToList();
        }
    }
}
