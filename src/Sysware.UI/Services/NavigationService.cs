using System;
using System.Collections.Generic;
using Avalonia.Threading;

namespace Sysware.UI.Services;

public sealed class NavigationService : INavigationService
{
    private readonly Dictionary<string, Func<object>> _routeToFactory = new();
    private readonly Stack<(string RouteKey, object Content, string? Title)> _backStack = new();
    private readonly Stack<(string RouteKey, object Content, string? Title)> _forwardStack = new();
    private (string RouteKey, object Content, string? Title)? _current;

    public event EventHandler<NavigationEventArgs>? Navigating;
    public event EventHandler<NavigationEventArgs>? Navigated;

    public void Register(string routeKey, Func<object> contentFactory, string? title = null)
    {
        if (string.IsNullOrWhiteSpace(routeKey)) throw new ArgumentException("routeKey 不能为空", nameof(routeKey));
        _routeToFactory[routeKey] = contentFactory ?? throw new ArgumentNullException(nameof(contentFactory));
    }

    public void Navigate(string routeKey, object? parameter = null, NavigationMode mode = NavigationMode.Push)
    {
        if (!_routeToFactory.TryGetValue(routeKey, out var factory))
        {
            throw new InvalidOperationException($"未注册路由: {routeKey}");
        }

        var content = factory();
        var title = routeKey; // 默认使用 key 作为标题，具体标题由 VM 再决定

        if (mode == NavigationMode.Push && _current.HasValue)
        {
            _backStack.Push(_current.Value);
            _forwardStack.Clear();
        }

        var navigatingArgs = new NavigationEventArgs(routeKey, title, content);
        if (Dispatcher.UIThread.CheckAccess())
        {
            Navigating?.Invoke(this, navigatingArgs);
        }
        else
        {
            Dispatcher.UIThread.Post(() => Navigating?.Invoke(this, navigatingArgs));
        }

        _current = (routeKey, content, title);

        var navigatedArgs = new NavigationEventArgs(routeKey, title, content);
        if (Dispatcher.UIThread.CheckAccess())
        {
            Navigated?.Invoke(this, navigatedArgs);
        }
        else
        {
            Dispatcher.UIThread.Post(() => Navigated?.Invoke(this, navigatedArgs));
        }
    }

    public bool CanGoBack => _backStack.Count > 0;
    public bool CanGoForward => _forwardStack.Count > 0;

    public void GoBack()
    {
        if (!CanGoBack) return;
        if (_current.HasValue)
        {
            _forwardStack.Push(_current.Value);
        }

        var previous = _backStack.Pop();
        var navigatingArgs = new NavigationEventArgs(previous.RouteKey, previous.Title, previous.Content);
        if (Dispatcher.UIThread.CheckAccess())
        {
            Navigating?.Invoke(this, navigatingArgs);
        }
        else
        {
            Dispatcher.UIThread.Post(() => Navigating?.Invoke(this, navigatingArgs));
        }
        _current = previous;
        var navigatedArgs = new NavigationEventArgs(previous.RouteKey, previous.Title, previous.Content);
        if (Dispatcher.UIThread.CheckAccess())
        {
            Navigated?.Invoke(this, navigatedArgs);
        }
        else
        {
            Dispatcher.UIThread.Post(() => Navigated?.Invoke(this, navigatedArgs));
        }
    }

    public void GoForward()
    {
        if (!CanGoForward) return;
        if (_current.HasValue)
        {
            _backStack.Push(_current.Value);
        }

        var next = _forwardStack.Pop();
        var navigatingArgs = new NavigationEventArgs(next.RouteKey, next.Title, next.Content);
        if (Dispatcher.UIThread.CheckAccess())
        {
            Navigating?.Invoke(this, navigatingArgs);
        }
        else
        {
            Dispatcher.UIThread.Post(() => Navigating?.Invoke(this, navigatingArgs));
        }
        _current = next;
        var navigatedArgs = new NavigationEventArgs(next.RouteKey, next.Title, next.Content);
        if (Dispatcher.UIThread.CheckAccess())
        {
            Navigated?.Invoke(this, navigatedArgs);
        }
        else
        {
            Dispatcher.UIThread.Post(() => Navigated?.Invoke(this, navigatedArgs));
        }
    }
}


