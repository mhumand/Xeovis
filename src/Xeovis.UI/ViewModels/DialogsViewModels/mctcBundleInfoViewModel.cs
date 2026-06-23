using System;
using System.Collections.Generic;
using System.Linq;
using Xeovis.Base;
using Xeovis.Domain;
using Xeovis.Presentation;
using Xeovis.UI.Models;

namespace Xeovis.UI.ViewModels.DialogsViewModels;

public class mctcBundleInfoViewModel : ViewModelBase
{
    public mctcBundle mctcBundle;
    public List<string> OtherNames { get; set; }
    
    public mctcBundleInfoViewModel()
    {
        mctcBundle = new ();
        OtherNames = new();
    }
    
    public mctcBundleInfoViewModel(mctcBundle bundle, List<string> otherNames) : this()
    {
        mctcBundle = bundle;
        OtherNames = otherNames;
    }

    public List<ListableItem> GetBatcheListItems()
    {
        var batches = mctcBundle.Batches.OrderBy(x => x.ExperimentName).ToList();
        var no = Enumerable.Range(1, batches.Count).ToList();
        
        var items = new List<ListableItem>();
        for (int i = 0; i < mctcBundle.Batches.Count; i++)
        {
            var batch = mctcBundle.Batches[i];
            var name = $"{no[i]} - {batch.ExperimentName} - {batch.ExpRunName}";
            var item = new ListableItem(name, 
                description: $"({batch.ExpRunTemperature} °C, {batch.ExpRunPressure} kbar)")
            {
                PresenterId = batch.Id
            };
            items.Add(item);
        }
        return items;
    }
    
    public TableModel GetBatchesTableModel()
    {
        var batches = mctcBundle.Batches.OrderBy(x => x.ExperimentName).ToList();
        
        var columns = new List<ColumnModel>()
        {
            new ColumnModel("ID", batches.Select(x => x.Id.ToString()).ToList()) {IsVisible = false},
            new ColumnModel("#", Enumerable.Range(1, batches.Count).ToList())
            {
                IsReadOnly =  true
            },
            new ColumnModel("Experiment", batches.Select(x => x.ExperimentName).ToList()),
            new ColumnModel("Run", batches.Select(x => x.ExpRunName).ToList()),
            new ColumnModel("T [°C]", batches.Select(x => x.ExpRunTemperature).ToList()),
            new ColumnModel("P [kbar]", batches.Select(x => x.ExpRunPressure).ToList()),
            new ColumnModel("Component : Composition", batches.Select(x => x.BulkComposition.ToStringInline()).ToList())
            {
                IsReadOnly = true
            },
            new ColumnModel("mctc Run : Is Failed", batches.Select(x => x.IsFailed.ToStringInline()).ToList())
            {
                IsReadOnly =  true
            }
        };
        
        var tableModel = new TableModel("Batches", columns);
        tableModel.IsReadOnly = true; //temp
        return tableModel;
    }

    public TableModel GetXeosVariablesTableModel(Guid batchID)
    {
        var batch = mctcBundle.Batches.FirstOrDefault(x => x.Id == batchID);
        if (batch == null) return null;
        var vars = batch.xeosVariables;

        var columns = new List<ColumnModel>()
        {
            new ColumnModel("ID", vars.Select(x => x.Id.ToString()).ToList()) {IsVisible = false},
            new ColumnModel("#", Enumerable.Range(1, vars.Count).ToList())
            {
                IsReadOnly =  true
            },
            new ColumnModel("Name", vars.Select(x => x.VariableName.ToString()).ToList()),
            new ColumnModel("Phase Key", vars.Select(x => x.PhaseKeyName).ToList()),
            new ColumnModel("Observed Val.", vars.Select(x => x.ObsVal).ToList()),
            new ColumnModel("Calculated Val.", vars.Select(x => x.CalcVals.ToStringInline()).ToList())
        };
        
        var tablemodel = new TableModel("x-eos Variables", columns);
        tablemodel.IsReadOnly = true;
        return tablemodel;
    }

    public TableModel GetMctcVariablesTableModel()
    {
        var vars = mctcBundle.mctcVariables;
        var columns = new List<ColumnModel>()
        {
            new ColumnModel("ID", vars.Select(x => x.Id.ToString()).ToList()) {IsVisible = false},
            new ColumnModel("#", Enumerable.Range(1, vars.Count).ToList()),
            new ColumnModel("Name", vars.Select(x => x.Name).ToList()),
            new ColumnModel("Starting Val.", vars.Select(x => x.StartingValue).ToList()),
            new ColumnModel("Range", vars.Select(x => x.Range).ToList()),
            new ColumnModel("All Values", vars.Select(x => x.Values.ToStringInline()).ToList()),
        };
        
        var tableModel = new TableModel("mctc Variables", columns);
        tableModel.IsReadOnly = true;
        return tableModel;
    }
}
