using Newtonsoft.Json;
using Xeovis.Domain.Definitions.Globals;
using Xeovis.Persistence.Persistables;

namespace Xeovis.Persistence
{
    public class ProjectSettings01
    {
        public List<PhaseNameSet01> AllowedPhaseNames { get; set; }
        public Dictionary<Guid, List<xeosVariableName01>> PhaseXeosVariables { get; set; }
        
        [JsonProperty("PhaseOxides")]
        public Dictionary<Guid, List<string>> PhaseComponents { get; set; }
        
        [JsonProperty("xeosVariableCations")]
        public Dictionary<Guid, List<string>> xeosVariableElements { get; set; }
        
        public List<CustomElement01> CustomElements { get; set; }
        public List<CustomComponent01> CustomComponents { get; set; }
        
        public ProjectSettings01()
        {
            
        }
        
        public ProjectSettings01(ProjectSettings realObject)
        {
            AllowedPhaseNames = realObject.AllowedPhaseNames.Select(x => new PhaseNameSet01(x)).ToList();
            PhaseXeosVariables = realObject.PhaseXeosVariables.ToDictionary(o => o.Key, o => o.Value.Select(x => new xeosVariableName01(x)).ToList());
            PhaseComponents = realObject.PhaseComponents.ToDictionary(o => o.Key, o => o.Value.Select(x => x.Formula).ToList());
            xeosVariableElements = realObject.xeosVariableElements.ToDictionary(o => o.Key, o => o.Value.Select(x => x.Symbol).ToList());
            
            CustomElements = realObject.CustomElements?
                .Select(e => new CustomElement01 { Symbol = e.Symbol })
                .ToList() ?? new List<CustomElement01>();

            CustomComponents = realObject.CustomComponents?
                .Select(c => new CustomComponent01 
                { 
                    Formula = c.Formula, 
                    AssociatedElementSymbols = c.AssociatedElements.Select(x => x.Symbol).ToList()
                }).ToList() ?? new List<CustomComponent01>();
        }

        public ProjectSettings ConvertToRealObject()
        {
            // Rebuild user's custom elements
            var rebuiltCustomElements = new List<Element>();
            CustomElements?.ForEach(ce => rebuiltCustomElements.Add(Element.GetOrRegister(ce.Symbol)));

            // Rebuild user's custom components
            var rebuiltCustomComponents = new List<Component>();
            CustomComponents?.ForEach(cc => rebuiltCustomComponents.Add(Component.GetOrRegister(cc.Formula, cc.AssociatedElementSymbols)));
            
            return new ProjectSettings()
            {
                AllowedPhaseNames = AllowedPhaseNames.Select(x => x.ConvertToRealObject()).ToList(),
                PhaseXeosVariables = PhaseXeosVariables.ToDictionary(o => o.Key, o => o.Value.Select(x => x.ConvertToRealObject()).ToList()),
                
                PhaseComponents = PhaseComponents.ToDictionary(
                    o => o.Key, 
                    o => o.Value.Select(formula => Component.GetOrRegister(formula, "Unknown")).ToList()),
                
                xeosVariableElements = xeosVariableElements.ToDictionary(
                    o => o.Key, 
                    o => o.Value.Select(symbol => Element.GetOrRegister(symbol)).ToList()),
                
                CustomElements = rebuiltCustomElements,
                CustomComponents = rebuiltCustomComponents
            };
        }
    }
}
