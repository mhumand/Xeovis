using System.ComponentModel;

namespace Xeovis.UI.Models.Enums
{
    public enum FilterType
    {
        [Description("None")]
        None,

        [Description("Includes Any")]
        IncludesAny,

        [Description("Includes All")]
        IncludesAll,

        [Description("Excludes Any")]
        ExcludesAny,

        [Description("Excludes All")]
        ExcludesAll
    }
}
