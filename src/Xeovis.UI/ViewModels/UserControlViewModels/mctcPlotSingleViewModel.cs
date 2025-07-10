using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Xeovis.Domain;
using Xeovis.Domain.Definitions.Experiment;
using Xeovis.Presentation;
using Xeovis.UI.Models;
using Xeovis.UI.Models.Enums;

namespace Xeovis.UI.ViewModels
{
    public partial class mctcPlotSingleViewModel : TabControlViewModelBase
    {
        public mctcPlotDto mctcPlotDto { get; set; }
        public mctcBundle mctcBundle { get; set; }
        public int mctcRunIndex { get; set; }
        public Dictionary<PhaseNameSet, FilterType> PhaseFilters { get; set; }

        private List<IGrouping<object, xeosVariable>> _groupedXeosVariables { get; set; }
        private mctcPlotAxesSettings _axesSettings { get; set; }
        private Dictionary<string, MarkerType> _inUseMarkers { get; set; }
        private Dictionary<string, Color> _inUseColors { get; set; }

        public mctcPlotSingleViewModel()
        {
            mctcPlotDto = new mctcPlotDto();
            mctcBundle = new mctcBundle();
            mctcRunIndex = 0;
            PhaseFilters = new Dictionary<PhaseNameSet, FilterType>();
            _groupedXeosVariables = new List<IGrouping<object, xeosVariable>>();
            _axesSettings = new mctcPlotAxesSettings();
            _inUseColors = new Dictionary<string, Color>();
            _inUseMarkers = new Dictionary<string, MarkerType>();
        }

        public List<ListableItem> GetXeosVarItems()
        {
            var list = new List<ListableItem>();
            foreach (var sampleVar in mctcPlotDto.SampleXeosVariables)
                list.Add(new ListableItem(sampleVar));

            return list;
        }

        public bool GroupAndFilterXeosVariables(mctcPlotAxesSettings axesSettings)
        {
            _axesSettings = axesSettings; 
            _groupedXeosVariables = mctcPlotViewModelUtilities.GetGroupedFilteredXeosVariables(mctcBundle, _axesSettings, PhaseFilters, mctcPlotDto);
            return _groupedXeosVariables != null && _groupedXeosVariables.Count > 0;
        }

        public ChartModel CreateChartModel()
        {
            var chartModel = mctcPlotViewModelUtilities.CreateChartModel(mctcBundle, mctcPlotDto, _groupedXeosVariables, _axesSettings, mctcRunIndex, _inUseMarkers, _inUseColors);

            foreach (var curve in chartModel.Curves)
            {
                if (!_inUseMarkers.ContainsKey(curve.Name))
                    _inUseMarkers.Add(curve.Name, curve.MarkerType);

                if (!_inUseColors.ContainsKey(curve.Name))
                    _inUseColors.Add(curve.Name, curve.MarkerColor);
            }

            return chartModel;
        }

        public TableModel CreateTableModel()
        {
            return mctcPlotViewModelUtilities.CreateTableModel(mctcBundle, _groupedXeosVariables, _axesSettings, mctcRunIndex);
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
