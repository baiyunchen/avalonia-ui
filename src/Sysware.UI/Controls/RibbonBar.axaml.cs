using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using System.Collections.Generic;
using System.Linq;

namespace Sysware.UI.Controls
{
    public partial class RibbonBar : UserControl
    {
        private readonly Dictionary<string, ScrollViewer> _tabContents;
        private readonly List<Button> _tabButtons;
        private Button? _activeTab;

        public RibbonBar()
        {
            InitializeComponent();
            
            _tabContents = new Dictionary<string, ScrollViewer>();
            _tabButtons = new List<Button>();
            
            InitializeTabControls();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }

        private void InitializeTabControls()
        {
            // 获取所有Tab按钮
            _tabButtons.Add(this.FindControl<Button>("HomeTab")!);
            _tabButtons.Add(this.FindControl<Button>("ModelingTab")!);
            _tabButtons.Add(this.FindControl<Button>("FunctionsTab")!);
            _tabButtons.Add(this.FindControl<Button>("DesignTab")!);
            _tabButtons.Add(this.FindControl<Button>("TestingTab")!);
            _tabButtons.Add(this.FindControl<Button>("LibraryTab")!);
            _tabButtons.Add(this.FindControl<Button>("ManagementTab")!);
            _tabButtons.Add(this.FindControl<Button>("HelpTab")!);

            // 获取所有内容面板
            _tabContents.Add("Home", this.FindControl<ScrollViewer>("HomeContent")!);
            _tabContents.Add("Modeling", this.FindControl<ScrollViewer>("ModelingContent")!);
            _tabContents.Add("Functions", this.FindControl<ScrollViewer>("FunctionsContent")!);
            _tabContents.Add("Design", this.FindControl<ScrollViewer>("DesignContent")!);
            _tabContents.Add("Testing", this.FindControl<ScrollViewer>("TestingContent")!);
            _tabContents.Add("Library", this.FindControl<ScrollViewer>("LibraryContent")!);
            _tabContents.Add("Management", this.FindControl<ScrollViewer>("ManagementContent")!);
            _tabContents.Add("Help", this.FindControl<ScrollViewer>("HelpContent")!);

            // 设置默认激活的Tab
            _activeTab = this.FindControl<Button>("HomeTab");
        }

        private void TabButton_Click(object? sender, RoutedEventArgs e)
        {
            if (sender is Button clickedButton)
            {
                var tabName = clickedButton.Tag?.ToString();
                if (!string.IsNullOrEmpty(tabName))
                {
                    SwitchToTab(tabName, clickedButton);
                }
            }
        }

        private void SwitchToTab(string tabName, Button tabButton)
        {
            // 更新Tab按钮样式
            foreach (var button in _tabButtons)
            {
                button.Classes.Remove("active");
            }
            tabButton.Classes.Add("active");
            _activeTab = tabButton;

            // 隐藏所有内容面板
            foreach (var content in _tabContents.Values)
            {
                content.IsVisible = false;
            }

            // 显示选中的内容面板
            if (_tabContents.TryGetValue(tabName, out var selectedContent))
            {
                selectedContent.IsVisible = true;
            }
        }

        private void GlobalMenuButton_Click(object? sender, RoutedEventArgs e)
        {
            var popup = this.FindControl<Popup>("GlobalMenuPopup");
            if (popup != null)
            {
                popup.IsOpen = !popup.IsOpen;
            }
        }
    }
}
