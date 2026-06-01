using Avalonia.Controls;
using MsBox.Avalonia;
using System.Collections.Generic;
using Serilog;
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
        Log.Information("Dialog opened.");
        
        this.Loaded += RenameDialog_Loaded;

        var vm = new LabelInputControlViewModel();
        vm.Label = "Name";
        textBox.ViewModel = vm;
        textBox.DataContext = vm;
        
        _newName = string.Empty;
        _inUseNames = new List<string>();
    }

    private void RenameDialog_Loaded(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        textBox.FocusAndSelectAll();
    }

    public RenameDialog(string currentName, List<string>? inUseNames = null, int maxNameLength = 100) : this()
    {
        _newName = currentName;
        _inUseNames = inUseNames ?? new List<string>();
        textBox.ViewModel.Value = _newName;
        textBox.ViewModel.ValidationRules.Add(new NotEmptyOrWhitespaceRule());
        textBox.ViewModel.ValidationRules.Add(new DuplicateNameRule(_inUseNames));
        textBox.ViewModel.ValidationRules.Add(new MaxNameLengthRule(maxNameLength));
    }

    private void btnCancel_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        this.Close(DialogResultType.Cancel);
    }

    private void btnOK_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
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
        Log.Information("Rename completed.");
        this.Close(DialogResultType.OK);
    }
}