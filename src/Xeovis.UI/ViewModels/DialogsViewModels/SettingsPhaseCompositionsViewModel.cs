using System;
using System.Collections.Generic;
using System.Linq;
using Xeovis.Domain;
using Xeovis.Domain.Definitions;
using Xeovis.Domain.Definitions.Globals;
using Xeovis.Presentation;

namespace Xeovis.UI.ViewModels.DialogsViewModels
{
    public class SettingsPhaseCompositionsViewModel : ViewModelBase
    {
        public TableModel GetProjectValuesTableModel()
        {
            var phaseOxides = PhaseOxideList.OxideList;
            return GetTableModel(phaseOxides);
        }

        private TableModel GetTableModel(Dictionary<Guid, List<OxideType>> phaseOxides)
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

            var oxides = Enum.GetValues(typeof(OxideType)).Cast<OxideType>().ToList();
            foreach (var oxideType in oxides.Except([OxideType.None]))
            {
                var vals = ids.Select(x => phaseOxides.ContainsKey(x) && phaseOxides[x].Contains(oxideType)).ToList();
                var oxideCol = new ColumnModel(oxideType.ToString(), vals);
                oxideCol.CanSort = false;
                columns.Add(oxideCol);
            }

            var tableModel = new TableModel("", columns);
            tableModel.CanReorderColumns = false;
            tableModel.FrozenColumnCount = 2;
            return tableModel;
        }

        public TableModel GetXeovisDefaultsTableModel()
        {
            var phaseOxides = PhaseOxideList.XeovisDefaults;
            return GetTableModel(phaseOxides);
        }

        public void SavePhaseOxides(TableModel tableModel)
        {
            var idCol = tableModel.Columns[0];

            var oxideCols = new Dictionary<OxideType, ColumnModel>();
            for (int i = 3; i < tableModel.Columns.Count; i++)
            {
                var oxideType = Enum.Parse<OxideType>(tableModel.Columns[i].Header, true);
                oxideCols.Add(oxideType, tableModel.Columns[i]);
            }

            var phaseOxides = new Dictionary<Guid, List<OxideType>>();
            for (int i = 0; i < idCol.DataCount; i++)
            {
                var res = Guid.TryParse(idCol.Values[i].ToString(), out Guid phaseId);
                if (!res || phaseId == Guid.Empty) continue;
                var oxidesBool = oxideCols.Values.Select(x => bool.Parse(x.Values[i].ToString())).ToList();
                if (!oxidesBool.Any(x => x == true)) continue;
                var oxides = oxideCols.Keys.Where(x => bool.Parse(oxideCols[x].Values[i].ToString())).ToList();
                phaseOxides.Add(phaseId, oxides);
            }

            PhaseOxideList.SetPhaseOxides(phaseOxides, true);
        }
    }
}
