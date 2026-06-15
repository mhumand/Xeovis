using System;
using System.Collections.Generic;
using System.Linq;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Serilog;
using Xeovis.Base.Validations;
using Xeovis.Presentation;
using Xeovis.UI.ViewModels;

namespace Xeovis.UI.Views.Dialogs;

public partial class ApiSettingsDialog : Window
{
    private struct EndPoint
    {
        public string Name = String.Empty;
        public string Type = string.Empty;
        public string Description= String.Empty;
        public string InputArguments =  String.Empty;

        public EndPoint()
        {
        }
    };
    
    public ApiSettingsDialog()
    {
        InitializeComponent();
        Log.Information("Dialog opened.");

        PopulateUI();
    }

    private void PopulateUI()
    {
        btnStop.IsEnabled = ApiHost.IsRunning;
        btnStart.IsEnabled = !ApiHost.IsRunning;
        licApiAddress.IsEnabled = !ApiHost.IsRunning;
        
        btnStart.Click += btnStartOnClick;
        btnStop.Click += btnStopOnClick;
        
        var vmApiAddress = new LabelInputControlViewModel("URL:", ApiHost.URL);
        vmApiAddress.ValidationRules.Add(new ValidUrlRule());
        vmApiAddress.IsLabelVisible = false;
        licApiAddress.ViewModel = vmApiAddress;

        PopulateTable();
    }

    private void PopulateTable()
    {
        var epStatus = new EndPoint()
        {
            Name = "/status",
            Type = "Get",
            Description = "Gives API status"
        };

        var epLoad = new EndPoint()
        {
            Name = "/Load",
            Type = "Post",
            Description = "Loads an mctc bundle",
            InputArguments = "(string Bundle_Name, string Bundle_Path)"
        };

        var epReload = new EndPoint()
        {
            Name = "/Reload",
            Type = "Post",
            Description = "Reloads an mctc bundle, and appends data if set",
            InputArguments = "(string Bundle_Name, bool Append)"
        };

        var epRefresh = new EndPoint()
        {
            Name = "/Refresh",
            Type = "Post",
            Description = "Refreshs plots and tables for an mctc bundle",
            InputArguments = "(string Bundle_Name)"
        };

        var endpoints = new List<EndPoint>() { epStatus, epLoad, epReload, epRefresh };
        
        var colEndpoints = new ColumnModel("Endpoint", endpoints.Select(x => x.Name).ToList());
        var colTypes = new ColumnModel("Type", endpoints.Select(x => x.Type).ToList());
        var colDescriptions = new ColumnModel("Description", endpoints.Select(x => x.Description).ToList());
        var colInputArguments = new ColumnModel("Input Arguments", endpoints.Select(x => x.InputArguments).ToList());
        
        var table = new TableModel("API", new List<ColumnModel>()
        {
            colEndpoints, colTypes, colDescriptions, colInputArguments
        });
        table.IsReadOnly = true;
        
        tableEndPoints.PopulateTable(table);
    }

    private async void btnStopOnClick(object? sender, RoutedEventArgs e)
    {
        if (!ApiHost.IsRunning) return;
        await ApiHost.Stop();
        
        btnStop.IsEnabled = false;
        btnStart.IsEnabled = true;
        licApiAddress.IsEnabled = true;
    }

    private async void btnStartOnClick(object? sender, RoutedEventArgs e)
    {
        if (ApiHost.IsRunning) return;
        await ApiHost.Start();
        
        btnStop.IsEnabled = true;
        btnStart.IsEnabled = false;
        licApiAddress.IsEnabled = false;
    }

    private void btnClose_OnClick(object? sender, RoutedEventArgs e)
    {
        this.Close();
    }
}