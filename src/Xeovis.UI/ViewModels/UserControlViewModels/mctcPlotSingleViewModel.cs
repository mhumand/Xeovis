using System;
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
        public mctcPlotDto mctcPlotDto;
        public mctcBundle mctcBundle;

        public mctcPlotSingleViewModel()
        {
            mctcPlotDto = new mctcPlotDto();
            mctcBundle = new mctcBundle();
        }

        public List<ListableItem> GetXeosVarItems()
        {
            var list = new List<ListableItem>();
            foreach (var sampleVar in mctcPlotDto.SampleXeosVariables)
            {
                list.Add(new ListableItem(sampleVar));
            }

            return list;
        }

        public List<IGrouping<object, xeosVariable>> GroupXeosVariables(List<xeosVariable> xeosVariables)
        {
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
                        var sampleVar = xeosVarGroup.FirstOrDefault();
                        var phases = mctcBundle.Batches.FirstOrDefault(b => b.Id == sampleVar.BatchId).Phases.Except(new List<PhaseType>() { sampleVar.PhaseType });
                        var varType = sampleVar.VariableType;

                        var phaseAllCations = new List<CationType>();
                        foreach (var phase in phases)
                            phaseAllCations.AddRange(DefaultPhaseCompositions.GetCationsFromPhaseComposition(phase));

                        var phaseCations = phaseAllCations.Distinct().ToList();
                        var xeosVarCations = DefaultModelDefinitions.GetCationsFromModelVariable(sampleVar.PhaseType, sampleVar.VariableType);

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

        public ChartModel CreateChartModel(List<IGrouping<object, xeosVariable>> xeosVarGroups, xeosVariable sampleVar, int mctcRunIndex)
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

                //curve.MarkerSize = 10;
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
            chartModel.Curves.Add(xyCurve);

            chartModel.XAxis.MinLimit = minVal;
            chartModel.XAxis.MaxLimit = maxVal;
            chartModel.YAxis.MinLimit = minVal;
            chartModel.YAxis.MaxLimit = maxVal;

            return chartModel;
        }

        public TableModel CreateTableModel(List<IGrouping<object, xeosVariable>> xeosVarGroups, xeosVariable sampleVar, int mctcRunIndex)
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

        public TableModel CreateMctcRunsTableModel(int mctcRunIndex)
        {
            var columns = new List<ColumnModel>();
            foreach (var mctcVar in mctcBundle.mctcVariables)
                columns.Add(new ColumnModel(mctcVar.Name, new List<double>() { mctcVar.Values[mctcRunIndex] }));

            var tableModel = new TableModel("mctc Runs", columns);
            tableModel.IsReadOnly = true;
            return tableModel;
        }
    }
}
