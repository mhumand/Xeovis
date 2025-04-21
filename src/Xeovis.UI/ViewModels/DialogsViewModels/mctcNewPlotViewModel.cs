using System;
using System.Collections.Generic;
using System.Linq;
using Xeovis.Base;
using Xeovis.Domain;
using Xeovis.UI.Models;

namespace Xeovis.UI.ViewModels.DialogsViewModels
{
    public partial class mctcNewPlotViewModel : ViewModelBase
    {
        public List<mctcBundle> mctcBundles;
        public Guid SelectedBundleId { get; set; }
        public List<xeosVariable> SelectedXeosVariables { get; set; }
        public Dictionary<ListableItem, List<ListableItem>> AllPhaseVariableItems { get; set; }

        public mctcNewPlotViewModel()
        {
            mctcBundles = new List<mctcBundle>();
            SelectedXeosVariables = new List<xeosVariable>();
            AllPhaseVariableItems = new Dictionary<ListableItem, List<ListableItem>>();
        }

        public Dictionary<ListableItem, List<ListableItem>> MakePhaseVaribleItemsDictionary(mctcBundle bundle)
        {
            if (bundle == null) return null;
            var phases = bundle.xeosVariables.Select(x => x.PhaseType).ToList().Distinct();
            AllPhaseVariableItems.Clear();
            foreach (var phase in phases)
            {
                var phaseItem = new ListableItem(EnumExtensions.GetDescriptionFromValue(phase), tag: phase, description: phase.ToString());
                var variableItems = GetXeosVariableList(phase, bundle);
                AllPhaseVariableItems.Add(phaseItem, variableItems);
            }

            return AllPhaseVariableItems;
        }

        private List<ListableItem> GetXeosVariableList(PhaseType phaseType, mctcBundle bundle)
        {
            if (bundle == null) return null;
            var allVariables = bundle.Batches.SelectMany(b => b.xeosVariables.Where(v => v.PhaseType == phaseType)).ToList();
            var groupedVars = allVariables.GroupBy(v => v.ToString());
            var list = new List<ListableItem>();
            foreach (var group in groupedVars)
            {
                var sampleVariable = group.First();
                list.Add(new ListableItem(sampleVariable));
            }

            return list;
        }
    }
}
