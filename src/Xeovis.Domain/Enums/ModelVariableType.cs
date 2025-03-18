using System.Collections.Generic;
using System.ComponentModel;

namespace Xeovis.Domain
{
    public enum ModelVariableType
    {
        None,

        [Description("x")]
        x,

        [Description("y")]
        y,

        [Description("c")]
        c,

        [Description("o")]
        o,

        [Description("n")]
        n,

        [Description("Q")]
        Q,

        [Description("Q1")]
        Q1,

        [Description("Q2")]
        Q2,

        [Description("Q3")]
        Q3,

        [Description("Qt")]
        Qt,

        [Description("f")]
        f,

        [Description("cr")]
        cr,

        [Description("t")]
        t,

        [Description("k")]
        k,

        [Description("s")]
        s,

        [Description("j")]
        j,

        [Description("i")]
        i,

        [Description("g")]
        g,

        [Description("m")]
        m,

        [Description("mj")]
        mj,

        [Description("ca")]
        ca,

        [Description("z")]
        z,

        [Description("a")]
        a

    }

    public static class ModelVariableTypeExtensions
    {
        public static bool IsOrderVariable(this ModelVariableType variableType)
        {
            var orderTypes = new List<ModelVariableType> { ModelVariableType.Q, ModelVariableType.Q1, ModelVariableType.Q2, ModelVariableType.Q3, ModelVariableType.Qt };

            return orderTypes.Contains(variableType);
        }
    }
}
