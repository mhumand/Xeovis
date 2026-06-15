using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using Avalonia.Platform.Storage;
using MsBox.Avalonia;
using MsBox.Avalonia.Enums;
using Serilog;
using Xeovis.Base.Utilities;
using Xeovis.Domain;
using Xeovis.Domain.Definitions;
using Xeovis.Domain.Definitions.Globals;
using Xeovis.Persistence;
using Xeovis.Presentation;
using Xeovis.UI.Models;
using Xeovis.UI.Models.Enums;
using Xeovis.UI.Models.Events;
using Xeovis.UI.Models.FileReaders;
using Xeovis.UI.ViewModels.DialogsViewModels;
using Xeovis.UI.Views.Helpers;

namespace Xeovis.UI.Views;

public static class UIServices
{
    public static MainWindow ParentWindow;
    public static string LastFilePath;
    public static event Func<object, ProjectFileLoadedEventArgs, Task>? ProjectLoaded;
    public static event EventHandler<ExperimentsLoadedEventArgs>? ExperimentsLoaded;
    public static event EventHandler<mctcBundleLoadedEventArgs>? mctcBundleLoaded;

    public static async void LoadProjectFile(string path, bool raiseUnsavedChanges = false)
    {
        bool goAhead = true;
        if (raiseUnsavedChanges)
            goAhead = await CheckAndRaiseUnsavedChangesMessage();

        if (!goAhead) return;
        
        // Adding a blur effect
        ParentWindow.Effect = new Avalonia.Media.BlurEffect() { Radius = 15 };
        ParentWindow.Opacity = 0.6;
            
        var progBar = new ProgressBarDialog(isIndeterminate: true);
        progBar.ShowDialog(ParentWindow);
            
        // CRITICAL: Yield to the UI thread so Avalonia actually has time 
        // to paint the complicated blur effect before the heavy data loading starts!
        await Task.Delay(50);
            
        try
        {
            var projectFile = await Task.Run(() => ProjectFileManager.Load(path));
            if (projectFile == null)
            {
                var ms = MessageBoxManager.GetMessageBoxStandard("Error",
                    "Failed to open the project file!", ButtonEnum.Ok, Icon.Error);
                ms.ShowWindowDialogAsync(ParentWindow);
                ParentWindow.Effect = null;
                ParentWindow.Opacity = 1.0;
                return;
            }

            LastFilePath = path;
            // ProjectLoaded?.Invoke(this, new ProjectFileLoadedEventArgs(projectFile, path));
                
            // NEW: Safely await all subscribers of the async event
            if (ProjectLoaded != null)
            {
                var handlers = ProjectLoaded.GetInvocationList();
                foreach (Func<object, ProjectFileLoadedEventArgs, Task> handler in handlers)
                {
                    // This forces LoadProjectFile to WAIT until the UI is completely finished building
                    await handler(ParentWindow, new ProjectFileLoadedEventArgs(projectFile, path)); 
                }
            }
            Log.Information("Project file opened successfully.");
        }
        catch (Exception e)
        {
            Log.Information("Failed to open the project file.");
        }
        finally
        {
            progBar.Close();
                
            // 2. REMOVE THE BLUR: Reset the effect and opacity
            ParentWindow.Effect = null;
            ParentWindow.Opacity = 1.0;
        }
    }

    public static void InvokeProjectLoaded(object sender, ProjectFileLoadedEventArgs e)
    {
        ProjectLoaded?.Invoke(sender, e);
    }

    public static void InvokeExperimentsLoaded(object sender, ExperimentsLoadedEventArgs e)
    {
        ExperimentsLoaded?.Invoke(sender, e);
    }

    public static void InvokeMctcBundleLoaded(object sender, mctcBundleLoadedEventArgs e)
    {
        mctcBundleLoaded?.Invoke(sender, e);
    }
    
    public static async Task<bool> CheckAndRaiseUnsavedChangesMessage(bool askAnyWay = false)
    {
        if ((File.Exists(LastFilePath) || askAnyWay) && ParentWindow != null)
        {
            var mb = MessageBoxManager.GetMessageBoxStandard("Unsaved Changes",
                "You have unsaved changes.\nDo you want to save before continuing?", ButtonEnum.YesNoCancel,
                Icon.Warning);
            var result = await mb.ShowWindowDialogAsync(ParentWindow);
            if (result == ButtonResult.Yes)
                await Save(isSaveAs: false);
            else if (result == ButtonResult.No)
                Log.Information("Project file not saved.");
            else
                return false;
        }

        return true;
    }

    public static async Task Save(bool isSaveAs = false)
    {
        var projectFile = GetProjectFile();
        if (projectFile == null) return;

        string path;
        if (isSaveAs || !File.Exists(LastFilePath))
        {
            var topLevel = TopLevel.GetTopLevel(ParentWindow);
            if (topLevel == null) return;
            var options = new FilePickerSaveOptions()
            {
                Title = "Save",
                ShowOverwritePrompt = true,
                FileTypeChoices = [FileExtentionHelper.GetFilePickerFileType(FileExtensionType.XVS)]
            };

            var files = await topLevel.StorageProvider.SaveFilePickerAsync(options);
            if (files == null) return;
            path = Uri.UnescapeDataString(files.Path.AbsolutePath);
            LastFilePath = path;
        }
        else
            path = LastFilePath;

        projectFile.ProjectName = Path.GetFileNameWithoutExtension(path);
        var result = ProjectFileManager.Save(projectFile, path);
        if (result)
        {
            var extension = Path.GetExtension(path);
            ParentWindow.Title = $"Xeovis | {projectFile.ProjectName}{extension}";
            Log.Information("Project file saved.");
        }
    }
    
    private static ProjectFile GetProjectFile()
    {
        if (ParentWindow == null) return null;
        var projectFile = new ProjectFile();
        projectFile.Experiments = ParentWindow.ViewModel.Experiments;
        projectFile.mctcBundles = ParentWindow.ViewModel.mctcBundles;
        projectFile.mctcPlotDtos = ParentWindow.ViewModel.mctcPlotDtos;
        projectFile.ProjectSettings.AllowedPhaseNames = PhaseNameList.PhaseNames;
        projectFile.ProjectSettings.PhaseXeosVariables = xeosVariableNameList.VariableNames;
        projectFile.ProjectSettings.PhaseOxides = PhaseOxideList.OxideList;
        projectFile.ProjectSettings.xeosVariableCations = xeosVariableCationList.CationList;
        return projectFile;
    }

    public static async Task<mctcBundle> LoadMctcBundle(string folderDirectory)
    {
        if (ParentWindow == null) return null;
        var progressDialog = new ProgressBarDialog();
        progressDialog.ShowDialog(ParentWindow);

        var progress = progressDialog.ProgressReporter;
        var token = progressDialog.Token;

        try
        {
            var mctcReader = new mctcDataReader(folderDirectory);
            var mctcBundle = await Task.Run(() => mctcReader.ReadData(progress, token)) ??
                             throw new NullReferenceException();
            mctcBundle.TimeLoaded = DateTime.Now;
            mctcBundle.FilesDirectory = folderDirectory;
            return mctcBundle;
        }
        catch (Exception ex)
        {
            switch (ex)
            {
                case OperationCanceledException:
                    Log.Information("User cancelled loading mctc bundle.");
                    break;
                case FileNotFoundException:
                    Log.Error("Couldn't read files from {ExMessage}", ex.Message);
                    break;
                case NullReferenceException:
                    Log.Error("Couldn't load mctc bundle.");
                    break;
                default:
                    Log.Error(ex.Message);
                    break;
            }

            return null;
        }
        finally
        {
            progressDialog.Close();
        }
    }
    
    public static mctcBundle SimpleMctcBundleLoad(string folderDirectory)
    {
        var mctcReader = new mctcDataReader(folderDirectory);
        var mctcBundle = mctcReader.ReadData();
        mctcBundle.TimeLoaded = DateTime.Now;
        mctcBundle.FilesDirectory = folderDirectory;
        return mctcBundle;
    }

    public static async void LoadNewPlot(Guid selectedBundleId)
    {
        if (ParentWindow == null) return;
        var hasMctcBundle = ParentWindow.ViewModel.mctcBundles.Count > 0;
        if (!hasMctcBundle)
        {
            var msgBox = MessageBoxManager.GetMessageBoxStandard("Error",
                "No mctc bundle was found! Please load a bundle first.", ButtonEnum.Ok, Icon.Error);
            msgBox.ShowWindowDialogAsync(ParentWindow);
            return;
        }

        mctcNewPlotDialog dialog = GetNewMctcPlotDialog(selectedBundleId);
        var result = await dialog.ShowDialog<DialogResultType>(ParentWindow);

        if (result == DialogResultType.OK)
        {
            dialog.ViewModel.mctcPlotDto.TimeCreated = DateTime.Now;
            ParentWindow.CreateMctcPlot(dialog.ViewModel.mctcPlotDto);
        }
    }
    
    private static mctcNewPlotDialog GetNewMctcPlotDialog(Guid selectedBundleId, mctcPlotDto currentMctcPlot = null)
    {
        var viewModel = new mctcNewPlotViewModel();
        viewModel.mctcBundles = ParentWindow.ViewModel.mctcBundles;
        viewModel.ExistingPlotNames = ParentWindow.ViewModel.MctcBundleNodes
            .SelectMany(b => b.Children.Select(x => x.Label)).ToList();
        viewModel.SelectedBundleId = selectedBundleId;
        viewModel.mctcPlotDto = currentMctcPlot;
        var dialog = new mctcNewPlotDialog() { ViewModel = viewModel };
        return dialog;
    }

    public static async Task OpenMctcPlotDialog(mctcBundle mctcBundle, mctcPlotDto mctcPlotDto, ImctcPlotView tab)
    {
        var dialog = GetNewMctcPlotDialog(mctcBundle.Id, mctcPlotDto);
        var result = await dialog.ShowDialog<DialogResultType>(ParentWindow);
        if (result == DialogResultType.OK)
        {
            ParentWindow?.OnMctcPlotRenamed(mctcPlotDto);
            tab.PopulateUI(mctcPlotDto, mctcBundle);
        }
    }

    public static async Task LoadMctcBundle()
    {
        if (ParentWindow == null) return;
        var viewModel = new mctcLoadDataViewModel();
        viewModel.ExisitingBundleNames = ParentWindow.ViewModel.mctcBundles.Select(x => x.Name).ToList();
        var dialog = new mctcLoadDataDialog() { ViewModel = viewModel };

        var result = await dialog.ShowDialog<DialogResultType>(ParentWindow);

        if (result == DialogResultType.OK)
        {
            var vm = dialog.ViewModel;
            var mctcBundle = await UIServices.LoadMctcBundle(vm.FolderDirectory);
            if (mctcBundle != null)
            {
                mctcBundle.Name = vm.MctcBundleName;
                InvokeMctcBundleLoaded(ParentWindow, new mctcBundleLoadedEventArgs(mctcBundle));
            }
        }
    }

    public static async Task LoadExperiments()
    {
        if (ParentWindow == null) return;
        var viewModel = new DataManagerLoadDataViewModel();
        var dialog = new DataManagerLoadDataDialog();
        dialog.ViewModel = viewModel;
        var result = await dialog.ShowDialog<DialogResultType>(ParentWindow);

        if (result == DialogResultType.OK)
        {
            var progressDialog = new ProgressBarDialog();
            progressDialog.ShowDialog(ParentWindow);

            var progress = progressDialog.ProgressReporter;
            var token = progressDialog.Token;

            try
            {
                var vm = dialog.ViewModel;
                var expReader = new ExperimentReader(vm.SelectedFolders, vm.SubfolderDirectory, vm.IsSubfolderChecked);
                var expReaderResults = await Task.Run(() => expReader.GetExperiments(progress, token));

                // get confirmation

                var experiments = expReaderResults.Experiments;
                UIServices.InvokeExperimentsLoaded(ParentWindow, new ExperimentsLoadedEventArgs(experiments, dialog.ViewModel));
            }
            catch (Exception ex)
            {
                if (ex is OperationCanceledException)
                    Log.Information("User cancelled loading experiments.");
                else if (ex is IOException)
                {
                    Log.Information(ex.Message);
                    var mb = MessageBoxManager.GetMessageBoxStandard("Failed to Access",
                        StringUtilities.WrapTextToLines(ex.Message, 100), ButtonEnum.Ok, Icon.Error);
                    mb.ShowWindowDialogAsync(ParentWindow);
                }
                else
                    Log.Error(ex.Message);
            }
            finally
            {
                progressDialog.Close();
            }
        }
    }

    public static async Task OpenMctcPlotSettings(ImctcPlotView tabView, mctcPlotDto mctcPlotDto, mctcBundle mctcBundle)
    {
        if (ParentWindow == null) return;
        var otherNames = ParentWindow.ViewModel.mctcPlotDtos.Except(new List<mctcPlotDto> { mctcPlotDto }).Select(x => x.Name).ToList();
        var viewModel = new mctcPlotSettingsViewModel(mctcPlotDto, otherNames);
        var dialog = new mctcPlotSettingsDialog();
        dialog.ViewModel = viewModel;
        var result = await dialog.ShowDialog<DialogResultType>(ParentWindow);

        if (result == DialogResultType.OK)
        {
            tabView.PopulateUI(mctcPlotDto, mctcBundle);
            ParentWindow.OnMctcPlotRenamed(mctcPlotDto);
        }
    }

    public static async Task OpenMctcGridExportDialog(mctcPlotDto mctcPlotDto, UniformGrid uniformGrid)
    {
        if (ParentWindow == null) return;

        var dialog = new mctcPlotGridExportDialog(mctcPlotDto);
        var result = await dialog.ShowDialog<DialogResultType>(ParentWindow);

        if (result != DialogResultType.OK) return;

        if (dialog.ExportData || dialog.ExportImages)
        {
            var directory = await GetExportDirectory();
            if (!Directory.Exists(directory)) return;
            if (dialog.ExportImages) ExportImages(dialog, directory, uniformGrid);
            if (dialog.ExportData) ExportData(dialog, directory, uniformGrid);
        }
    }
    
    private static async Task<string> GetExportDirectory()
    {
        var topLevel = TopLevel.GetTopLevel(ParentWindow);
        if (topLevel == null) return string.Empty;
        var options = new FolderPickerOpenOptions()
        {
            Title = "Choose Folder",
            AllowMultiple = false,
        };

        var folders = await topLevel.StorageProvider.OpenFolderPickerAsync(options);
        if (folders.Count > 0)
            return Uri.UnescapeDataString(folders[0].Path.AbsolutePath);
        else
            return string.Empty;
    }
    
    private static void ExportImages(mctcPlotGridExportDialog dialog, string directory, UniformGrid uniformGrid)
    {
        var imageFileExtension = dialog.ImageFileType.GetStringExtention();
        var imageFileName = directory + dialog.ImageFilesName + imageFileExtension;
        var images = new List<Image>();
        foreach (ChartControl chartControl in uniformGrid.Children)
        {
            var image = chartControl.GetChartImage();
            image.Tag = chartControl;
            images.Add(image);
        }

        var sampleImage = images[0].Source as RenderTargetBitmap;
        if (sampleImage == null) return;
        var singleWidth = sampleImage.PixelSize.Width;
        var singleHeight = sampleImage.PixelSize.Height;
        int totalWidth = singleWidth * uniformGrid.Columns;
        int totalHeight = singleHeight * uniformGrid.Rows;
        var combined = new RenderTargetBitmap(new PixelSize(totalWidth, totalHeight), sampleImage.Dpi);
        var scale = 300.0 / 96.0;
        using (var ctx = combined.CreateDrawingContext())
        {
            ctx.FillRectangle(Brushes.White, new Rect(0, 0, totalWidth, totalHeight)); // background

            int counter = 0;
            for (int i = 0; i < uniformGrid.Rows; i++)
            {
                for (int j = 0; j < uniformGrid.Columns; j++)
                {
                    if (counter >= images.Count) break;

                    var image = images[counter];
                    var imageSource = image.Source as RenderTargetBitmap;
                    var width = imageSource.PixelSize.Width;
                    var height = imageSource.PixelSize.Height;
                    var imageRec = new Rect(0, 0, width, height);
                    ctx.DrawImage(imageSource, imageRec, new Rect(width * j / scale, height * i / scale, width / scale, height / scale));

                    if (dialog.IndiviualExport)
                    {
                        var chartControl = image.Tag as ChartControl;
                        imageSource.Save(directory + dialog.ImageFilesName + $" - {chartControl.ViewModel.Title}" + imageFileExtension);
                    }

                    counter++;
                }
            }

            combined.Save(imageFileName);
        }
    }

    private static void ExportData(mctcPlotGridExportDialog dialog, string directory, UniformGrid uniformGrid)
    {
        var dataFileExtension = dialog.DataFileType.GetStringExtention();
        var dataSB = new StringBuilder();
        foreach (ChartControl chartControl in uniformGrid.Children)
        {
            var dataString = chartControl.GetDataString();
            if (string.IsNullOrWhiteSpace(dataString)) continue;
            dataSB.AppendLine(dataString);

            if (dialog.IndiviualExport)
                File.WriteAllText(directory + dialog.DataFilesName + $" - {chartControl.ViewModel.Title}" + dataFileExtension, dataString);
        }

        if (dataSB.Length > 0)
            File.WriteAllText(directory + dialog.DataFilesName + dataFileExtension, dataSB.ToString());
    }
}