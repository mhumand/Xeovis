namespace Xeovis.Domain.Definitions.Globals;

public class Component
    {
        public string Formula { get; }
        
        // Direct link to its element (replaces GetCationType() switch statement!)
        public List<Element> AssociatedElements { get; }
        public bool IsCommon { get; }

        private static readonly Dictionary<string, Component> _registry = new(StringComparer.OrdinalIgnoreCase);

        // 1. Pre-define your defaults
        public static readonly Component None = new Component("None", Element.None, false);
        public static readonly Component SiO2 = new Component("SiO2", Element.Si, true);
        public static readonly Component TiO2 = new Component("TiO2", Element.Ti, true);
        public static readonly Component Al2O3 = new Component("Al2O3", Element.Al, true);
        public static readonly Component Fe2O3 = new Component("Fe2O3", Element.Fe3, true);
        public static readonly Component Cr2O3 = new Component("Cr2O3", Element.Cr, true);
        public static readonly Component FeO = new Component("FeO", Element.Fe2, true);
        public static readonly Component FeOt = new Component("FeOt", Element.Fe2, true);
        public static readonly Component MnO = new Component("MnO", Element.Mn, true);
        public static readonly Component MgO = new Component("MgO", Element.Mg, true);
        public static readonly Component NiO = new Component("NiO", Element.Ni, true);
        public static readonly Component CaO = new Component("CaO", Element.Ca, true);
        public static readonly Component Na2O = new Component("Na2O", Element.Na, true);
        public static readonly Component K2O = new Component("K2O", Element.K, true);
        public static readonly Component P2O5 = new Component("P2O5", Element.P, true);
        public static readonly Component BaO = new Component("BaO", Element.Ba, true);
        public static readonly Component H2O = new Component("H2O", Element.H, true);
        public static readonly Component CO2 = new Component("CO2", Element.C, true);
        public static readonly Component Cl = new Component("Cl", Element.Cl, true);
        public static readonly Component F = new Component("F", Element.F, true);
        public static readonly Component O = new Component("O", Element.O, true);

        private Component(string formula, Element element, bool isCommon)
        {
            Formula = formula;
            AssociatedElements = [element];
            IsCommon = isCommon;
            _registry[formula] = this;
        }

        private Component(string formula, List<Element> elements, bool isCommon)
        {
            Formula = formula;
            AssociatedElements = elements;
            IsCommon = isCommon;
            _registry[formula] = this;
        }

        // 2. Method for users to define their own custom components!
        public static Component GetOrRegister(string formula, string elementSymbol, bool isCommon = false)
        {
            if (string.IsNullOrWhiteSpace(formula)) return None;

            if (_registry.TryGetValue(formula, out var existing))
                return existing;

            // Get or create the custom element to go with the custom component
            var element = Element.GetOrRegister(elementSymbol);
            return new Component(formula, element, isCommon);
        }
        
        public static Component GetOrRegister(string formula, List<string> elementSymbols, bool isCommon = false)
        {
            if (string.IsNullOrWhiteSpace(formula)) return None;

            if (_registry.TryGetValue(formula, out var existing))
                return existing;

            // Get or create the custom element to go with the custom component
            var elements = new List<Element>();
            elements.AddRange(elementSymbols.Select(x => Element.GetOrRegister(x)));
            return new Component(formula, elements, isCommon);
        }
        
        public static Component Get(string formula)
        {
            if (string.IsNullOrWhiteSpace(formula)) return None;

            // Searches the centralized dictionary
            if (_registry.TryGetValue(formula, out var existingComponent))
                return existingComponent;

            // Fallback if the string doesn't match any known component
            return None; 
        }
        
        // Helper to get all registered components (for populating UI dropdowns, etc.)
        public static IEnumerable<Component> GetAll() => _registry.Values;

        public override string ToString() => Formula;

        public static bool operator ==(Component a, Component b) => a?.Formula == b?.Formula;
        public static bool operator !=(Component a, Component b) => !(a == b);
        public override bool Equals(object? obj) => obj is Component o && o.Formula == Formula;
        public override int GetHashCode() => Formula.GetHashCode();
    }