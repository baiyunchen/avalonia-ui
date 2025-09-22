using System;

namespace Sysware.UI.Services;

public enum NavigationMode
{
    Push,
    Replace
}

public sealed class NavigationEventArgs : EventArgs
{
    public NavigationEventArgs(string routeKey, string? title, object? content)
    {
        RouteKey = routeKey;
        Title = title;
        Content = content;
    }

    public string RouteKey { get; }
    public string? Title { get; }
    public object? Content { get; }
}

public interface INavigationService
{
    void Register(string routeKey, Func<object> contentFactory, string? title = null);

    void Navigate(string routeKey, object? parameter = null, NavigationMode mode = NavigationMode.Push);

    bool CanGoBack { get; }
    bool CanGoForward { get; }
    void GoBack();
    void GoForward();

    event EventHandler<NavigationEventArgs>? Navigating;
    event EventHandler<NavigationEventArgs>? Navigated;
}


