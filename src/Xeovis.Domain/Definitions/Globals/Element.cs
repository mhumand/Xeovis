namespace Xeovis.Domain.Definitions.Globals;

public class Element
{
    public string Symbol { get; }

    // A centralized dictionary to keep track of all elements (default + user-defined)
    private static readonly Dictionary<string, Element> _registry = new(StringComparer.OrdinalIgnoreCase);

    // 1. Pre-define your defaults (acts just like an Enum)
    public static readonly Element None = new Element("None");
    public static readonly Element Si = new Element("Si");
    public static readonly Element Ti = new Element("Ti");
    public static readonly Element Al = new Element("Al");
    public static readonly Element Fe2 = new Element("Fe2");
    public static readonly Element Fe3 = new Element("Fe3");
    public static readonly Element Cr = new Element("Cr");
    public static readonly Element Mg = new Element("Mg");
    public static readonly Element Ca = new Element("Ca");
    public static readonly Element Mn = new Element("Mn");
    public static readonly Element Ni = new Element("Ni");
    public static readonly Element Na = new Element("Na");
    public static readonly Element K = new Element("K");
    public static readonly Element P = new Element("P");
    public static readonly Element Ba = new Element("Ba");
    public static readonly Element Cl = new Element("Cl");
    public static readonly Element F = new Element("F");
    public static readonly Element C = new Element("C");
    public static readonly Element O = new Element("O");
    public static readonly Element H = new Element("H");

    // Private constructor forces usage of the Register method
    private Element(string symbol)
    {
        Symbol = symbol;
        _registry[symbol] = this;
    }

    // 2. The method to get existing or create user-defined elements safely
    public static Element GetOrRegister(string symbol)
    {
        if (string.IsNullOrWhiteSpace(symbol)) return None;
            
        if (_registry.TryGetValue(symbol, out var existing))
            return existing;

        // It's a new user-defined element! Create and register it.
        return new Element(symbol);
    }

    public static IEnumerable<Element> GetAll() => _registry.Values;

    public override string ToString() => Symbol;
        
    public static bool operator ==(Element a, Element b) => a?.Symbol == b?.Symbol;
    public static bool operator !=(Element a, Element b) => !(a == b);
    public override bool Equals(object? obj) => obj is Element c && c.Symbol == Symbol;
    public override int GetHashCode() => Symbol.GetHashCode();
}