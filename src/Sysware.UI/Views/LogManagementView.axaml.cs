using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Sysware.UI.ViewModels;

namespace Sysware.UI.Views;

public partial class LogManagementView : UserControl
{
    public LogManagementView()
    {
        InitializeComponent();
    }

    public LogManagementView(LogManagementViewModel vm)
    {
        InitializeComponent();
        DataContext = vm;
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}


