using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Xeovis.Domain;
using Xeovis.UI.Models;
using Xeovis.UI.Models.Enums;

namespace Xeovis.UI.ViewModels
{
    public partial class mctcPlotSingleViewModel : TabControlViewModelBase
    {
        public mctcPlotDto mctcPlotDto { get; set; }
        public mctcBundle mctcBundle { get; set; }
        public int mctcRunIndex { get; set; }
        public Dictionary<PhaseType, FilterType> PhaseFilters { get; set; }

        private List<IGrouping<object, xeosVariable>> _groupedXeosVariables { get; set; }
        private xeosVariable _sampleVariables { get; set; }
        private Dictionary<string, MarkerType> _inUseMarkers { get; set; }
        private Dictionary<string, Color> _inUseColors { get; set; }

        public mctcPlotSingleViewModel()
        {
            mctcPlotDto = new mctcPlotDto();
            mctcBundle = new mctcBundle();
            mctcRunIndex = 0;
            PhaseFilters = new Dictionary<PhaseType, FilterType>();
            _groupedXeosVariables = new List<IGrouping<object, xeosVariable>>();
            _sampleVariables = new xeosVariable();
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

        public bool GroupAndFilterXeosVariables(xeosVariable sampleVar)
        {
            _sampleVariables = sampleVar;
            _groupedXeosVariables = mctcPlotViewModelUtilities.GetGroupedFilteredXeosVariables(mctcBundle, sampleVar, PhaseFilters, mctcPlotDto);
            return _groupedXeosVariables.Count > 0;
        }

        public ChartModel CreateChartModel()
        {
            var chartModel = mctcPlotViewModelUtilities.CreateChartModel(mctcPlotDto, _groupedXeosVariables, _sampleVariables, mctcRunIndex, _inUseMarkers, _inUseColors);

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
            return mctcPlotViewModelUtilities.CreateTableModel(mctcBundle, _groupedXeosVariables, _sampleVariables, mctcRunIndex);
        }

        public TableModel CreateMctcRunsTableModel()
        {
            return mctcPlotViewModelUtilities.CreateMctcRunsTableModel(mctcBundle, mctcRunIndex);
        }

        public TableModel CreateFiltersTableModel()
        {
            return mctcPlotViewModelUtilities.CreateFiltersTableModel(mctcBundle, PhaseFilters);
        }
    }
}
