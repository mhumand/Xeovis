using Avalonia.Controls;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Xeovis.UI;

public partial class LabelInputControl : UserControl, INotifyPropertyChanged
{
    private string _value;

    public event PropertyChangedEventHandler? PropertyChanged;

    public string Value
    {
        get { return _value; }
        set
        {
            _value = value;
            OnPropertyChanged();
        }
    }

    private string _label;

    public string Label
    {
        get { return _label; }
        set
        {
            _label = value;
            OnPropertyChanged();
        }
    }

    private bool _isReadOnly;

    public bool IsReadOnly
    {
        get { return _isReadOnly; }
        set
        {
            _isReadOnly = value;
            OnPropertyChanged();
        }
    }

    public LabelInputControl()
    {
        InitializeComponent();
        DataContext = this;
        Focusable = true;
        tbInput.KeyDown += tbInput_KeyDown;
    }

    public double GetDoubleValue()
    {
        return Base.DoubleExtensions.GetDoubleValueFromText(Value);
    }

    private void btnClear_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        tbInput.Clear();
        tbInput.Focus();
    }

    private void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    private void tbInput_KeyDown(object? sender, Avalonia.Input.KeyEventArgs e)
    {
        if (e.Key == Avalonia.Input.Key.Escape)
        {
            this.Focus();
        }
    }
}