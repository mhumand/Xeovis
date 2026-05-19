using Xeovis.Domain.Definitions.Defaults;
using Xeovis.Domain.Definitions.Experiment;
using Xeovis.Domain.Definitions.MCTC;

namespace Xeovis.Domain.Definitions.Globals
{
    public static class xeosVariableNameList
    {
        private static Dictionary<Guid, List<xeosVariableName>> _variableNames { get; set; } = new();
        public static Dictionary<Guid, List<xeosVariableName>> VariableNames => _variableNames;

        private static Dictionary<Guid, List<xeosVariableName>> _xeovisDefaults { get; set; } = new();
        public static Dictionary<Guid, List<xeosVariableName>> XeovisDefaults => _xeovisDefaults;

        // NEW: Flat caches for instantaneous O(1) lookups
        private static Dictionary<Guid, xeosVariableName> _flatVariableNamesCache { get; set; } = new();
        private static Dictionary<Guid, Guid> _variableIdToPhaseIdCache { get; set; } = new();

        public static event EventHandler<xeosVariableNameListChangedEventArgs>? xeosVariableNameListChanged;

        public static void Initialize()
        {
            _variableNames = DefaultXeosVariableNames.GetDefaultXeosVariableNames();
            _variableNames.ToList().ForEach(x => _xeovisDefaults.Add(x.Key, x.Value.Select(x => x.Copy(true)).ToList()));
        }

        public static void SetXeosVariableNames(Dictionary<Guid, List<xeosVariableName>> xeosVariableNames, bool invokeEvent)
        {
            if (xeosVariableNames == null || xeosVariableNames.Count == 0)
                xeosVariableNames = _xeovisDefaults;

            _variableNames.Clear();
            _variableNames = xeosVariableNames;
            
            // OPTIMIZATION: Build the flat caches whenever the main dictionary is updated
            _flatVariableNamesCache = _variableNames.SelectMany(x => x.Value).ToDictionary(v => v.Id, v => v);
            _variableIdToPhaseIdCache = _variableNames.SelectMany(x => x.Value.Select(v => new { PhaseId = x.Key, VarId = v.Id }))
                                                      .ToDictionary(x => x.VarId, x => x.PhaseId);

            if (invokeEvent)
            {
                var xeosVarIds = _flatVariableNamesCache.Keys; // Faster than recalculating
                xeosVariableCationList.SetXeosVariableCations(xeosVariableCationList.CationList.Where(x => xeosVarIds.Contains(x.Key)).ToDictionary(), false);
                xeosVariableNameListChanged?.Invoke(null, new xeosVariableNameListChangedEventArgs(xeosVariableNames));
            }
        }

        public static Guid GetPhaseNameSetIdByVariableId(Guid variableId)
        {
            // OPTIMIZED: O(1) lookup instead of SelectMany().FirstOrDefault()
            return _variableIdToPhaseIdCache.TryGetValue(variableId, out Guid phaseId) ? phaseId : Guid.Empty;
        }

        public static PhaseNameSet GetPhaseNameSetByVariableId(Guid variableId)
        {
            var nameId = GetPhaseNameSetIdByVariableId(variableId);
            return nameId != Guid.Empty ? PhaseNameList.GetNameSetById(nameId) : null;
        }

        public static xeosVariableName GetVariableNameById(Guid id, xeosVariableName defaultValue = null)
        {
            // OPTIMIZED: This eliminates the 7-second UI freeze entirely
            return _flatVariableNamesCache.TryGetValue(id, out xeosVariableName variable) ? variable : defaultValue;
        }

        public static xeosVariableName GetVariableName(string name, Guid phaseId, xeosVariableName defaultValue = null)
        {
            if (string.IsNullOrWhiteSpace(name)) return null;
            var res = _variableNames.TryGetValue(phaseId, out List<xeosVariableName> xeosVariableNames);
            return res ? xeosVariableNames.FirstOrDefault(x => x.Name.Equals(name, StringComparison.OrdinalIgnoreCase)) ?? defaultValue : defaultValue;
        }

        public static Guid GetVariableNameId(string name, Guid phaseId)
        {
            var xeosVarName = GetVariableName(name, phaseId);
            return xeosVarName != null ? xeosVarName.Id : Guid.Empty;
        }
    }
}

//
// namespace Xeovis.Domain.Definitions.Globals
// {
//     public static class xeosVariableNameList
//     {
//         private static Dictionary<Guid, List<xeosVariableName>> _variableNames { get; set; } = new();
//         public static Dictionary<Guid, List<xeosVariableName>> VariableNames => _variableNames;
//
//         private static Dictionary<Guid, List<xeosVariableName>> _xeovisDefaults { get; set; } = new();
//         public static Dictionary<Guid, List<xeosVariableName>> XeovisDefaults => _xeovisDefaults;
//
//         public static event EventHandler<xeosVariableNameListChangedEventArgs>? xeosVariableNameListChanged;
//
//         public static void Initialize()
//         {
//             _variableNames = DefaultXeosVariableNames.GetDefaultXeosVariableNames();
//             _variableNames.ToList().ForEach(x => _xeovisDefaults.Add(x.Key, x.Value.Select(x => x.Copy(true)).ToList()));
//         }
//
//         public static void SetXeosVariableNames(Dictionary<Guid, List<xeosVariableName>> xeosVariableNames, bool invokeEvent)
//         {
//             if (xeosVariableNames == null || xeosVariableNames.Count == 0)
//                 xeosVariableNames = _xeovisDefaults;
//
//             _variableNames.Clear();
//             _variableNames = xeosVariableNames;
//             if (invokeEvent)
//             {
//                 var xeosVarIds = xeosVariableNames.SelectMany(x => x.Value.Select(v => v.Id));
//                 xeosVariableCationList.SetXeosVariableCations(xeosVariableCationList.CationList.Where(x => xeosVarIds.Contains(x.Key)).ToDictionary(), false);
//                 xeosVariableNameListChanged?.Invoke(null, new xeosVariableNameListChangedEventArgs(xeosVariableNames));
//             }
//         }
//
//         public static Guid GetPhaseNameSetIdByVariableId(Guid variableId)
//         {
//             return _variableNames.FirstOrDefault(x => x.Value.Any(v => v.Id == variableId)).Key;
//         }
//
//         public static PhaseNameSet GetPhaseNameSetByVariableId(Guid variableId)
//         {
//             var nameId = GetPhaseNameSetIdByVariableId(variableId);
//             return PhaseNameList.GetNameSetById(nameId);
//         }
//
//         public static xeosVariableName GetVariableNameById(Guid id, xeosVariableName defaultValue = null)
//         {
//             return _variableNames.SelectMany(x => x.Value).FirstOrDefault(v => v.Id == id, defaultValue);
//         }
//
//         public static xeosVariableName GetVariableName(string name, Guid phaseId, xeosVariableName defaultValue = null)
//         {
//             if (string.IsNullOrWhiteSpace(name)) return null;
//             var res = _variableNames.TryGetValue(phaseId, out List<xeosVariableName> xeosVariableNames);
//             return res ? xeosVariableNames.FirstOrDefault(x => x.Name.Equals(name, StringComparison.InvariantCultureIgnoreCase), defaultValue) : defaultValue;
//         }
//
//         public static Guid GetVariableNameId(string name, Guid phaseId)
//         {
//             var xeosVarName = GetVariableName(name, phaseId);
//             return xeosVarName != null ? xeosVarName.Id : Guid.Empty;
//         }
//     }
// }
