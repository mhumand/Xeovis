using System.Collections.Generic;
using System.Linq;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Serilog;
using Xeovis.Base;
using Xeovis.Base.Validations;
using Xeovis.Presentation;
using Xeovis.UI.Models.Enums;
using Xeovis.UI.Models.Events;
using Xeovis.UI.ViewModels;
using Xeovis.UI.ViewModels.DialogsViewModels;

namespace Xeovis.UI.Views.Dialogs;

public partial class ElementsAndComponentsDialog : Window
{
    private ElementsAndComponentsViewModel _viewModel = new();
    
    public ElementsAndComponentsViewModel ViewModel
    {
        get { return _viewModel; }
        set
        {
            _viewModel = value ?? new ();
            DataContext = _viewModel;
            PrepareUI();
        }
    }
    
    private int _selectedIndex;
    
    public ElementsAndComponentsDialog()
    {
        InitializeComponent();
        Log.Information("Dialog opened.");
        if (this.Content is Control content)
            content.DataContext = this.DataContext;

        btnCancel.Click += btnCancel_Click;
        btnSave.Click += btnSave_Click;
        btnProjectDefaults.Click += btnProjectDefaults_Click;
        btnXeovisDefaults.Click += btnXeovisDefaults_Click;
        
        btnAddElement.Click += btnAddElement_Click;
        btnAddComponent.Click += btnAddComponent_Click;
        btnRemoveComponent.Click += btnRemoveComponent_Click;
        btnMoveUpComponent.Click += btnMoveUpComponent_Click;
        btnMoveDownComponent.Click += btnMoveDownComponent_Click;

        btnMoveUpComponent.IsEnabled = false;
        btnMoveDownComponent.IsEnabled = false;
        
        tableComponents.CellValueChanged += tableComponents_CellValueChanged;
        tableComponents.SelectionChanged += tableComponents_SelectionChanged;
    }

    private void PrepareUI()
    {
        licNewElement.ViewModel = new LabelInputControlViewModel("Add New Element", string.Empty);
        licNewComponent.ViewModel = new LabelInputControlViewModel("Add New Component (Formula)", string.Empty);
        
        var elementsItems = _viewModel.GetElementListableItems();
        llcElements.ViewModel = new LabelListControlViewModel(elementsItems)
        {
            Label = "Elements",
            IsRemovable = true,
            CanChangeOrder = true,
            SelectionMode = ItemSelectionMode.None
        };
        llcElements.ItemRemoved += llcElements_ItemRemoved;

        UpdateTable();
        UpdateElementDuplicateRule();
        UpdateComponentDuplicateRule();
    }

    private void btnRemoveComponent_Click(object? sender, RoutedEventArgs e)
    {
        var selectedIndex = tableComponents.dataGrid.SelectedIndex;
        if (selectedIndex < 0) return;
        
        _viewModel.Components.Remove(_viewModel.Components.ElementAt(selectedIndex).Key);
        UpdateComponentDuplicateRule();
        UpdateTable();
    }

    private void btnAddComponent_Click(object? sender, RoutedEventArgs e)
    {
        var name = licNewComponent.Value.Trim();
        if (string.IsNullOrWhiteSpace(name)) return;
        
        _viewModel.Components.Add(name, []);
        licNewComponent.ViewModel.Value = string.Empty;
        UpdateComponentDuplicateRule();
        UpdateTable();
    }

    private void btnAddElement_Click(object? sender, RoutedEventArgs e)
    {
        var name = licNewElement.Value.Trim();
        if (string.IsNullOrWhiteSpace(name)) return;
        _viewModel.Elements.Add(name);
        llcElements.ViewModel.SetItems(_viewModel.GetElementListableItems());
        
        licNewElement.ViewModel.Value = string.Empty;
        UpdateElementDuplicateRule();
        UpdateTable();
    }

    private void UpdateTable()
    {
        var table = _viewModel.GetComponentTable();
        tableComponents.PopulateTable(table);
    }

    private void llcElements_ItemRemoved(object? sender, ListableItemRemovedEventArgs e)
    {
        _viewModel.Elements.Remove(e.Item.ToString());
        foreach (var comp in _viewModel.Components)
        {
            comp.Value.Remove(e.Item.ToString());
        }
        
        UpdateElementDuplicateRule();
        UpdateTable();
    }

    private void UpdateElementDuplicateRule()
    {
        licNewElement.ViewModel.ValidationRules = new List<IValidationRule<string>>()
        {
            new DuplicateNameRule(_viewModel.Elements.Select(x => x).ToList(), ignoreCaseComparison: true)
        };
    }

    private void UpdateComponentDuplicateRule()
    {
        licNewComponent.ViewModel.ValidationRules = new List<IValidationRule<string>>()
        {
            new DuplicateNameRule(_viewModel.Components.Select(x => x.Key).ToList(), ignoreCaseComparison: true)
        };
    }
    
    private void btnProjectDefaults_Click(object? sender, RoutedEventArgs e)
    {
        _viewModel.FetchProjectDefaults();
        llcElements.ViewModel.SetItems(_viewModel.GetElementListableItems());
        UpdateTable();
        UpdateElementDuplicateRule();
        UpdateComponentDuplicateRule();
    }
    
    private void btnXeovisDefaults_Click(object? sender, RoutedEventArgs e)
    {
        _viewModel.FetchXeovisDefaults();
        llcElements.ViewModel.SetItems(_viewModel.GetElementListableItems());
        UpdateTable();
        UpdateElementDuplicateRule();
        UpdateComponentDuplicateRule();
    }

    private void tableComponents_CellValueChanged(object? sender, TableCellValueChangedEventArgs e)
    {
        var comp = _viewModel.Components.ElementAt(e.RowIndex);
        var columnModel = tableComponents.ViewModel.TableModel.GetColumnById(e.ColumnId);
        if (columnModel == null) return;
        if (!bool.TryParse(e.Value.ToString(), out var isChecked)) return;
        if (isChecked)
        {
            comp.Value.Add(columnModel.Header);
        }
        else
        {
            comp.Value.Remove(columnModel.Header);
        }
    }
    
    private void tableComponents_SelectionChanged(object? sender, TableSelectionChangedEventArgs e)
    {
        _selectedIndex = e.TrueRowIndex;
        if (_selectedIndex < 0)
        {
            btnMoveUpComponent.IsEnabled = false;
            btnMoveDownComponent.IsEnabled = false;
            return;
        }
        
        btnMoveUpComponent.IsEnabled = _selectedIndex >= 1;
        btnMoveDownComponent.IsEnabled = _selectedIndex < _viewModel.Components.Count - 1;
    }
    
    private void btnMoveDownComponent_Click(object? sender, RoutedEventArgs e)
    {
        var index = _selectedIndex;
        if (index < 0 || index >= _viewModel.Components.Count) return;
        _viewModel.Components = _viewModel.Components.MoveItemByIndex(index, index + 1);
        UpdateTable();
        tableComponents.SelectRow(index + 1, true);
    }

    private void btnMoveUpComponent_Click(object? sender, RoutedEventArgs e)
    {
        var index = _selectedIndex;
        if  (index < 1 || index >= _viewModel.Components.Count - 1) return;
        _viewModel.Components = _viewModel.Components.MoveItemByIndex(index, index - 1);
        UpdateTable();
        tableComponents.SelectRow(index - 1, true);
    }
    
    private void btnSave_Click(object? sender, RoutedEventArgs e)
    {
        this.Close(DialogResultType.OK);
    }

    private void btnCancel_Click(object? sender, RoutedEventArgs e)
    {
        this.Close(DialogResultType.Cancel);
    }
}