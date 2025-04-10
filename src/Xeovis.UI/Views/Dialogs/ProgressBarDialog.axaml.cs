using Avalonia.Controls;
using System;
using System.Threading;
using Xeovis.UI.Models;

namespace Xeovis.UI;

public partial class ProgressBarDialog : Window
{
    private CancellationTokenSource _cancellationToken;
    public CancellationToken Token => _cancellationToken.Token;
    public IProgress<ProgressBarData> ProgressReporter { get; private set; }

    public ProgressBarDialog()
    {
        InitializeComponent();
        DataContext = this;

        pbPrimary.Minimum = 0;
        pbPrimary.Maximum = 100;
        pbSecondary.Minimum = 0;
        pbSecondary.Maximum = 100;

        pbPrimary.Value = 0;
        txtPrimary.Text = "Loading...";
        pbSecondary.IsVisible = false;
        txtSecondary.IsVisible = false;

        _cancellationToken = new CancellationTokenSource();
        ProgressReporter = new Progress<ProgressBarData>(data =>
        {
            pbPrimary.Value = data.PrimaryValue;
            txtPrimary.Text = data.PrimaryLabel;
            pbSecondary.Value = data.SecondaryValue;
            txtSecondary.Text = data.SecondaryLabel;
            pbSecondary.IsVisible = data.SecondaryValue != double.NaN && !string.IsNullOrWhiteSpace(data.SecondaryLabel);
            txtSecondary.IsVisible = pbSecondary.IsVisible; 
        });
    }

    private void btnCancel_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        _cancellationToken?.Cancel();
    }
}