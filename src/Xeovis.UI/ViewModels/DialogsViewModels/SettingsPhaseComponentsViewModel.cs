using System;
using System.Collections.Generic;
using System.Linq;
using Xeovis.Domain.Definitions;
using Xeovis.Domain.Definitions.Globals;
using Xeovis.Presentation;

namespace Xeovis.UI.ViewModels.DialogsViewModels
{
    public class SettingsPhaseComponentsViewModel : ViewModelBase
    {
        public TableModel GetProjectValuesTableModel()
        {
            var phaseComponents = PhaseComponentList.ComponentList;
            return GetTableModel(phaseComponents);
        }

        private TableModel GetTableModel(Dictionary<Guid, List<Component>> phaseComponents)
        {
            var columns = new List<ColumnModel>();

            var ids = PhaseNameList.PhaseNames.OrderBy(x => x.FullName).Select(x => x.Id).ToList();
            var idCol = new ColumnModel("Id", ids.Select(x => x.ToString()).ToList()) { IsVisible = false };
            columns.Add(idCol);

            var fullNameCol = new ColumnModel("Full Name", ids.Select(x => PhaseNameList.GetNameSetById(x).FullName).ToList());
            fullNameCol.IsReadOnly = true;
            columns.Add(fullNameCol);

            var keyNameCol = new ColumnModel("Key Name", ids.Select(x => PhaseNameList.GetNameSetById(x).KeyName).ToList());
            keyNameCol.IsReadOnly = true;
            columns.Add(keyNameCol);

            var components = Component.GetAll();
            foreach (var component in components.Except([Component.None]))
            {
                var vals = ids.Select(x => phaseComponents.ContainsKey(x) && phaseComponents[x].Contains(component)).ToList();
                var componentCol = new ColumnModel(component.ToString(), vals);
                componentCol.CanSort = false;
                columns.Add(componentCol);
            }

            var tableModel = new TableModel("", columns);
            tableModel.CanReorderColumns = false;
            tableModel.FrozenColumnCount = 2;
            return tableModel;
        }

        public TableModel GetXeovisDefaultsTableModel()
        {
            var phaseComponents = PhaseComponentList.XeovisDefaults;
            return GetTableModel(phaseComponents);
        }

        public void SavePhaseComponents(TableModel tableModel)
        {
            var idCol = tableModel.Columns[0];

            var compCols = new Dictionary<Component, ColumnModel>();
            for (int i = 3; i < tableModel.Columns.Count; i++)
            {
                var component = Component.Get(tableModel.Columns[i].Header);
                compCols.Add(component, tableModel.Columns[i]);
            }

            var phaseComponents = new Dictionary<Guid, List<Component>>();
            for (int i = 0; i < idCol.DataCount; i++)
            {
                var res = Guid.TryParse(idCol.Values[i].ToString(), out Guid phaseId);
                if (!res || phaseId == Guid.Empty) continue;
                var compsBool = compCols.Values.Select(x => bool.Parse(x.Values[i].ToString())).ToList();
                if (!compsBool.Any(x => x == true)) continue;
                var components = compCols.Keys.Where(x => bool.Parse(compCols[x].Values[i].ToString())).ToList();
                phaseComponents.Add(phaseId, components);
            }

            PhaseComponentList.SetPhaseComponents(phaseComponents, true);
        }
    }
}
