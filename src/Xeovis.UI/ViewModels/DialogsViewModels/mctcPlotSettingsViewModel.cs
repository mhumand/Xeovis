using System;
using System.Collections.Generic;
using System.Linq;
using Xeovis.Base;
using Xeovis.Presentation;
using Xeovis.UI.Models;

namespace Xeovis.UI.ViewModels.DialogsViewModels
{
    public partial class mctcPlotSettingsViewModel : ViewModelBase
    {
        public mctcPlotDto mctcPlotDto { get; set; }

        public List<string> ExisitingPlotNames { get; set; }

        public mctcPlotSettingsViewModel()
        {
            mctcPlotDto = new mctcPlotDto();
            ExisitingPlotNames = new List<string>();
        }

        public mctcPlotSettingsViewModel(mctcPlotDto mctcPlotDto, List<string> existingPlotNames = null) : this()
        {
            this.mctcPlotDto = mctcPlotDto;
            ExisitingPlotNames = existingPlotNames ?? new List<string>();
        }

        public List<ListableItem> GetCategoryTypeListItems()
        {
            var groupingTypes = Enum.GetValues(typeof(mctcPlotPointsGroupingType)).Cast<mctcPlotPointsGroupingType>().ToList();
            var list = new List<ListableItem>();
            foreach (var type in groupingTypes)
                list.Add(new ListableItem(EnumExtensions.GetDescriptionFromValue(type)) { Tag = type });

            return list;
        }
    }
}
