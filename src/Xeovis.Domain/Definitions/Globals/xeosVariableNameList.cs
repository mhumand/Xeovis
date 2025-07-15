using Xeovis.Domain.Definitions.Defaults;
using Xeovis.Domain.Definitions.Experiment;
using Xeovis.Domain.Definitions.MCTC;

namespace Xeovis.Domain.Definitions.Globals
{
    public static class xeosVariableNameList
    {
        private static Dictionary<Guid, List<xeosVariableName>> _xeosVariableNames { get; set; } = new();
        public static Dictionary<Guid, List<xeosVariableName>> xeosVariableNames => _xeosVariableNames;

        private static Dictionary<Guid, List<xeosVariableName>> _xeovisDefaults { get; set; } = new();
        public static Dictionary<Guid, List<xeosVariableName>> XeovisDefaults => _xeovisDefaults;

        public static event EventHandler<xeosVariableNameListChangedEventArgs>? xeosVariableNameListChanged;

        public static void Initialize()
        {
            _xeosVariableNames = DefaultXeosVariableNames.GetDefaultXeosVariableNames();
            _xeosVariableNames.ToList().ForEach(x => _xeovisDefaults.Add(x.Key, x.Value.Select(x => x.Copy(true)).ToList()));
        }

        public static void SetXeosVariableNames(Dictionary<Guid, List<xeosVariableName>> xeosVariableNames, bool invokeEvent)
        {
            if (xeosVariableNames == null || xeosVariableNames.Count == 0)
                xeosVariableNames = _xeovisDefaults;

            _xeosVariableNames.Clear();
            _xeosVariableNames = xeosVariableNames;
            if (invokeEvent)
                xeosVariableNameListChanged?.Invoke(null, new xeosVariableNameListChangedEventArgs(xeosVariableNames));
        }

        public static Guid GetPhaseNameSetIdByVariableId(Guid variableId)
        {
            return _xeosVariableNames.FirstOrDefault(x => x.Value.Any(v => v.PhaseId == variableId)).Key;
        }

        public static PhaseNameSet GetPhaseNameSetByVariableId(Guid variableId)
        {
            var nameId = GetPhaseNameSetIdByVariableId(variableId);
            return PhaseNameList.GetNameSetById(nameId);
        }

        public static xeosVariableName GetXeosVariableNameById(Guid id, xeosVariableName defaultValue = null)
        {
            return _xeosVariableNames.SelectMany(x => x.Value).FirstOrDefault(v => v.Id == id, defaultValue);
        }

        public static xeosVariableName GetXeosVariableName(string name, Guid phaseId, xeosVariableName defaultValue = null)
        {
            if (string.IsNullOrWhiteSpace(name)) return null;
            var res = _xeosVariableNames.TryGetValue(phaseId, out List<xeosVariableName> xeosVariableNames);
            return res ? xeosVariableNames.FirstOrDefault(x => x.Name.Equals(name, StringComparison.InvariantCultureIgnoreCase), defaultValue) : defaultValue;
        }

        public static Guid GetXeosVariableNameId(string name, Guid phaseId)
        {
            var xeosVarName = GetXeosVariableName(name, phaseId);
            return xeosVarName != null ? xeosVarName.Id : Guid.Empty;
        }
    }
}
