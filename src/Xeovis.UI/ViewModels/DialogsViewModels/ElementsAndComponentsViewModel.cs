using System.Collections.Generic;
using System.Linq;
using Xeovis.Domain.Definitions.Globals;
using Xeovis.Presentation;
using Xeovis.UI.Models;

namespace Xeovis.UI.ViewModels.DialogsViewModels;

public class ElementsAndComponentsViewModel : ViewModelBase
{
    public List<string> Elements { get; set; }
    public Dictionary<string, List<string>> Components { get; set; }
    
    public ElementsAndComponentsViewModel()
    {
        FetchProjectDefaults();
    }

    public void FetchProjectDefaults()
    {
        var elements = Element.GetAll().Except([Element.None]);
        Elements = elements.Select(x => x.Symbol).ToList();
        
        var components = Component.GetAll().Except([Component.None]).ToList();
        Components = components.ToDictionary(
            k => k.Formula, 
            v => v.AssociatedElements.Select(x => x.Symbol).ToList());
    }

    public void FetchXeovisDefaults()
    {
        Elements = Element.GetXeovisDefaults();
        Components = Component.GetXeovisDefaults();
        Elements.Remove("None");
        Components.Remove("None");
    }
    
    public List<ListableItem> GetElementListableItems()
    {
        return Elements.Select(el => new ListableItem(el, tag: el)).ToList();
    }

    public TableModel GetComponentTable()
    {
        var columns = new List<ColumnModel>();

        var compCol = new ColumnModel("Component", Components.Select(x => x.Key).ToList());
        compCol.IsReadOnly = true;
        columns.Add(compCol);
        
        foreach (var el in Elements)
        {
            var elCol = new ColumnModel(el, Components.Select(c => c.Value.Contains(el)).ToList());
            columns.Add(elCol);
        }
        
        return new TableModel("Component", columns)
        {
            FrozenColumnCount = 1,
            IsMultiSelect = false,
            CanSortColumns = false
        };
    }
}