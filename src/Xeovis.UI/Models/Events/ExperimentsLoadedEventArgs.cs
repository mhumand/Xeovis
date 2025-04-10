using System;
using System.Collections.Generic;
using Xeovis.Domain;
using Xeovis.UI.ViewModels.DialogsViewModels;

namespace Xeovis.UI.Models.Events
{
    public class ExperimentsLoadedEventArgs : EventArgs
    {
        public List<Experiment> Experiments { get; set; }
        public DataManagerLoadDataViewModel LoadDataViewModel { get; set; }

        public ExperimentsLoadedEventArgs(List<Experiment> experiments, DataManagerLoadDataViewModel loadDataViewModel = null)
        {
            Experiments = experiments;
            LoadDataViewModel = loadDataViewModel;
        }
    }
}
