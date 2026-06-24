namespace Xeovis.Domain.Definitions.Globals
{
    public static class xeosVariableElementList
    {
        private static Dictionary<Guid, List<Element>> _elementList { get; set; } = new();
        public static Dictionary<Guid, List<Element>> ElementList => _elementList;

        private static Dictionary<Guid, List<Element>> _xeovisDefaults { get; set; } = new();
        public static Dictionary<Guid, List<Element>> XeovisDefaults => _xeovisDefaults;

        public static event EventHandler<xeosVariableElementListChangedEventArgs>? xeosVariableElementListChanged;

        public static void Initialize()
        {
            _elementList = DefaultXeosVariableElements.GetDefaultXeosVariableCations();
            _elementList.ToList().ForEach(x => _xeovisDefaults.Add(x.Key, x.Value.Select(x => x).ToList()));
        }

        public static void SetXeosVariableElements(Dictionary<Guid, List<Element>> xeosVariableCations, bool invokeEvent)
        {
            if (xeosVariableCations == null || xeosVariableCations.Count == 0)
                xeosVariableCations = _xeovisDefaults;

            _elementList.Clear();
            _elementList = xeosVariableCations;
            if (invokeEvent)
                xeosVariableElementListChanged?.Invoke(null, new xeosVariableElementListChangedEventArgs(xeosVariableCations));
        }

        public static void UpdateFromProjectElements()
        {
            var elements = Element.GetAll();
            var anyRemoved = false;

            foreach (var kvp in _elementList)
            {
                int removedCount = kvp.Value.RemoveAll(x => !elements.Contains(x));
                if (removedCount > 0) anyRemoved = true;
            }

            if (anyRemoved)
                xeosVariableElementListChanged?.Invoke(null, new xeosVariableElementListChangedEventArgs(_elementList));
            
            // _xeovisDefaults.ToList().ForEach(e => e.Value.RemoveAll(x => !elements.Contains(x)));
        }
    }
}
