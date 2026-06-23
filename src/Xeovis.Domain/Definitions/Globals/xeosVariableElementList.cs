namespace Xeovis.Domain.Definitions.Globals
{
    public static class xeosVariableElementList
    {
        private static Dictionary<Guid, List<Element>> _cationList { get; set; } = new();
        public static Dictionary<Guid, List<Element>> ElementList => _cationList;

        private static Dictionary<Guid, List<Element>> _xeovisDefaults { get; set; } = new();
        public static Dictionary<Guid, List<Element>> XeovisDefaults => _xeovisDefaults;

        public static event EventHandler<xeosVariableElementListChangedEventArgs>? xeosVariableElementListChanged;

        public static void Initialize()
        {
            _cationList = DefaultXeosVariableCations.GetDefaultXeosVariableCations();
            _cationList.ToList().ForEach(x => _xeovisDefaults.Add(x.Key, x.Value.Select(x => x).ToList()));
        }

        public static void SetXeosVariableElements(Dictionary<Guid, List<Element>> xeosVariableCations, bool invokeEvent)
        {
            if (xeosVariableCations == null || xeosVariableCations.Count == 0)
                xeosVariableCations = _xeovisDefaults;

            _cationList.Clear();
            _cationList = xeosVariableCations;
            if (invokeEvent)
                xeosVariableElementListChanged?.Invoke(null, new xeosVariableElementListChangedEventArgs(xeosVariableCations));
        }
    }
}
