using System.Collections.Generic;
using System.Drawing;
using Xeovis.Domain;
using Xeovis.Domain.Definitions.Experiment;
using Xeovis.Presentation;
using Xeovis.UI.Models;
using Xeovis.UI.Models.Enums;

namespace Xeovis.UI.ViewModels
{
    public partial class mctcPlotGridViewModel : TabControlViewModelBase
    {
        public mctcPlotDto mctcPlotDto { get; set; }
        public mctcBundle mctcBundle { get; set; }
        public int mctcRunIndex { get; set; }
        public Dictionary<PhaseNameSet, FilterType> PhaseFilters { get; set; }

        private Dictionary<string, MarkerType> _inUseMarkers { get; set; }
        private Dictionary<string, Color> _inUseColors { get; set; }

        public mctcPlotGridViewModel()
        {
            mctcPlotDto = new mctcPlotDto();
            mctcBundle = new mctcBundle();
            PhaseFilters = new Dictionary<PhaseNameSet, FilterType>();
            _inUseColors = new Dictionary<string, Color>();
            _inUseMarkers = new Dictionary<string, MarkerType>();
        }

        public ChartModel CreateChartModel(mctcPlotAxesSettings axesSettings)
        {
            var groupedVars = mctcPlotViewModelUtilities.GetGroupedFilteredXeosVariables(mctcBundle, axesSettings, PhaseFilters, mctcPlotDto);
            if (groupedVars == null || groupedVars.Count > 0) return null;
            var chartModel = mctcPlotViewModelUtilities.CreateChartModel(mctcBundle, mctcPlotDto, groupedVars, axesSettings, mctcRunIndex, _inUseMarkers, _inUseColors);
            if (chartModel == null) return null;
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

        public List<ListableItem> GetXeosVarItems()
        {
            var list = new List<ListableItem>();
            foreach (var sampleVar in mctcPlotDto.SampleXeosVariables)
                list.Add(new ListableItem(sampleVar));

            list.Insert(0, new ListableItem("Self"));
            return list;
        }
    }
}
