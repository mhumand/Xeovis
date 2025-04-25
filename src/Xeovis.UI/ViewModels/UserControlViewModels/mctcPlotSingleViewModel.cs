using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Xeovis.Base;
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

        public mctcPlotSingleViewModel()
        {
            mctcPlotDto = new mctcPlotDto();
            mctcBundle = new mctcBundle();
            mctcRunIndex = 0;
            PhaseFilters = new Dictionary<PhaseType, FilterType>();
        }

        public List<ListableItem> GetXeosVarItems()
        {
            var list = new List<ListableItem>();
            foreach (var sampleVar in mctcPlotDto.SampleXeosVariables)
                list.Add(new ListableItem(sampleVar));

            return list;
        }

        public List<IGrouping<object, xeosVariable>> GetGroupedFilteredXeosVariables(xeosVariable sampleVar)
        {
            // Filtering
            var xeosVariables = mctcBundle.xeosVariables.Where(x => x.VariableType == sampleVar.VariableType && x.PhaseType == sampleVar.PhaseType).ToList();

            var hasIncludesAny = PhaseFilters.Any(x => x.Value == FilterType.IncludesAny);
            var hasIncludesAll = PhaseFilters.Any(x => x.Value == FilterType.IncludesAll);
            var hasExcludesAny = PhaseFilters.Any(x => x.Value == FilterType.ExcludesAny);
            var hasExcludesAll = PhaseFilters.Any(x => x.Value == FilterType.ExcludesAll);

            var excludedVars = new List<xeosVariable>();
            if (hasIncludesAny)
            {
                var phases = PhaseFilters.Where(x => x.Value == FilterType.IncludesAny).Select(x => x.Key).ToList();
                foreach (var xeosVar in xeosVariables)
                {
                    var batch = mctcBundle.Batches.FirstOrDefault(b => b.Id == xeosVar.BatchId);
                    if (batch == null) continue;
                    if (batch.Phases.All(x => !phases.Contains(x)))
                        excludedVars.Add(xeosVar);
                }
            }

            if (hasIncludesAll)
            {
                var phases = PhaseFilters.Where(x => x.Value == FilterType.IncludesAll).Select(x => x.Key).ToList();
                foreach (var xeosVar in xeosVariables)
                {
                    var batch = mctcBundle.Batches.FirstOrDefault(b => b.Id == xeosVar.BatchId);
                    if (batch == null) continue;
                    if (phases.Except(batch.Phases).Any())
                        excludedVars.Add(xeosVar);
                }
            }

            if (hasExcludesAny)
            {
                var phases = PhaseFilters.Where(x => x.Value == FilterType.ExcludesAny).Select(x => x.Key).ToList();
                foreach (var xeosVar in xeosVariables)
                {
                    var batch = mctcBundle.Batches.FirstOrDefault(b => b.Id == xeosVar.BatchId);
                    if (batch == null) continue;
                    if (batch.Phases.Any(x => phases.Contains(x)))
                        excludedVars.Add(xeosVar);
                }
            }

            if (hasExcludesAll)
            {
                var phases = PhaseFilters.Where(x => x.Value == FilterType.ExcludesAll).Select(x => x.Key).ToList();
                foreach (var xeosVar in xeosVariables)
                {
                    var batch = mctcBundle.Batches.FirstOrDefault(b => b.Id == xeosVar.BatchId);
                    if (batch == null) continue;
                    if (phases.All(x => batch.Phases.Contains(x)))
                        excludedVars.Add(xeosVar);
                }
            }

            xeosVariables = xeosVariables.Except(excludedVars).ToList();

            // Grouping
            var allGroups = new List<IGrouping<object, xeosVariable>>();

            if (mctcPlotDto.PointsGroupingType == mctcPlotPointsGroupingType.Batch)
            {
                allGroups = xeosVariables.GroupBy(mv => (object)mctcBundle.Batches.FirstOrDefault(b => b.Id == mv.BatchId).GetPhaseAssemblageString()).ToList();
            }
            else if (mctcPlotDto.PointsGroupingType == mctcPlotPointsGroupingType.Temperature)
            {
                allGroups = xeosVariables.GroupBy(mv => (object)mctcBundle.Batches.FirstOrDefault(b => b.Id == mv.BatchId).ExpRun.Temperature).ToList();
            }
            else if (mctcPlotDto.PointsGroupingType == mctcPlotPointsGroupingType.Pressure)
            {
                allGroups = xeosVariables.GroupBy(mv => (object)mctcBundle.Batches.FirstOrDefault(b => b.Id == mv.BatchId).ExpRun.Pressure).ToList();
            }

            var selectedGroups = new List<IGrouping<object, xeosVariable>>();
            if (mctcPlotDto.ExcludeIrrelevantBatches)
            {
                if (mctcPlotDto.PointsGroupingType == mctcPlotPointsGroupingType.Batch)
                {
                    foreach (var xeosVarGroup in allGroups)
                    {
                        var sampleVarInGroup = xeosVarGroup.FirstOrDefault();
                        var phases = mctcBundle.Batches.FirstOrDefault(b => b.Id == sampleVarInGroup.BatchId).Phases.Except(new List<PhaseType>() { sampleVarInGroup.PhaseType });
                        var varType = sampleVarInGroup.VariableType;

                        var phaseAllCations = new List<CationType>();
                        foreach (var phase in phases)
                            phaseAllCations.AddRange(DefaultPhaseCompositions.GetCationsFromPhaseComposition(phase));

                        var phaseCations = phaseAllCations.Distinct().ToList();
                        var xeosVarCations = DefaultModelDefinitions.GetCationsFromModelVariable(sampleVarInGroup.PhaseType, sampleVarInGroup.VariableType);

                        if (phaseCations.Count != 0 && xeosVarCations.Count != 0)
                            if (!phaseCations.Any(c => xeosVarCations.Contains(c)))
                                continue;

                        selectedGroups.Add(xeosVarGroup);
                    }
                }
                else
                {
                    foreach (var xeosVarGroup in allGroups)
                    {
                        var excludedVariables = new List<xeosVariable>();
                        foreach (var modelVar in xeosVarGroup)
                        {
                            var phases = modelVar.Batch.Phases.Except(new List<PhaseType>() { modelVar.PhaseType });
                            var varType = modelVar.VariableType;

                            var phaseAllCations = new List<CationType>();
                            foreach (var phase in phases)
                                phaseAllCations.AddRange(DefaultPhaseCompositions.GetCationsFromPhaseComposition(phase));

                            var phaseCations = phaseAllCations.Distinct().ToList();
                            var xeosVarCations = DefaultModelDefinitions.GetCationsFromModelVariable(modelVar.PhaseType, modelVar.VariableType);

                            if (phaseCations.Count != 0 && xeosVarCations.Count != 0)
                                if (!phaseCations.Any(c => xeosVarCations.Contains(c)))
                                    excludedVariables.Add(modelVar);
                        }

                        var leftXeosVars = xeosVarGroup.Except(excludedVariables).ToList();
                        if (mctcPlotDto.PointsGroupingType == mctcPlotPointsGroupingType.Temperature)
                        {
                            var grp = leftXeosVars.GroupBy(x => (object)x.Batch.ExpRun.Temperature).FirstOrDefault();
                            if (grp != null && grp.Count() != 0)
                                selectedGroups.Add(grp);
                        }
                        else if (mctcPlotDto.PointsGroupingType == mctcPlotPointsGroupingType.Pressure)
                        {
                            var grp = leftXeosVars.GroupBy(x => (object)x.Batch.ExpRun.Pressure).FirstOrDefault();
                            if (grp != null && grp.Count() != 0)
                                selectedGroups.Add(grp);
                        }

                    }
                }
            }
            else
                selectedGroups = allGroups;

            if (mctcPlotDto.PointsGroupingType != mctcPlotPointsGroupingType.Batch)
                selectedGroups = selectedGroups.OrderBy(g => g.Key).ToList();

            return selectedGroups;
        }

        public ChartModel CreateChartModel(List<IGrouping<object, xeosVariable>> xeosVarGroups, xeosVariable sampleVar)
        {
            var chartModel = new ChartModel();
            chartModel.HasEqualDimensions = true;
            chartModel.Title = sampleVar.ToString();
            chartModel.XAxis.Label = $"Observed {sampleVar}";
            chartModel.YAxis.Label = $"Calculated {sampleVar}";
            chartModel.Legend.IsVisible = false;

            int colorIndex = 0;
            int markerIndex = 0;
            int counter = 0;
            foreach (var modelVarGroup in xeosVarGroups)
            {
                var curve = new CurveModel();
                curve.Name = modelVarGroup.Key.ToString();

                curve.Xvals = modelVarGroup.Where(mv => mv.CalcVals.ContainsKey(mctcRunIndex)).Select(mv => mv.ObsVal).ToList();
                curve.Yvals = modelVarGroup.Where(mv => mv.CalcVals.ContainsKey(mctcRunIndex)).Select(mv => mv.CalcVals[mctcRunIndex]).ToList();
                curve.IsLineVisible = false;

                if (curve.Xvals.Count == 0 || curve.Yvals.Count == 0) continue;

                if (mctcPlotDto.PointsGroupingType == mctcPlotPointsGroupingType.Batch)
                {
                    ChartUtilities.SetColorAndMarker(curve, counter);
                }
                else
                {
                    curve.MarkerColor = ChartUtilities.GetColorInGradient(counter, xeosVarGroups.Count);
                    curve.MarkerType = MarkerType.FilledSquare;
                }

                curve.MarkerSize = 8;
                chartModel.Curves.Add(curve);
                counter++;
            }

            /// x = y line
            var xyCurve = new CurveModel();
            double minVal = chartModel.Curves.Count == 0 ? 0 : Math.Min(chartModel.Curves.Where(c => c.Xvals.Count > 0).Min(c => c.Xvals.Min()), chartModel.Curves.Where(c => c.Yvals.Count > 0).Min(c => c.Yvals.Min()));
            double maxVal = chartModel.Curves.Count == 0 ? 1 : Math.Max(chartModel.Curves.Where(c => c.Xvals.Count > 0).Max(c => c.Xvals.Max()), chartModel.Curves.Where(c => c.Yvals.Count > 0).Max(c => c.Yvals.Max()));
            double difference = maxVal - minVal;
            minVal -= difference * 0.05;
            maxVal += difference * 0.05;
            xyCurve.Xvals = new List<double>() { minVal, maxVal };
            xyCurve.Yvals = new List<double>() { minVal, maxVal };
            xyCurve.IsMarkerVisible = false;
            xyCurve.LineWidth = 1;
            xyCurve.LineColor = Color.DarkRed;
            xyCurve.CurveType = CurveType.Line;
            xyCurve.IsVisibleInLegend = false;
            chartModel.Curves.Insert(0, xyCurve);

            chartModel.XAxis.MinLimit = minVal;
            chartModel.XAxis.MaxLimit = maxVal;
            chartModel.YAxis.MinLimit = minVal;
            chartModel.YAxis.MaxLimit = maxVal;

            return chartModel;
        }

        public TableModel CreateTableModel(List<IGrouping<object, xeosVariable>> xeosVarGroups, xeosVariable sampleVar)
        {
            var batches = mctcBundle.Batches;
            var xeosVars = mctcBundle.xeosVariables.Where(x => xeosVarGroups.Any(g => g.Contains(x)) && x.CalcVals.ContainsKey(mctcRunIndex)).ToList();
            var columns = new List<ColumnModel>()
            {
                new ColumnModel("#", Enumerable.Range(1, xeosVars.Count).Select(x => x.ToString("000")).ToList()),
                new ColumnModel("Obs", xeosVars.Select(x => x.ObsVal).ToList()),
                new ColumnModel("Calc", xeosVars.Select(x => x.CalcVals[mctcRunIndex]).ToList()),
                new ColumnModel("Assemblage", xeosVars.Select(x => batches.FirstOrDefault(b => b.Id == x.BatchId).GetPhaseAssemblageString()).ToList()),
                new ColumnModel("Batch", xeosVars.Select(x => batches.FirstOrDefault(b => b.Id == x.BatchId).ToString()).ToList()),
                new ColumnModel("P [kbar]", xeosVars.Select(x => batches.FirstOrDefault(b => b.Id == x.BatchId).ExpRunPressure).ToList()),
                new ColumnModel("T [°C]", xeosVars.Select(x => batches.FirstOrDefault(b => b.Id == x.BatchId).ExpRunTemperature).ToList()),

            };

            var tableModel = new TableModel(sampleVar.ToString(), columns);
            tableModel.IsReadOnly = true;
            tableModel.IsRowSelect = true;
            tableModel.IsMultiSelect = false;
            return tableModel;
        }

        public TableModel CreateMctcRunsTableModel()
        {
            var columns = new List<ColumnModel>();
            foreach (var mctcVar in mctcBundle.mctcVariables)
                columns.Add(new ColumnModel(mctcVar.Name, new List<double>() { mctcVar.Values[mctcRunIndex] }));

            var tableModel = new TableModel("mctc Runs", columns);
            tableModel.IsReadOnly = true;
            return tableModel;
        }

        public TableModel CreateFiltersTableModel()
        {
            var phases = mctcBundle.Batches.SelectMany(x => x.Phases).ToList().Distinct();
            var filters = new List<string>();
            foreach (var item in Enum.GetValues(typeof(FilterType)).Cast<FilterType>().ToList())
                filters.Add(EnumExtensions.GetDescriptionFromValue(item));

            PhaseFilters.Clear();
            foreach (var phase in phases)
                PhaseFilters.Add(phase, EnumExtensions.GetValueFromDescription<FilterType>(filters[0]));

            var columns = new List<ColumnModel>()
            {
                new ColumnModel("Filter", Enumerable.Range(1, phases.Count()).ToList().Select(x => filters[0]).ToList())
                {
                    Options = filters.Cast<object>().ToList()
                },
                new ColumnModel("Phase", phases.Select(x => x.ToString()).ToList()) {IsReadOnly = true},
            };

            var tableModel = new TableModel("Filter", columns);
            tableModel.CanSortColumns = false;

            return tableModel;
        }
    }
}
