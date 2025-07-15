using System;
using System.Collections.Generic;
using System.Linq;
using Xeovis.Base.Validations;
using Xeovis.Domain.Definitions;
using Xeovis.Domain.Definitions.Experiment;
using Xeovis.Presentation;

namespace Xeovis.UI.ViewModels.DialogsViewModels
{
    public class SettingsPhaseNamesViewModel : ViewModelBase
    {
        public TableModel GetProjectValuesTableModel()
        {
            var phases = PhaseNameList.PhaseNames.OrderBy(x => x.FullName).ToList();
            return GetTableModel(phases);
        }

        public TableModel GetXeovisDefaultsTableModel()
        {
            var phases = PhaseNameList.XeovisDefaults.OrderBy(x => x.FullName).ToList();
            return GetTableModel(phases);
        }

        private TableModel GetTableModel(List<PhaseNameSet> phaseNames)
        {
            var idCol = new ColumnModel("Id", phaseNames.Select(x => x.Id.ToString()).ToList()) { IsVisible = false };

            var allFullNames = phaseNames.Select(x => x.FullName).ToList();
            var fullNameCol = new ColumnModel("Full Name", allFullNames);
            fullNameCol.ValidationRules.Add(new NotEmptyOrWhitespaceRule());

            var allKeyNames = phaseNames.Select(x => x.KeyName).ToList();
            var keyNameCol = new ColumnModel("Key Name", allKeyNames);
            keyNameCol.ValidationRules.Add(new NotEmptyOrWhitespaceRule());

            var allOtherNames = phaseNames.SelectMany(x => x.OtherNames).ToList();
            var otherNamesCol = new ColumnModel("Other Names (comma-delimited)", phaseNames.Select(x => string.Join(", ", x.OtherNames.ToArray())).ToList());

            var allNames = allFullNames.Union(allKeyNames).Union(allOtherNames).ToList();
            var duplicateRule = new DuplicateNameRule(allNames, true);
            fullNameCol.ValidationRules.Add(duplicateRule);
            keyNameCol.ValidationRules.Add(duplicateRule);
            otherNamesCol.ValidationRules.Add(new DuplicateNameRule(allNames, true, true, ","));

            var tableModel = new TableModel("Phase Names", new List<ColumnModel>()
            {
                idCol, fullNameCol, keyNameCol, otherNamesCol
            });
            tableModel.CanReorderColumns = false;

            return tableModel;
        }

        public void UpdateValidationRules(TableModel tableModel)
        {
            var fullNameCol = tableModel.Columns[1];
            fullNameCol.ValidationRules.Clear();
            fullNameCol.ValidationRules.Add(new NotEmptyOrWhitespaceRule());

            var keyNameCol = tableModel.Columns[2];
            keyNameCol.ValidationRules.Clear();
            keyNameCol.ValidationRules.Add(new NotEmptyOrWhitespaceRule());

            var otherNamesCol = tableModel.Columns[3];
            otherNamesCol.ValidationRules.Clear();

            var fullNames = fullNameCol.Values.Cast<string>().ToList();
            var keyNames = keyNameCol.Values.Cast<string>().ToList();
            var otherNames = otherNamesCol.Values.SelectMany(x => x.ToString().Split(",", System.StringSplitOptions.TrimEntries)).ToList();
            otherNames.RemoveAll(x => string.IsNullOrWhiteSpace(x));

            var allNames = fullNames.Union(keyNames).Union(otherNames).ToList();
            var duplicateRule = new DuplicateNameRule(allNames, true);
            fullNameCol.ValidationRules.Add(duplicateRule);
            keyNameCol.ValidationRules.Add(duplicateRule);
            otherNamesCol.ValidationRules.Add(new DuplicateNameRule(allNames, true, true, ","));
        }

        public void SavePhaseNames(TableModel tableModel)
        {
            var phaseNames = new List<PhaseNameSet>();

            var idCol = tableModel.Columns[0];
            var fullNameCol = tableModel.Columns[1];
            var keyNameCol = tableModel.Columns[2];
            var otherNamesCol = tableModel.Columns[3];
            for (int i = 0; i < idCol.DataCount; i++)
            {
                var res = Guid.TryParse(idCol.Values[i].ToString(), out Guid id);
                if (id == Guid.Empty || !res) id = Guid.NewGuid();
                var fullName = fullNameCol.Values[i].ToString();
                if (string.IsNullOrWhiteSpace(fullName)) continue;
                var keyName = keyNameCol.Values[i].ToString();
                if (string.IsNullOrWhiteSpace(keyName)) continue;
                var otherNames = otherNamesCol.Values[i].ToString().Split(",", StringSplitOptions.TrimEntries).ToList();
                otherNames.RemoveAll(x => string.IsNullOrWhiteSpace(x));
                phaseNames.Add(new PhaseNameSet()
                {
                    Id = id,
                    FullName = fullName,
                    KeyName = keyName,
                    OtherNames = otherNames
                });
            }

            PhaseNameList.SetPhaseNames(phaseNames, true);
        }
    }
}
