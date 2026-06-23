using System;
using System.Collections.Generic;
using System.Linq;
using Xeovis.Domain.Definitions;
using Xeovis.Domain.Definitions.Globals;
using Xeovis.Presentation;

namespace Xeovis.UI.ViewModels.DialogsViewModels
{
    public class SettingsVariableElementsViewModel : ViewModelBase
    {
        public TableModel GetProjectValuesTableModel()
        {
            var phaseComponents = xeosVariableElementList.ElementList;
            return GetTableModel(phaseComponents);
        }

        private TableModel GetTableModel(Dictionary<Guid, List<Element>> variableElements)
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

            var elements = Element.GetAll();
            foreach (var element in elements.Except([Element.None]))
            {
                var vals = xeosVarNames.Select(x =>
                    variableElements.ContainsKey(x.Id) && variableElements[x.Id].Contains(element)).ToList();
                var elementCol = new ColumnModel(element.ToString(), vals);
                elementCol.CanSort = false;
                columns.Add(elementCol);
            }

            var tableModel = new TableModel("", columns);
            tableModel.CanReorderColumns = false;
            tableModel.FrozenColumnCount = 2;
            return tableModel;
        }

        public TableModel GetXeovisDefaultsTableModel()
        {
            var phaseComponents = xeosVariableElementList.XeovisDefaults;
            return GetTableModel(phaseComponents);
        }

        public void SaveVariableElements(TableModel tableModel)
        {
            var idCol = tableModel.Columns[0];

            var elementCols = new Dictionary<Element, ColumnModel>();
            for (int i = 3; i < tableModel.Columns.Count; i++)
            {
                var element = Element.GetOrRegister(tableModel.Columns[i].Header);
                elementCols.Add(element, tableModel.Columns[i]);
            }

            var variableElements = new Dictionary<Guid, List<Element>>();
            for (int i = 0; i < idCol.DataCount; i++)
            {
                var res = Guid.TryParse(idCol.Values[i].ToString(), out Guid xeosVarId);
                if (!res || xeosVarId == Guid.Empty) continue;
                var elementsBool = elementCols.Values.Select(x => bool.Parse(x.Values[i].ToString())).ToList();
                if (!elementsBool.Any(x => x == true)) continue;
                var elements = elementCols.Keys.Where(x => bool.Parse(elementCols[x].Values[i].ToString())).ToList();
                variableElements.Add(xeosVarId, elements);
            }

            xeosVariableElementList.SetXeosVariableElements(variableElements, true);
        }
    }
}
