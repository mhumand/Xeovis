namespace Xeovis.Domain.Definitions.Globals;

public class Component
{
    public string Formula { get; }

    // Direct link to its element (replaces GetCationType() switch statement!)
    public List<Element> AssociatedElements { get; private set; }

    private static readonly Dictionary<string, Component> _registry = new(StringComparer.OrdinalIgnoreCase);

    // 1. Pre-define your defaults
    public static readonly Component None = new Component("None", Element.None);
    public static readonly Component SiO2 = new Component("SiO2", Element.Si);
    public static readonly Component TiO2 = new Component("TiO2", Element.Ti);
    public static readonly Component Al2O3 = new Component("Al2O3", Element.Al);
    public static readonly Component Fe2O3 = new Component("Fe2O3", Element.Fe3);
    public static readonly Component Cr2O3 = new Component("Cr2O3", Element.Cr);
    public static readonly Component FeO = new Component("FeO", Element.Fe2);
    public static readonly Component FeOt = new Component("FeOt", Element.Fe2);
    public static readonly Component MnO = new Component("MnO", Element.Mn);
    public static readonly Component MgO = new Component("MgO", Element.Mg);
    public static readonly Component NiO = new Component("NiO", Element.Ni);
    public static readonly Component CaO = new Component("CaO", Element.Ca);
    public static readonly Component Na2O = new Component("Na2O", Element.Na);
    public static readonly Component K2O = new Component("K2O", Element.K);
    public static readonly Component P2O5 = new Component("P2O5", Element.P);
    public static readonly Component BaO = new Component("BaO", Element.Ba);
    public static readonly Component H2O = new Component("H2O", Element.H);
    public static readonly Component CO2 = new Component("CO2", Element.C);
    public static readonly Component Cl = new Component("Cl", Element.Cl);
    public static readonly Component F = new Component("F", Element.F);
    public static readonly Component O = new Component("O", Element.O);

    private Component(string formula, Element element)
    {
        Formula = formula;
        AssociatedElements = [element];
        _registry[formula] = this;
    }

    private Component(string formula, List<Element> elements)
    {
        Formula = formula;
        AssociatedElements = elements;
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
        return new Component(formula, element);
    }

    public static Component GetOrRegister(string formula, IEnumerable<string> elementSymbols)
    {
        if (string.IsNullOrWhiteSpace(formula)) return None;

        var elements = elementSymbols
            .Where(s => !string.IsNullOrWhiteSpace(s))
            .Select(s => Element.GetOrRegister(s))
            .ToList();

        if (_registry.TryGetValue(formula, out var existing))
        {
            // It already exists! The user is loading a project where they modified 
            // a built-in component. Overwrite the elements and return it!
            existing.UpdateElements(elements);
            return existing;
        }

        return new Component(formula, elements);
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

    // 3. Methods changing the elements
    public void UpdateElements(IEnumerable<Element> newElements)
    {
        AssociatedElements = newElements.ToList();
    }

    // Helper to get all registered components (for populating UI dropdowns, etc.)
    public static List<Component> GetAll() => _registry.Values.ToList();
    public static void ClearAll() => _registry.Clear();
    public bool HasElement(Element element) => AssociatedElements.Contains(element);

    public override string ToString() => Formula;

    public static bool operator ==(Component a, Component b) => a?.Formula == b?.Formula;
    public static bool operator !=(Component a, Component b) => !(a == b);
    public override bool Equals(object? obj) => obj is Component o && o.Formula == Formula;
    public override int GetHashCode() => Formula.GetHashCode();

    public static void Initialize()
    {
        ClearAll();
        GetXeovisDefaults().ToList().ForEach(x => 
            new Component(x.Key, x.Value.Select(el => Element.GetOrRegister(el)).ToList()));
    }

    public static Dictionary<string, List<string>> GetXeovisDefaults()
    {
        var dic = new Dictionary<string, List<string>>();
        dic["None"] = ["None"];
        dic["SiO2"] = ["Si"];
        dic["TiO2"] = ["Ti"];
        dic["Al2O3"] = ["Al"];
        dic["Fe2O3"] = ["Fe3"];
        dic["Cr2O3"] = ["Cr"];
        dic["FeO"] = ["Fe2"];
        dic["FeOt"] = ["Fe2"];
        dic["MnO"] = ["Mn"];
        dic["MgO"] = ["Mg"];
        dic["NiO"] = ["Ni"];
        dic["CaO"] = ["Ca"];
        dic["Na2O"] = ["Na"];
        dic["K2O"] = ["K"];
        dic["P2O5"] = ["P"];
        dic["BaO"] = ["Ba"];
        dic["H2O"] = ["H"];
        dic["CO2"] = ["C"];
        dic["Cl"] = ["Cl"];
        dic["F"] = ["F"];
        dic["O"] = ["O"];
        return dic;
    }
}