using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Xeovis.Base.Validations;

namespace Xeovis.UI.ViewModels
{
    public partial class LabelInputControlViewModel : LabelControlViewModelBase, INotifyDataErrorInfo
    {
        [ObservableProperty]
        private string value;

        [ObservableProperty]
        private bool isMultiLine;

        public List<IValidationRule<string>> ValidationRules { get; set; }

        private string _originalValue;
        public string OriginalValue => _originalValue;

        public LabelInputControlViewModel()
        {
            Value = string.Empty;
            _originalValue = string.Empty;
            IsMultiLine = false;
            this.PropertyChanged += LabelInputControlViewModel_PropertyChanged;
            ValidationRules = new List<IValidationRule<string>>();
        }

        public LabelInputControlViewModel(string label, string value, bool isReadOnly = false, bool isMultiLine = false) : this()
        {
            Label = label;
            Value = value;
            _originalValue = value;
            IsReadOnly = isReadOnly;
            IsMultiLine = isMultiLine;
        }

        private void LabelInputControlViewModel_PropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(Value))
                ValidateInputText(Value);
        }

        private void ValidateInputText(string value)
        {
            ClearErrors(nameof(Value));

            foreach (var rule in ValidationRules)
            {
                if (!rule.Validate(value.Trim()))
                {
                    AddError(nameof(Value), rule.ErrorMessage);
                    break; // Stop at first error
                }
            }
        }

        #region INotifyDataErrorInfo
        private readonly Dictionary<string, List<string>> _errors = new();

        public bool HasErrors => _errors.Count > 0;

        public event EventHandler<DataErrorsChangedEventArgs>? ErrorsChanged;

        public IEnumerable GetErrors(string propertyName)
        {
            if (propertyName != null && _errors.ContainsKey(propertyName))
                return _errors[propertyName];

            return null;
        }

        private void AddError(string propertyName, string error)
        {
            if (!_errors.ContainsKey(propertyName))
                _errors[propertyName] = new List<string>();

            if (!_errors[propertyName].Contains(error))
            {
                _errors[propertyName].Add(error);
                ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
            }
        }

        private void ClearErrors(string propertyName)
        {
            if (_errors.Remove(propertyName))
            {
                ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
            }
        }
        #endregion

        public double GetDoubleValue()
        {
            return Base.DoubleExtensions.GetDoubleValueFromText(Value);
        }
    }
}
