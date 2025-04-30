using Avalonia.Controls;
using MsBox.Avalonia;
using System.Collections.Generic;
using Xeovis.Base.Validations;
using Xeovis.UI.Models.Enums;
using Xeovis.UI.ViewModels;

namespace Xeovis.UI;

public partial class RenameDialog : Window
{
    private string _newName;
    public string NewName => _newName;

    private List<string> _inUseNames;

    public RenameDialog()
    {
        InitializeComponent();
        DataContext = this;

        var vm = new LabelInputControlViewModel();
        vm.Label = "Name";
        textBox.ViewModel = vm;

        _newName = string.Empty;
        _inUseNames = new List<string>();
    }

    public RenameDialog(string currentName, List<string>? InUseNames = null) : this()
    {
        _newName = currentName;
        _inUseNames = InUseNames ?? new List<string>();
        textBox.ViewModel.Value = _newName;
        textBox.ViewModel.ValidationRules.Add(new NotEmptyOrWhitespaceRule());
        textBox.ViewModel.ValidationRules.Add(new DuplicateNameRule(InUseNames));
    }

    private void btnCancel_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        this.Close(DialogResultType.Cancel);
    }

    private async void btnOK_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        var name = textBox.ViewModel.Value;
        if (_inUseNames.Contains(name))
        {
            var mb = MessageBoxManager.GetMessageBoxStandard("Duplication", $"The name '{name}' is already in use!" +
                $"\nPlease try another name.", MsBox.Avalonia.Enums.ButtonEnum.Ok, MsBox.Avalonia.Enums.Icon.Error);
            mb.ShowWindowDialogAsync(this);
            return;
        }

        _newName = name;
        this.Close(DialogResultType.OK);
    }
}