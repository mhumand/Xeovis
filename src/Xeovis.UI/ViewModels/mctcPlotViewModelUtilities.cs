using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Xeovis.Base;
using Xeovis.Domain;
using Xeovis.Domain.Definitions.Experiment;
using Xeovis.Domain.Definitions.Globals;
using Xeovis.Presentation;
using Xeovis.UI.Models.Enums;

namespace Xeovis.UI.ViewModels
{
    public static class mctcPlotViewModelUtilities
    {
        public static List<IGrouping<object, xeosVariable>> GetGroupedFilteredXeosVariables(mctcBundle mctcBundle, 
            mctcPlotAxesSettings axesSettings, Dictionary<PhaseNameSet, FilterType> PhaseFilters, mctcPlotDto mctcPlotDto)
        {
            var yVar = axesSettings.YAxisVariable;
            var xVar = axesSettings.XAxisVariable;

            if (yVar.PhaseNameSet == null || xVar.PhaseNameSet == null)
                return null;

            var batchDict = mctcBundle.Batches.ToDictionary(b => b.Id, b => b);
            
            // Filtering
            var xeosVariables = mctcBundle.xeosVariables.Where(x => x.IsEqualTo(yVar)).ToHashSet();
            
            var hasIncludesAny = PhaseFilters.Any(x => x.Value == FilterType.IncludesAny);
            var hasIncludesAll = PhaseFilters.Any(x => x.Value == FilterType.IncludesAll);
            var hasExcludesAny = PhaseFilters.Any(x => x.Value == FilterType.ExcludesAny);
            var hasExcludesAll = PhaseFilters.Any(x => x.Value == FilterType.ExcludesAll);

            var excludedVars = new List<xeosVariable>();
            if (hasIncludesAny)
            {
                var phases = PhaseFilters.Where(x => x.Value == FilterType.IncludesAny)
                    .Select(x => x.Key).ToHashSet();
                foreach (var xeosVar in xeosVariables)
                {
                    // var batch = mctcBundle.Batches.FirstOrDefault(b => b.Id == xeosVar.BatchId);
                    // if (batch == null) continue;
                    if (!batchDict.TryGetValue(xeosVar.BatchId, out var batch)) continue;
                    
                    if (batch.PhaseNameSets.All(x => !phases.Contains(x)))
                        excludedVars.Add(xeosVar);
                }
            }

            if (hasIncludesAll)
            {
                var phases = PhaseFilters.Where(x => x.Value == FilterType.IncludesAll)
                    .Select(x => x.Key).ToHashSet();
                foreach (var xeosVar in xeosVariables)
                {
                    // var batch = mctcBundle.Batches.FirstOrDefault(b => b.Id == xeosVar.BatchId);
                    // if (batch == null) continue;
                    if (!batchDict.TryGetValue(xeosVar.BatchId, out var batch)) continue;

                    if (phases.Except(batch.PhaseNameSets).Any())
                        excludedVars.Add(xeosVar);
                }
            }

            if (hasExcludesAny)
            {
                var phases = PhaseFilters.Where(x => x.Value == FilterType.ExcludesAny)
                    .Select(x => x.Key).ToHashSet();
                foreach (var xeosVar in xeosVariables)
                {
                    // var batch = mctcBundle.Batches.FirstOrDefault(b => b.Id == xeosVar.BatchId);
                    // if (batch == null) continue;
                    if (!batchDict.TryGetValue(xeosVar.BatchId, out var batch)) continue;

                    if (batch.PhaseNameSets.Any(x => phases.Contains(x)))
                        excludedVars.Add(xeosVar);
                }
            }

            if (hasExcludesAll)
            {
                var phases = PhaseFilters.Where(x => x.Value == FilterType.ExcludesAll)
                    .Select(x => x.Key).ToHashSet();
                foreach (var xeosVar in xeosVariables)
                {
                    // var batch = mctcBundle.Batches.FirstOrDefault(b => b.Id == xeosVar.BatchId);
                    // if (batch == null) continue;
                    if (!batchDict.TryGetValue(xeosVar.BatchId, out var batch)) continue;

                    if (phases.All(x => batch.PhaseNameSets.Contains(x)))
                        excludedVars.Add(xeosVar);
                }
            }

            xeosVariables = xeosVariables.Except(excludedVars).ToHashSet();

            // Grouping
            var allGroups = new List<IGrouping<object, xeosVariable>>();

            if (mctcPlotDto.PointsGroupingType == mctcPlotPointsGroupingType.Batch)
            {
                allGroups = xeosVariables.GroupBy(mv => (object)mctcBundle.Batches.FirstOrDefault(b => b.Id == mv.BatchId).GetPhaseAssemblageString()).ToList();
            }
            else if (mctcPlotDto.PointsGroupingType == mctcPlotPointsGroupingType.Temperature)
            {
                allGroups = xeosVariables.GroupBy(mv => (object)mctcBundle.Batches.FirstOrDefault(b => b.Id == mv.BatchId).ExpRunTemperature).ToList();
            }
            else if (mctcPlotDto.PointsGroupingType == mctcPlotPointsGroupingType.Pressure)
            {
                allGroups = xeosVariables.GroupBy(mv => (object)mctcBundle.Batches.FirstOrDefault(b => b.Id == mv.BatchId).ExpRunPressure).ToList();
            }
            else if (mctcPlotDto.PointsGroupingType == mctcPlotPointsGroupingType.Experiment)
            {
                allGroups = xeosVariables.GroupBy(mv => (object)mctcBundle.Batches.FirstOrDefault(b => b.Id == mv.BatchId).ExperimentName).ToList();
            }

            var selectedGroups = new List<IGrouping<object, xeosVariable>>();
            if (mctcPlotDto.ExcludeIrrelevantBatches)
            {
                if (mctcPlotDto.PointsGroupingType == mctcPlotPointsGroupingType.Batch)
                {
                    foreach (var xeosVarGroup in allGroups)
                    {
                        var sampleVarInGroup = xeosVarGroup.FirstOrDefault();
                        var batch = mctcBundle.Batches.FirstOrDefault(b => b.Id == sampleVarInGroup.BatchId);
                        if (batch == null) continue;
                        var phases = batch.PhaseNameSets.Except(batch.PhaseNameSets.Where(x => x != null && x.HasName(sampleVarInGroup.PhaseKeyName))).ToList();
                        phases.RemoveAll(x => x == null);

                        var phaseAllCations = new List<CationType>();
                        phases.ForEach(x => phaseAllCations.AddRange(PhaseOxideList.GetPhaseCations(x.Id)));

                        var phaseCations = phaseAllCations.Distinct().ToList();
                        var res = xeosVariableCationList.CationList.TryGetValue(sampleVarInGroup.NameId, out List<CationType>? sampleVarCations);
                        if (!res || sampleVarCations == null) continue;
                        if (!sampleVarInGroup.IsEqualTo(xVar) && axesSettings.XAxisType == mctcPlotAxisType.Observed)
                        {
                            if (xeosVariableCationList.CationList.TryGetValue(xVar.NameId, out List<CationType>? xVarCations) && xVarCations != null)
                                sampleVarCations = sampleVarCations.Union(xVarCations).Distinct().ToList();
                        }

                        if (phaseCations.Count != 0 && sampleVarCations.Count != 0)
                            if (!phaseCations.Any(c => sampleVarCations.Contains(c))) continue;

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
                            var batch = mctcBundle.Batches.FirstOrDefault(b => b.Id == modelVar.BatchId);
                            if (batch == null) continue;
                            var phases = batch.PhaseNameSets.Except(
                                batch.PhaseNameSets.Where(x => x != null && x.HasName(modelVar.PhaseKeyName))).ToList();
                            phases.RemoveAll(x => x == null);

                            var phaseAllCations = new List<CationType>();
                            phases.ForEach(x => phaseAllCations.AddRange(PhaseOxideList.GetPhaseCations(x.Id)));

                            var phaseCations = phaseAllCations.Distinct().ToList();
                            var res = xeosVariableCationList.CationList.TryGetValue(modelVar.NameId, out List<CationType>? modelVarCations);
                            if (!res || modelVarCations == null) continue;
                            if (!modelVar.IsEqualTo(xVar) && axesSettings.XAxisType == mctcPlotAxisType.Observed)
                            {
                                if (xeosVariableCationList.CationList.TryGetValue(xVar.NameId, out List<CationType>? xVarCations) && xVarCations != null)
                                    modelVarCations = modelVarCations.Union(xVarCations).Distinct().ToList();
                            }

                            if (phaseCations.Count != 0 && modelVarCations.Count != 0)
                                if (!phaseCations.Any(c => modelVarCations.Contains(c)))
                                    excludedVariables.Add(modelVar);
                        }

                        var leftXeosVars = xeosVarGroup.Except(excludedVariables).ToList();
                        if (mctcPlotDto.PointsGroupingType == mctcPlotPointsGroupingType.Temperature)
                        {
                            var grp = leftXeosVars.GroupBy(x => (object)mctcBundle.Batches.FirstOrDefault(b => b.Id == x.BatchId).ExpRunTemperature).FirstOrDefault();
                            if (grp != null && grp.Count() != 0) selectedGroups.Add(grp);
                        }
                        else if (mctcPlotDto.PointsGroupingType == mctcPlotPointsGroupingType.Pressure)
                        {
                            var grp = leftXeosVars.GroupBy(x => (object)mctcBundle.Batches.FirstOrDefault(b => b.Id == x.BatchId).ExpRunPressure).FirstOrDefault();
                            if (grp != null && grp.Count() != 0) selectedGroups.Add(grp);
                        }
                        else if (mctcPlotDto.PointsGroupingType == mctcPlotPointsGroupingType.Experiment)
                        {
                            var grp = leftXeosVars.GroupBy(x => (object)mctcBundle.Batches.FirstOrDefault(b => b.Id == x.BatchId).ExperimentName).FirstOrDefault();
                            if (grp != null && grp.Count() != 0) selectedGroups.Add(grp);
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

        private static void SetChartAxesLabel(ChartModel chartModel, mctcPlotAxesSettings axesSettings,
            xeosVariable xVariable, xeosVariable yVariable)
        {
            chartModel.XAxis.Label = axesSettings.XAxisType switch
            {
                mctcPlotAxisType.Calculated => $"Calculated {xVariable}",
                mctcPlotAxisType.Observed => $"Observed {xVariable}",
                mctcPlotAxisType.Residual => $"Residual {xVariable}",
                mctcPlotAxisType.Temperature => "Temperature [°C]",
                mctcPlotAxisType.Pressure => "Pressure [kbar]",
                _ => throw new Exception("Axis Type Not Supported")
            };

            chartModel.YAxis.Label = axesSettings.YAxisType switch
            {
                mctcPlotAxisType.Calculated => $"Calculated {yVariable}",
                mctcPlotAxisType.Observed => $"Observed {yVariable}",
                mctcPlotAxisType.Residual => $"Residual {yVariable}",
                mctcPlotAxisType.Temperature => "Temperature [°C]",
                mctcPlotAxisType.Pressure => "Pressure [kbar]",
                _ => throw new Exception("Axis Type Not Supported")
            };
        }

        private static void SetChartAxesLimits(ChartModel chartModel, mctcPlotDto mctcPlotDto, mctcPlotAxesSettings axesSettings)
        {
            if (chartModel.Curves.Count <= 0) return;
            double minXVal, maxXVal, minYVal, maxYVal;
            if (mctcPlotDto.ChartSettings.UseAxesLimits)
            {
                minXVal = mctcPlotDto.ChartSettings.AxesMinLimit;
                maxXVal = mctcPlotDto.ChartSettings.AxesMaxLimit;
                minYVal = mctcPlotDto.ChartSettings.AxesMinLimit;
                maxYVal = mctcPlotDto.ChartSettings.AxesMaxLimit;
            }
            else
            {
                minXVal = chartModel.Curves.Where(c => c.Xvals.Count > 0).Min(c => c.Xvals.Min());
                maxXVal = chartModel.Curves.Where(c => c.Xvals.Count > 0).Max(c => c.Xvals.Max());
                minYVal = chartModel.Curves.Where(c => c.Yvals.Count > 0).Min(c => c.Yvals.Min());
                maxYVal = chartModel.Curves.Where(c => c.Yvals.Count > 0).Max(c => c.Yvals.Max());

                double differenceX = maxXVal - minXVal;
                double differenceY = maxYVal - minYVal;
                minXVal -= differenceX * 0.05;
                maxXVal += differenceX * 0.05;
                minYVal -= differenceY * 0.05;
                maxYVal += differenceY * 0.05;
            }

            if (axesSettings.XAxisType == mctcPlotAxisType.Observed
                || axesSettings.YAxisType == mctcPlotAxisType.Residual)
            {
                var xyCurve = new CurveModel();
                if (axesSettings.YAxisType == mctcPlotAxisType.Calculated)
                {
                    var minVal = Math.Min(minXVal, minYVal);
                    var maxVal = Math.Max(maxXVal, maxYVal);
                    xyCurve.Name = "1:1 Line";
                    xyCurve.Xvals = [minVal, maxVal];
                    xyCurve.Yvals = [minVal, maxVal];
                }
                else if (axesSettings.YAxisType == mctcPlotAxisType.Residual)
                {
                    xyCurve.Name = "Y = 0 Line";
                    xyCurve.Xvals = [minXVal, maxXVal];
                    xyCurve.Yvals = [0, 0];
                }

                xyCurve.IsMarkerVisible = false;
                xyCurve.LineWidth = 1;
                xyCurve.LineColor = Color.DarkRed;
                xyCurve.CurveType = CurveType.Line;
                xyCurve.IsVisibleInLegend = false;
                chartModel.Curves.Insert(0, xyCurve);
            }

            chartModel.XAxis.MinLimit = minXVal;
            chartModel.XAxis.MaxLimit = maxXVal;
            chartModel.YAxis.MinLimit = minYVal;
            chartModel.YAxis.MaxLimit = maxYVal;
        }

        private static void SetChartTitle(ChartModel chartModel, List<double> allXvals, List<double> allYvals)
        {
            if (allXvals.Count <= 0 || allXvals.Count != allYvals.Count) return;
            var average = allXvals.Zip(allYvals,
                (obs, calc) => obs != 0 ? Math.Abs(1 - calc / obs) * 100.0
                    : Math.Abs(1 - obs / calc) * 100.0).Where(x => !double.IsNaN(x)).Average();
            chartModel.Title += $" ({average.Round(2)}%)";
        }
        
        public static ChartModel CreateChartModel(mctcBundle mctcBundle, mctcPlotDto mctcPlotDto, 
            List<IGrouping<object, xeosVariable>> xeosVarGroups, mctcPlotAxesSettings axesSettings, 
            int mctcRunIndex, Dictionary<string, MarkerType> inUseMarkers, Dictionary<string, Color> inUseColors)
        {
            var yVariable = axesSettings.YAxisVariable;
            var xVariable = axesSettings.XAxisVariable;

            var chartModel = new ChartModel();
            chartModel.HasEqualDimensions = true;
            chartModel.Title = yVariable.ToString();
            SetChartAxesLabel(chartModel, axesSettings, xVariable, yVariable);

            chartModel.Legend.IsVisible = false;
            chartModel.YAxis.IsLabelVisible = mctcPlotDto.ChartSettings.ShowAxesTitle;
            chartModel.XAxis.IsLabelVisible = mctcPlotDto.ChartSettings.ShowAxesTitle;
            chartModel.IsTitleVisible = mctcPlotDto.ChartSettings.ShowChartTitle;

            int counter = inUseMarkers == null || inUseColors == null ? 0 : Math.Max(inUseColors.Count, inUseMarkers.Count);
            if (mctcPlotDto.PointsGroupingType.IsNumber())
                counter = 0;//todo: temp

            var allXvals = new List<double>();
            var allYvals = new List<double>();
            foreach (var modelVarGroup in xeosVarGroups)
            {
                // modelVarGroup contains yVar
                var yVarsValid = modelVarGroup.Where(mv => mv.CalcVals.ContainsKey(mctcRunIndex));

                var xValues = new List<double>();
                var yValues = new List<double>();

                foreach (var yVar in yVarsValid)
                {
                    double x = double.NaN;
                    double y = double.NaN;

                    var batch = mctcBundle.Batches.FirstOrDefault(b => b.Id == yVar.BatchId);
                    if (batch == null) continue;
                    var xAxisVariable = batch.xeosVariables.FirstOrDefault(x => x.IsEqualTo(xVariable));
                    if (axesSettings.XAxisType == mctcPlotAxisType.Observed && xAxisVariable != null)
                        x = xAxisVariable.ObsVal;
                    else if (axesSettings.XAxisType == mctcPlotAxisType.Temperature)
                        x = batch.ExpRunTemperature;
                    else if (axesSettings.XAxisType == mctcPlotAxisType.Pressure)
                        x = batch.ExpRunPressure;
                    else
                        continue;

                    if (axesSettings.YAxisType == mctcPlotAxisType.Calculated)
                        y = yVar.CalcVals[mctcRunIndex];
                    else if (axesSettings.YAxisType == mctcPlotAxisType.Residual)
                        y = (yVar.CalcVals[mctcRunIndex] - yVar.ObsVal).Round(10);
                    else
                        continue;

                    if (!double.IsNaN(x) && !double.IsNaN(y))
                    {
                        xValues.Add(x);
                        yValues.Add(y);
                    }
                }

                if (xValues.Count == 0 || yValues.Count == 0) continue;

                if (axesSettings.XAxisType is mctcPlotAxisType.Observed && axesSettings.YAxisType is mctcPlotAxisType.Calculated)
                {
                    allXvals.AddRange(xValues);
                    allYvals.AddRange(yValues);
                }

                var curve = new CurveModel();
                curve.Name = modelVarGroup.Key.ToString();
                curve.Xvals = xValues;
                curve.Yvals = yValues;
                curve.IsLineVisible = false;

                if (!mctcPlotDto.PointsGroupingType.IsNumber())
                {
                    if (inUseColors.ContainsKey(curve.Name) && inUseMarkers.ContainsKey(curve.Name))
                    {
                        curve.MarkerColor = inUseColors[curve.Name];
                        curve.MarkerType = inUseMarkers[curve.Name];
                    }
                    else
                    {
                        ChartUtilities.SetColorAndMarker(curve, counter);
                        counter++;
                    }
                }
                else
                {
                    var lowColor = ColorTranslator.FromHtml("#1F77B4");
                    var highColor = ColorTranslator.FromHtml("#D62728");
                    curve.MarkerColor = ChartUtilities.GetColorInGradient(lowColor, highColor, counter, xeosVarGroups.Count);
                    curve.MarkerType = MarkerType.FilledCircle;
                    counter++;
                }

                curve.MarkerSize = 8;
                chartModel.Curves.Add(curve);
            }

            SetChartTitle(chartModel, allXvals, allYvals);
            SetChartAxesLimits(chartModel, mctcPlotDto, axesSettings);

            return chartModel;
        }

        public static TableModel CreateMctcRunsTableModel(mctcBundle mctcBundle, int mctcRunIndex)
        {
            var columns = new List<ColumnModel>();
            foreach (var mctcVar in mctcBundle.mctcVariables)
                columns.Add(new ColumnModel(mctcVar.Name, new List<double>() { mctcVar.Values[mctcRunIndex] }));

            var tableModel = new TableModel("mctc Runs", columns);
            tableModel.IsReadOnly = true;
            return tableModel;
        }

        public static TableModel CreateFiltersTableModel(mctcBundle mctcBundle, Dictionary<PhaseNameSet, FilterType> PhaseFilters)
        {
            var phases = mctcBundle.Batches.SelectMany(x => x.PhaseNameSets).ToList().Distinct().ToList();
            phases.RemoveAll(x => x == null);
            var filters = new List<string>();
            foreach (var item in Enum.GetValues(typeof(FilterType)).Cast<FilterType>().ToList())
                filters.Add(EnumExtensions.GetDescriptionFromValue(item));

            PhaseFilters.Clear();
            foreach (var phase in phases.OrderBy(x => x.KeyName))
                PhaseFilters.Add(phase, EnumExtensions.GetValueFromDescription<FilterType>(filters[0]));

            var columns = new List<ColumnModel>()
            {
                new ColumnModel("Filter", Enumerable.Range(1, PhaseFilters.Count()).ToList().Select(x => filters[0]).ToList())
                {
                    Options = filters.Cast<object>().ToList()
                },
                new ColumnModel("Phase", PhaseFilters.Keys.Select(x => x.KeyName).ToList()) {IsReadOnly = true},
            };

            var tableModel = new TableModel("Filter", columns);
            tableModel.CanSortColumns = false;

            return tableModel;
        }

        public static TableModel CreateTableModel(mctcBundle mctcBundle, List<IGrouping<object, xeosVariable>> xeosVarGroups, mctcPlotAxesSettings axesSettings, int mctcRunIndex)
        {
            var yVariable = axesSettings.YAxisVariable;
            var xVariable = axesSettings.XAxisVariable;

            var xeosVars = mctcBundle.xeosVariables.Where(x => xeosVarGroups.Any(g => g.Contains(x)) && x.CalcVals.ContainsKey(mctcRunIndex)).ToList();

            var xValues = new List<double>();
            var yValues = new List<double>();
            var assemblages = new List<string>();
            var batches = new List<string>();
            var temperatures = new List<double>();
            var pressures = new List<double>();
            foreach (var yVar in xeosVars)
            {
                double x = double.NaN;
                double y = double.NaN;

                var batch = mctcBundle.Batches.FirstOrDefault(b => b.Id == yVar.BatchId);
                if (batch == null) continue;
                var xAxisVariable = batch.xeosVariables.FirstOrDefault(x => x.IsEqualTo(xVariable));
                if (axesSettings.XAxisType == mctcPlotAxisType.Observed && xAxisVariable != null)
                    x = xAxisVariable.ObsVal;
                else if (axesSettings.XAxisType == mctcPlotAxisType.Temperature)
                    x = batch.ExpRunTemperature;
                else if (axesSettings.XAxisType == mctcPlotAxisType.Pressure)
                    x = batch.ExpRunPressure;
                else
                    continue;

                if (axesSettings.YAxisType == mctcPlotAxisType.Calculated)
                    y = yVar.CalcVals[mctcRunIndex];
                else if (axesSettings.YAxisType == mctcPlotAxisType.Residual)
                    y = (yVar.CalcVals[mctcRunIndex] - yVar.ObsVal).Round(10);
                else
                    continue;

                if (double.IsNaN(x) || double.IsNaN(y)) continue;
                xValues.Add(x);
                yValues.Add(y);

                assemblages.Add(batch.GetPhaseAssemblageString());
                batches.Add(batch.ToString());
                temperatures.Add(batch.ExpRunTemperature);
                pressures.Add(batch.ExpRunPressure);
            }

            var columns = new List<ColumnModel>()
            {
                new ColumnModel("#", Enumerable.Range(1, xValues.Count).ToList()),
                new ColumnModel("X", xValues),
                new ColumnModel("Y", yValues),
                new ColumnModel("Assemblage", assemblages),
                new ColumnModel("Batch", batches),
                new ColumnModel("P [kbar]", pressures),
                new ColumnModel("T [°C]", temperatures),
            };

            var tableModel = new TableModel(yVariable.ToString(), columns);
            tableModel.IsReadOnly = true;
            tableModel.IsRowSelect = true;
            tableModel.IsMultiSelect = false;
            return tableModel;
        }
    }
}
