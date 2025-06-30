using System.Collections.Generic;
using System.Linq;
using System;
using Xeovis.Base;
using Xeovis.Domain;
using Xeovis.UI.Models.Enums;
using System.Drawing;
using Xeovis.Presentation;

namespace Xeovis.UI.ViewModels
{
    public static class mctcPlotViewModelUtilities
    {
        public static List<IGrouping<object, xeosVariable>> GetGroupedFilteredXeosVariables(mctcBundle mctcBundle, mctcPlotAxesSettings axesSettings, Dictionary<PhaseType, FilterType> PhaseFilters, mctcPlotDto mctcPlotDto)
        {
            var yVar = axesSettings.YAxisVariable;
            var xVar = axesSettings.XAxisVariable;

            // Filtering
            var xeosVariables = mctcBundle.xeosVariables.Where(x => x.IsEqualTo(yVar)).ToList();

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
                allGroups = xeosVariables.GroupBy(mv => (object)mctcBundle.Batches.FirstOrDefault(b => b.Id == mv.BatchId).ExpRunTemperature).ToList();
            }
            else if (mctcPlotDto.PointsGroupingType == mctcPlotPointsGroupingType.Pressure)
            {
                allGroups = xeosVariables.GroupBy(mv => (object)mctcBundle.Batches.FirstOrDefault(b => b.Id == mv.BatchId).ExpRunPressure).ToList();
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
                        var phases = batch.Phases.Except([sampleVarInGroup.PhaseType]).ToList();

                        var phaseAllCations = new List<CationType>();
                        phases.ForEach(x => phaseAllCations.AddRange(DefaultPhaseCompositions.GetCationsFromPhaseComposition(x)));

                        var phaseCations = phaseAllCations.Distinct().ToList();
                        var xeosVarCations = DefaultModelDefinitions.GetCationsFromModelVariable(sampleVarInGroup.PhaseType, sampleVarInGroup.VariableType);
                        if (!sampleVarInGroup.IsEqualTo(xVar) && axesSettings.XAxisType == mctcPlotAxisType.Observed)
                        {
                            xeosVarCations = xeosVarCations.Union(DefaultModelDefinitions.GetCationsFromModelVariable(xVar.PhaseType, xVar.VariableType)).Distinct().ToList();
                        }

                        if (phaseCations.Count != 0 && xeosVarCations.Count != 0)
                            if (!phaseCations.Any(c => xeosVarCations.Contains(c))) continue;

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
                            var phases = batch.Phases.Except([modelVar.PhaseType]).ToList();

                            var phaseAllCations = new List<CationType>();
                            phases.ForEach(x => phaseAllCations.AddRange(DefaultPhaseCompositions.GetCationsFromPhaseComposition(x)));

                            var phaseCations = phaseAllCations.Distinct().ToList();
                            var xeosVarCations = DefaultModelDefinitions.GetCationsFromModelVariable(modelVar.PhaseType, modelVar.VariableType);
                            if (!modelVar.IsEqualTo(xVar) && axesSettings.XAxisType == mctcPlotAxisType.Observed)
                            {
                                xeosVarCations = xeosVarCations.Union(DefaultModelDefinitions.GetCationsFromModelVariable(xVar.PhaseType, xVar.VariableType)).Distinct().ToList();
                            }

                            if (phaseCations.Count != 0 && xeosVarCations.Count != 0)
                                if (!phaseCations.Any(c => xeosVarCations.Contains(c)))
                                    excludedVariables.Add(modelVar);
                        }

                        var leftXeosVars = xeosVarGroup.Except(excludedVariables).ToList();
                        if (mctcPlotDto.PointsGroupingType == mctcPlotPointsGroupingType.Temperature)
                        {
                            var grp = leftXeosVars.GroupBy(x => (object)mctcBundle.Batches.FirstOrDefault(b => b.Id == x.BatchId).ExpRunTemperature).FirstOrDefault();
                            if (grp != null && grp.Count() != 0)
                                selectedGroups.Add(grp);
                        }
                        else if (mctcPlotDto.PointsGroupingType == mctcPlotPointsGroupingType.Pressure)
                        {
                            var grp = leftXeosVars.GroupBy(x => (object)mctcBundle.Batches.FirstOrDefault(b => b.Id == x.BatchId).ExpRunPressure).FirstOrDefault();
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

        public static ChartModel CreateChartModel(mctcBundle mctcBundle, mctcPlotDto mctcPlotDto, List<IGrouping<object, xeosVariable>> xeosVarGroups, mctcPlotAxesSettings axesSettings, int mctcRunIndex, Dictionary<string, MarkerType> inUseMarkers, Dictionary<string, Color> inUseColors)
        {
            var yVariable = axesSettings.YAxisVariable;
            var xVariable = axesSettings.XAxisVariable;

            var chartModel = new ChartModel();
            chartModel.HasEqualDimensions = true;
            chartModel.Title = yVariable.ToString();
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

            chartModel.Legend.IsVisible = false;
            chartModel.YAxis.IsLabelVisible = mctcPlotDto.ChartSettings.ShowAxesTitle;
            chartModel.XAxis.IsLabelVisible = mctcPlotDto.ChartSettings.ShowAxesTitle;
            chartModel.IsTitleVisible = mctcPlotDto.ChartSettings.ShowChartTitle;

            int counter = inUseMarkers == null || inUseColors == null ? 0 : Math.Max(inUseColors.Count, inUseMarkers.Count);
            if (mctcPlotDto.PointsGroupingType != mctcPlotPointsGroupingType.Batch)
                counter = 0;//todo: temp

            foreach (var modelVarGroup in xeosVarGroups)
            {
                var curve = new CurveModel();
                curve.Name = modelVarGroup.Key.ToString();

                // modelVarGroup contains yVar
                var yVarsValid = modelVarGroup.Where(mv => mv.CalcVals.ContainsKey(mctcRunIndex));

                var xValues = new List<double>();
                var yValues = new List<double>();

                foreach (var yVar in yVarsValid)
                {
                    var batch = mctcBundle.Batches.FirstOrDefault(b => b.Id == yVar.BatchId);
                    if (batch == null) continue;
                    var xAxisVariable = batch.xeosVariables.FirstOrDefault(x => x.IsEqualTo(xVariable));
                    if (axesSettings.XAxisType == mctcPlotAxisType.Observed && xAxisVariable != null)
                        xValues.Add(xAxisVariable.ObsVal);
                    else if (axesSettings.XAxisType == mctcPlotAxisType.Temperature)
                        xValues.Add(batch.ExpRunTemperature);
                    else if (axesSettings.XAxisType == mctcPlotAxisType.Pressure)
                        xValues.Add(batch.ExpRunPressure);

                    if (axesSettings.YAxisType == mctcPlotAxisType.Calculated)
                        yValues.Add(yVar.CalcVals[mctcRunIndex]);
                    else if (axesSettings.YAxisType == mctcPlotAxisType.Residual)
                        yValues.Add(yVar.CalcVals[mctcRunIndex] - yVar.ObsVal);
                }

                if (xValues.Count == 0 || yValues.Count == 0) continue;
                curve.Xvals = xValues;
                curve.Yvals = yValues;
                curve.IsLineVisible = false;

                if (mctcPlotDto.PointsGroupingType == mctcPlotPointsGroupingType.Batch)
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

            if (chartModel.Curves.Count > 0)
            {
                var minXVal = chartModel.Curves.Where(c => c.Xvals.Count > 0).Min(c => c.Xvals.Min());
                var maxXVal = chartModel.Curves.Where(c => c.Xvals.Count > 0).Max(c => c.Xvals.Max());
                var minYVal = chartModel.Curves.Where(c => c.Yvals.Count > 0).Min(c => c.Yvals.Min());
                var maxYVal = chartModel.Curves.Where(c => c.Yvals.Count > 0).Max(c => c.Yvals.Max());

                double differenceX = maxXVal - minXVal;
                double differenceY = maxYVal - minYVal;
                minXVal -= differenceX * 0.05;
                maxXVal += differenceX * 0.05;
                minYVal -= differenceY * 0.05;
                maxYVal += differenceY * 0.05;

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

        public static TableModel CreateFiltersTableModel(mctcBundle mctcBundle, Dictionary<PhaseType, FilterType> PhaseFilters)
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

        public static TableModel CreateTableModel(mctcBundle mctcBundle, List<IGrouping<object, xeosVariable>> xeosVarGroups, mctcPlotAxesSettings axesSettings, int mctcRunIndex)
        {
            var yVar = axesSettings.YAxisVariable;
            var xVar = axesSettings.XAxisVariable;

            var batches = mctcBundle.Batches;
            var xeosVars = mctcBundle.xeosVariables.Where(x => xeosVarGroups.Any(g => g.Contains(x)) && x.CalcVals.ContainsKey(mctcRunIndex)).ToList();
            var columns = new List<ColumnModel>()
            {
                new ColumnModel("#", Enumerable.Range(1, xeosVars.Count).ToList()),
                new ColumnModel("X", xeosVars.Select(x => x.ObsVal).ToList()),
                new ColumnModel("Y", xeosVars.Select(x => x.CalcVals[mctcRunIndex]).ToList()),
                new ColumnModel("Assemblage", xeosVars.Select(x => batches.FirstOrDefault(b => b.Id == x.BatchId).GetPhaseAssemblageString()).ToList()),
                new ColumnModel("Batch", xeosVars.Select(x => batches.FirstOrDefault(b => b.Id == x.BatchId).ToString()).ToList()),
                new ColumnModel("P [kbar]", xeosVars.Select(x => batches.FirstOrDefault(b => b.Id == x.BatchId).ExpRunPressure).ToList()),
                new ColumnModel("T [°C]", xeosVars.Select(x => batches.FirstOrDefault(b => b.Id == x.BatchId).ExpRunTemperature).ToList()),

            };

            var tableModel = new TableModel(yVar.ToString(), columns);
            tableModel.IsReadOnly = true;
            tableModel.IsRowSelect = true;
            tableModel.IsMultiSelect = false;
            return tableModel;
        }
    }
}
