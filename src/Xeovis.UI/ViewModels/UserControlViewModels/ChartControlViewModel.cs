using CommunityToolkit.Mvvm.ComponentModel;
using System;
using Xeovis.Presentation;

namespace Xeovis.UI.ViewModels
{
    public partial class ChartControlViewModel : ViewModelBase
    {
        [ObservableProperty]
        private string title;

        [ObservableProperty]
        private ChartModel chartModel;

        public Guid Id { get; set; }

        public ChartControlViewModel()
        {
            Title = "TITLE";
            Id = Guid.NewGuid();
            chartModel = new ChartModel();
        }

        public ChartControlViewModel(ChartModel chartModel) : this()
        {
            ChartModel = chartModel;
            Title = chartModel.Title;
        }
    }
}
