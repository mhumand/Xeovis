using System;
using System.Collections.Generic;
using System.Linq;
using Xeovis.Domain.Definitions;
using Xeovis.Domain.Definitions.Experiment;
using Xeovis.Domain.Definitions.Globals;
using Xeovis.Domain.Definitions.MCTC;
using Xeovis.Presentation;

namespace Xeovis.UI.ViewModels.DialogsViewModels
{
    public class SettingsXeosVariablesViewModel : ViewModelBase
    {
        public TableModel GetProjectValuesTableModel()
        {
            var phases = PhaseNameList.PhaseNames.OrderBy(x => x.FullName).ToList();
            return GetTableModel(phases, xeosVariableNameList.VariableNames);
        }

        public TableModel GetXeovisDefaultsTableModel()
        {
            var phases = PhaseNameList.PhaseNames.OrderBy(x => x.FullName).ToList();
            return GetTableModel(phases, xeosVariableNameList.XeovisDefaults);
        }

        private TableModel GetTableModel(List<PhaseNameSet> phaseNames, Dictionary<Guid, List<xeosVariableName>> list)
        {
            var idCol = new ColumnModel("Id", phaseNames.Select(x => x.Id.ToString()).ToList()) { IsVisible = false };

            var allFullNames = phaseNames.Select(x => x.FullName).ToList();
            var fullNameCol = new ColumnModel("Full Name", allFullNames);
            fullNameCol.IsReadOnly = true;

            var allKeyNames = phaseNames.Select(x => x.KeyName).ToList();
            var keyNameCol = new ColumnModel("Key Name", allKeyNames);
            keyNameCol.IsReadOnly = true;

            var xeosVarNamesCol = new ColumnModel("x-eos Variables (comma-delimited)",
                phaseNames.Select(x => string.Join(", ", GetXeosVarNames(list, x.Id).Select(x => x.Name).ToArray())).ToList());

            var tableModel = new TableModel("", new List<ColumnModel>()
            {
                idCol, fullNameCol, keyNameCol, xeosVarNamesCol
            });
            tableModel.CanReorderColumns = false;

            return tableModel;
        }

        private List<xeosVariableName> GetXeosVarNames(Dictionary<Guid, List<xeosVariableName>> list, Guid phaseID)
        {
            var res = list.TryGetValue(phaseID, out List<xeosVariableName>? xeosVarNames);
            return res && xeosVarNames != null ? xeosVarNames : new();
        }

        public void SaveXeosVariables(TableModel tableModel)
        {
            var phaseXeosVariables = new Dictionary<Guid, List<xeosVariableName>>();

            var idCol = tableModel.Columns[0];
            var varNamesCol = tableModel.Columns[3];
            for (int i = 0; i < idCol.DataCount; i++)
            {
                var resId = Guid.TryParse(idCol.Values[i].ToString(), out Guid phaseId);
                if (!resId || phaseId == Guid.Empty) continue;
                var xeosVarNames = varNamesCol.Values[i].ToString().Split(",", StringSplitOptions.TrimEntries).Distinct().ToList();
                xeosVarNames.RemoveAll(x => string.IsNullOrWhiteSpace(x));
                if (xeosVarNames.Count == 0) continue;
                var resVars = xeosVariableNameList.VariableNames.TryGetValue(phaseId, out List<xeosVariableName>? currentVars);
                if (!resVars || currentVars == null) currentVars = new();
                var overlap = currentVars.Where(x => xeosVarNames.Contains(x.Name));
                var newXeosVars = xeosVarNames.Where(x => !currentVars.Select(v => v.Name).Contains(x)).Select(x => new xeosVariableName(x, phaseId));
                phaseXeosVariables.Add(phaseId, newXeosVars.Union(overlap).ToList());
            }

            xeosVariableNameList.SetXeosVariableNames(phaseXeosVariables, true);
        }
    }
}
