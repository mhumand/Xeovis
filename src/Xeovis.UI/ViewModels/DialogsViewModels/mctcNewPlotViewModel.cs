using System;
using System.Collections.Generic;
using System.Linq;
using Xeovis.Domain;
using Xeovis.Domain.Definitions;
using Xeovis.Domain.Definitions.Experiment;
using Xeovis.Presentation;
using Xeovis.UI.Models;

namespace Xeovis.UI.ViewModels.DialogsViewModels
{
    public partial class mctcNewPlotViewModel : ViewModelBase
    {
        public List<mctcBundle> mctcBundles;
        public Guid SelectedBundleId { get; set; }
        public List<xeosVariable> SelectedXeosVariables { get; set; }
        public Dictionary<ListableItem, List<ListableItem>> AllPhaseVariableItems { get; set; }
        public List<string> ExistingPlotNames { get; set; }
        public mctcPlotDto mctcPlotDto { get; set; }


        public mctcNewPlotViewModel()
        {
            mctcBundles = new List<mctcBundle>();
            SelectedXeosVariables = new List<xeosVariable>();
            AllPhaseVariableItems = new Dictionary<ListableItem, List<ListableItem>>();
            ExistingPlotNames = new List<string>();
            mctcPlotDto = new mctcPlotDto();
        }

        public Dictionary<ListableItem, List<ListableItem>> MakePhaseVaribleItemsDictionary(mctcBundle bundle)
        {
            if (bundle == null) return null;
            var phases = bundle.xeosVariables.Select(x => x.PhaseKeyName).ToList().Distinct();
            AllPhaseVariableItems.Clear();
            foreach (var phase in phases)
            {
                var phaseNameSet = PhaseNameList.GetNameSetByString(phase);
                if (phaseNameSet == null) continue;
                var phaseItem = new ListableItem(phaseNameSet.ToString(), tag: phaseNameSet, description: phaseNameSet.KeyName);
                var variableItems = GetXeosVariableList(phaseNameSet, bundle);
                AllPhaseVariableItems.Add(phaseItem, variableItems);
            }

            return AllPhaseVariableItems;
        }

        private List<ListableItem> GetXeosVariableList(PhaseNameSet phaseNameSet, mctcBundle bundle)
        {
            if (bundle == null || phaseNameSet == null) return null;
            var allVariables = bundle.Batches.SelectMany(b => b.xeosVariables.Where(v => phaseNameSet.HasName(v.PhaseKeyName))).ToList();
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
