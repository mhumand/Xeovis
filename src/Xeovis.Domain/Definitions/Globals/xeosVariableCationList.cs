namespace Xeovis.Domain.Definitions.Globals
{
    public static class xeosVariableCationList
    {
        private static Dictionary<Guid, List<CationType>> _cationList { get; set; } = new();
        public static Dictionary<Guid, List<CationType>> CationList => _cationList;

        private static Dictionary<Guid, List<CationType>> _xeovisDefaults { get; set; } = new();
        public static Dictionary<Guid, List<CationType>> XeovisDefaults => _xeovisDefaults;

        public static event EventHandler<xeosVariableCationListChangedEventArgs>? xeosVariableCationListChanged;

        public static void Initialize()
        {
            _cationList = DefaultXeosVariableCations.GetDefaultXeosVariableCations();
            _cationList.ToList().ForEach(x => _xeovisDefaults.Add(x.Key, x.Value.Select(x => x).ToList()));
        }

        public static void SetXeosVariableCations(Dictionary<Guid, List<CationType>> xeosVariableCations, bool invokeEvent)
        {
            if (xeosVariableCations == null || xeosVariableCations.Count == 0)
                xeosVariableCations = _xeovisDefaults;

            _cationList.Clear();
            _cationList = xeosVariableCations;
            if (invokeEvent)
                xeosVariableCationListChanged?.Invoke(null, new xeosVariableCationListChangedEventArgs(xeosVariableCations));
        }
    }
}
