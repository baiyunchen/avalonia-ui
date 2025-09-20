using System.Collections.Generic;

namespace Sysware.UI.Models;

public class NavigationItem
{
    public string Name { get; set; } = string.Empty;
    public string Icon { get; set; } = string.Empty;
    public string Id { get; set; } = string.Empty;
    public List<NavigationItem> Children { get; set; } = new();
    public bool IsExpanded { get; set; }
}

