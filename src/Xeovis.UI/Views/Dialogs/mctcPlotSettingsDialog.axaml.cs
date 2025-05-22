using Avalonia.Controls;
using Avalonia.Data;
using System;
using System.Linq;
using Serilog;
using Xeovis.Base.Validations;
using Xeovis.Presentation;
using Xeovis.UI.Models.Enums;
using Xeovis.UI.ViewModels;
using Xeovis.UI.ViewModels.DialogsViewModels;

namespace Xeovis.UI;

public partial class mctcPlotSettingsDialog : Window
{
    private mctcPlotSettingsViewModel _viewModel = new mctcPlotSettingsViewModel();
    public mctcPlotSettingsViewModel ViewModel
    {
        get { return _viewModel; }
        set
        {
            _viewModel = value ?? new mctcPlotSettingsViewModel();
            DataContext = _viewModel;
            PrepareUI();
        }
    }

    private mctcPlotDto _mctcPlotDto => _viewModel.mctcPlotDto;

    public mctcPlotSettingsDialog()
    {
        InitializeComponent();
        Log.Information("Dialog opened.");
        if (this.Content is Control content)
            content.DataContext = this.DataContext;
    }

    private void PrepareUI()
    {
        // general settings
        var vmName = new LabelInputControlViewModel("Name", _mctcPlotDto.Name);
        vmName.ValidationRules.Add(new NotEmptyOrWhitespaceRule());
        vmName.ValidationRules.Add(new DuplicateNameRule(_viewModel.ExisitingPlotNames));
        licName.ViewModel = vmName;

        var vmNote = new LabelInputControlViewModel("Note", _mctcPlotDto.Note, isMultiLine: true);
        licNote.ViewModel = vmNote;

        var vmCategoryType = new LabelListControlViewModel(_viewModel.GetCategoryTypeListItems());
        vmCategoryType.Label = "Group points based on";
        vmCategoryType.SelectionMode = ItemSelectionMode.AlwaysSelected;
        vmCategoryType.SelectedIndex = vmCategoryType.Items.ToList().FindIndex(x => (mctcPlotPointsGroupingType)x.Tag == _mctcPlotDto.PointsGroupingType);
        llcCategoryType.ViewModel = vmCategoryType;

        var vmExcludeIrrelevantBatches = new CheckBoxControlViewModel("Exclude irrelevant batches", isChecked: _mctcPlotDto.ExcludeIrrelevantBatches);
        vmExcludeIrrelevantBatches.ToolTip = "Exlucdes those batches in which none of the phases have a cation that exists in the formula of the x-eos variable being plotted.";
        ckbxExcludeIrrelevantBatches.ViewModel = vmExcludeIrrelevantBatches;

        // chart settings
        var vmLockAxesLimits = new CheckBoxControlViewModel("Lock axes limits", isChecked: _mctcPlotDto.ChartSettings.LockAxesLimits);
        vmLockAxesLimits.ToolTip = "Locks all axes in all plots when changing mctc run number.";
        ckbxLockAxesLimits.ViewModel = vmLockAxesLimits;

        var vmGlobalAxisLimits = new CheckBoxControlViewModel("Use global axes limits", isChecked: _mctcPlotDto.ChartSettings.UseAxesLimits);
        ckbxGlobalAxisLimits.ViewModel = vmGlobalAxisLimits;

        var vmAxisMinLimit = new LabelInputControlViewModel("Axes min limit", _mctcPlotDto.ChartSettings.AxesMinLimit.ToString());
        vmAxisMinLimit.ValidationRules.Add(new NumericRule(0, 0.99));
        licAxesMinLimit.ViewModel = vmAxisMinLimit;
        licAxesMinLimit.TextInputChanged += licAxesMinLimit_TextInputChanged;

        var vmAxisMaxLimit = new LabelInputControlViewModel("Axes max limit", _mctcPlotDto.ChartSettings.AxesMaxLimit.ToString());
        vmAxisMaxLimit.ValidationRules.Add(new NumericRule(0.01, 1));
        licAxesMaxLimit.ViewModel = vmAxisMaxLimit;
        licAxesMaxLimit.TextInputChanged += licAxesMaxLimit_TextInputChanged;

        var binding = new Binding
        {
            Source = ckbxGlobalAxisLimits.ViewModel,
            Path = nameof(ckbxGlobalAxisLimits.ViewModel.IsChecked),
            Mode = BindingMode.OneWay,
            UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged,
        };
        licAxesMinLimit.Bind(IsEnabledProperty, binding);
        licAxesMaxLimit.Bind(IsEnabledProperty, binding);

        var vmShowChartTitle = new CheckBoxControlViewModel("Show chart title", isChecked: _mctcPlotDto.ChartSettings.ShowChartTitle);
        ckbxShowChartTitle.ViewModel = vmShowChartTitle;

        var vmShowAxesTitle = new CheckBoxControlViewModel("Show axes title", isChecked: _mctcPlotDto.ChartSettings.ShowAxesTitle);
        ckbxShowAxesTitle.ViewModel = vmShowAxesTitle;
    }

    private void licAxesMaxLimit_TextInputChanged(object? sender, Models.Events.TextInputChangedEventArgs e)
    {
        var rule = licAxesMinLimit.ViewModel.ValidationRules.FirstOrDefault(x => x is NumericRule) as NumericRule;
        rule?.SetMinMaxLimits(0, Math.Min(0.99, licAxesMaxLimit.ViewModel.GetDoubleValue()));
    }

    private void licAxesMinLimit_TextInputChanged(object? sender, Models.Events.TextInputChangedEventArgs e)
    {
        var rule = licAxesMaxLimit.ViewModel.ValidationRules.FirstOrDefault(x => x is NumericRule) as NumericRule;
        rule?.SetMinMaxLimits(Math.Max(0.01, licAxesMinLimit.ViewModel.GetDoubleValue()), 1);
    }

    private void btnCancel_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        this.Close(DialogResultType.Cancel);
    }

    private void btnSave_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        var plotDto = _viewModel.mctcPlotDto;
        plotDto.Name = licName.Value;
        plotDto.Note = licNote.Value;
        plotDto.ExcludeIrrelevantBatches = ckbxExcludeIrrelevantBatches.IsChecked;
        plotDto.PointsGroupingType = (mctcPlotPointsGroupingType)llcCategoryType.GetSelectedItem().Tag;

        plotDto.ChartSettings.ShowChartTitle = ckbxShowChartTitle.IsChecked;
        plotDto.ChartSettings.ShowAxesTitle = ckbxShowAxesTitle.IsChecked;
        plotDto.ChartSettings.LockAxesLimits = ckbxLockAxesLimits.IsChecked;
        plotDto.ChartSettings.UseAxesLimits = ckbxGlobalAxisLimits.IsChecked;
        plotDto.ChartSettings.AxesMinLimit = licAxesMinLimit.ViewModel.GetDoubleValue();
        plotDto.ChartSettings.AxesMaxLimit = licAxesMaxLimit.ViewModel.GetDoubleValue();

        Log.Information("mctc plot settings saved: {name}", plotDto.Name);
        this.Close(DialogResultType.OK);
    }
}
