namespace Xeovis.Domain.Definitions.Globals
{
    public static class PhaseComponentList
    {
        private static Dictionary<Guid, List<Component>> _componentList { get; set; } = new();
        public static Dictionary<Guid, List<Component>> ComponentList => _componentList;

        private static Dictionary<Guid, List<Component>> _xeovisDefaults { get; set; } = new();
        public static Dictionary<Guid, List<Component>> XeovisDefaults => _xeovisDefaults;

        public static event EventHandler<PhaseComponentListChangedEventArgs>? PhaseComponentListChanged;

        public static void Initialize()
        {
            _componentList = DefaultPhaseComponents.GetDefaultPhaseComponents();
            _componentList.ToList().ForEach(x => _xeovisDefaults.Add(x.Key, x.Value.Select(x => x).ToList()));
        }

        public static void SetPhaseComponents(Dictionary<Guid, List<Component>> phaseComponents, bool invokeEvent)
        {
            if (phaseComponents == null || phaseComponents.Count == 0)
                phaseComponents = _xeovisDefaults;

            _componentList.Clear();
            _componentList = phaseComponents;
            if (invokeEvent)
                PhaseComponentListChanged?.Invoke(null, new PhaseComponentListChangedEventArgs(phaseComponents));
        }

        public static List<Element> GetPhaseElements(Guid phaseNameId)
        {
            var res = _componentList.TryGetValue(phaseNameId, out List<Component>? components);
            if (!res || components == null || components.Count == 0) return new();
            return components.SelectMany(x => x.AssociatedElements).ToList();
        }

        public static void UpdateFromProjectComponents()
        {
            var components = Component.GetAll();
            var anyRemoved = false;
            foreach (var kvp in _componentList)
            {
                var removedCount = kvp.Value.RemoveAll(x => !components.Contains(x));
                if (removedCount > 0) anyRemoved = true;
            }

            if (anyRemoved)
                PhaseComponentListChanged?.Invoke(null, new PhaseComponentListChangedEventArgs(_componentList));
            
            // _xeovisDefaults.ToList().ForEach(c => c.Value.RemoveAll(x => !components.Contains(x)));
        }
    }
}