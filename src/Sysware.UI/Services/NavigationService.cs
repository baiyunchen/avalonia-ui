using System;
using System.Collections.Generic;
using Avalonia.Threading;

namespace Sysware.UI.Services;

/// <summary>
/// 默认的页面导航服务实现。
/// - 通过路由键与内容工厂方法建立映射
/// - 支持 Push/Replace 导航模式
/// - 维护后退/前进栈
/// - 触发 Navigating/Navigated 事件（在 UI 线程）
/// </summary>
public sealed class NavigationService : INavigationService
{
    /// <summary>
    /// 路由键到页面内容工厂方法的映射。
    /// </summary>
    private readonly Dictionary<string, Func<object>> _routeToFactory = new();

    /// <summary>
    /// 后退栈，保存之前访问过的页面信息。
    /// </summary>
    private readonly Stack<(string RouteKey, object Content, string? Title)> _backStack = new();

    /// <summary>
    /// 前进栈，配合后退使用以支持前进功能。
    /// </summary>
    private readonly Stack<(string RouteKey, object Content, string? Title)> _forwardStack = new();

    /// <summary>
    /// 当前页面信息。
    /// </summary>
    private (string RouteKey, object Content, string? Title)? _current;

    /// <inheritdoc />
    public event EventHandler<NavigationEventArgs>? Navigating;

    /// <inheritdoc />
    public event EventHandler<NavigationEventArgs>? Navigated;

    /// <inheritdoc />
    public void Register(string routeKey, Func<object> contentFactory, string? title = null)
    {
        if (string.IsNullOrWhiteSpace(routeKey)) throw new ArgumentException("routeKey 不能为空", nameof(routeKey));
        _routeToFactory[routeKey] = contentFactory ?? throw new ArgumentNullException(nameof(contentFactory));
        // 注：当前实现未记录 title，仅保留参数以便将来扩展（例如用于显示友好名称）。
    }

    /// <inheritdoc />
    public void Navigate(string routeKey, object? parameter = null, NavigationMode mode = NavigationMode.Push)
    {
        if (!_routeToFactory.TryGetValue(routeKey, out var factory))
        {
            throw new InvalidOperationException($"未注册路由: {routeKey}");
        }

        var content = factory();
        var title = routeKey; // 默认使用路由键作为标题，可在外层 ViewModel 再行决定最终标题

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

    /// <inheritdoc />
    public bool CanGoBack => _backStack.Count > 0;

    /// <inheritdoc />
    public bool CanGoForward => _forwardStack.Count > 0;

    /// <inheritdoc />
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

    /// <inheritdoc />
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


