using System;
using System.Collections.Generic;
using System.Linq;
using Xeovis.Domain;
using Xeovis.Domain.Definitions;
using Xeovis.Domain.Definitions.Globals;
using Xeovis.Presentation;

namespace Xeovis.UI.ViewModels.DialogsViewModels
{
    public class SettingsVariableCationsViewModel : ViewModelBase
    {
        public TableModel GetProjectValuesTableModel()
        {
            var phaseOxides = xeosVariableCationList.CationList;
            return GetTableModel(phaseOxides);
        }

        private TableModel GetTableModel(Dictionary<Guid, List<CationType>> variableCations)
        {
            var columns = new List<ColumnModel>();

            var phaseIds = xeosVariableNameList.VariableNames.Select(x => x.Key).ToList(); // phases with x-eos variables
            var xeosVarNames = phaseIds.SelectMany(x => xeosVariableNameList.VariableNames[x]).OrderBy(x => PhaseNameList.GetNameSetById(x.PhaseId).FullName);
            var idCol = new ColumnModel("Id", xeosVarNames.Select(x => x.Id.ToString()).ToList()) { IsVisible = false };
            columns.Add(idCol);
            
            var phaseNameCol = new ColumnModel("Phase", xeosVarNames.Select(x => xeosVariableNameList.GetPhaseNameSetByVariableId(x.Id).FullName).ToList());
            phaseNameCol.IsReadOnly = true;
            columns.Add(phaseNameCol);

            var variableNameCol = new ColumnModel("Variable", xeosVarNames.Select(x => $"{xeosVariableNameList.GetVariableNameById(x.Id).Name}({xeosVariableNameList.GetPhaseNameSetByVariableId(x.Id).KeyName})").ToList());
            variableNameCol.IsReadOnly = true;
            columns.Add(variableNameCol);

            var cations = Enum.GetValues(typeof(CationType)).Cast<CationType>().ToList();
            foreach (var oxideType in cations.Except([CationType.None]))
            {
                var vals = xeosVarNames.Select(x =>
                    variableCations.ContainsKey(x.Id) && variableCations[x.Id].Contains(oxideType)).ToList();
                var cationCol = new ColumnModel(oxideType.ToString(), vals);
                cationCol.CanSort = false;
                columns.Add(cationCol);
            }

            var tableModel = new TableModel("", columns);
            tableModel.CanReorderColumns = false;
            tableModel.FrozenColumnCount = 2;
            return tableModel;
        }

        public TableModel GetXeovisDefaultsTableModel()
        {
            var phaseOxides = xeosVariableCationList.XeovisDefaults;
            return GetTableModel(phaseOxides);
        }

        public void SaveVariableCations(TableModel tableModel)
        {
            var idCol = tableModel.Columns[0];

            var cationCols = new Dictionary<CationType, ColumnModel>();
            for (int i = 3; i < tableModel.Columns.Count; i++)
            {
                var cationType = Enum.Parse<CationType>(tableModel.Columns[i].Header, true);
                cationCols.Add(cationType, tableModel.Columns[i]);
            }

            var variableCations = new Dictionary<Guid, List<CationType>>();
            for (int i = 0; i < idCol.DataCount; i++)
            {
                var res = Guid.TryParse(idCol.Values[i].ToString(), out Guid xeosVarId);
                if (!res || xeosVarId == Guid.Empty) continue;
                var cationsBool = cationCols.Values.Select(x => bool.Parse(x.Values[i].ToString())).ToList();
                if (!cationsBool.Any(x => x == true)) continue;
                var oxides = cationCols.Keys.Where(x => bool.Parse(cationCols[x].Values[i].ToString())).ToList();
                variableCations.Add(xeosVarId, oxides);
            }

            xeosVariableCationList.SetXeosVariableCations(variableCations, true);
        }
    }
}
