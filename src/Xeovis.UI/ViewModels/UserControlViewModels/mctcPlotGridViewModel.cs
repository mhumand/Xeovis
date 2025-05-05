using System;
using System.Collections.Generic;
using System.Drawing;
using Xeovis.Domain;
using Xeovis.UI.Models;
using Xeovis.UI.Models.Enums;

namespace Xeovis.UI.ViewModels
{
    public partial class mctcPlotGridViewModel : TabControlViewModelBase
    {
        public mctcPlotDto mctcPlotDto { get; set; }
        public mctcBundle mctcBundle { get; set; }
        public int mctcRunIndex { get; set; }
        public Dictionary<PhaseType, FilterType> PhaseFilters { get; set; }

        private Dictionary<string, MarkerType> _inUseMarkers { get; set; }
        private Dictionary<string, Color> _inUseColors { get; set; }

        public mctcPlotGridViewModel()
        {
            mctcPlotDto = new mctcPlotDto();
            mctcBundle = new mctcBundle();
            PhaseFilters = new Dictionary<PhaseType, FilterType>();
            _inUseColors = new Dictionary<string, Color>();
            _inUseMarkers = new Dictionary<string, MarkerType>();
        }

        public ChartModel CreateChartModel(xeosVariable sampleVariable)
        {
            var groupedVars = mctcPlotViewModelUtilities.GetGroupedFilteredXeosVariables(mctcBundle, sampleVariable, PhaseFilters, mctcPlotDto);
            var chartModel = mctcPlotViewModelUtilities.CreateChartModel(mctcPlotDto, groupedVars, sampleVariable, mctcRunIndex, _inUseMarkers, _inUseColors);

            foreach (var curve in chartModel.Curves)
            {
                if (!_inUseMarkers.ContainsKey(curve.Name))
                    _inUseMarkers.Add(curve.Name, curve.MarkerType);

                if (!_inUseColors.ContainsKey(curve.Name))
                    _inUseColors.Add(curve.Name, curve.MarkerColor);
            }

            return chartModel;
        }

        public TableModel CreateMctcRunsTableModel()
        {
            return mctcPlotViewModelUtilities.CreateMctcRunsTableModel(mctcBundle, mctcRunIndex);
        }

        public TableModel CreateFiltersTableModel()
        {
            return mctcPlotViewModelUtilities.CreateFiltersTableModel(mctcBundle, PhaseFilters);
        }

        public void ClearInUseMarkersAndColors()
        {
            _inUseColors.Clear();
            _inUseMarkers.Clear();
        }
    }
}
