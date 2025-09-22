using System;

namespace Sysware.UI.Services;

/// <summary>
/// 导航模式。
/// </summary>
public enum NavigationMode
{
    /// <summary>
    /// 入栈导航：将当前页面推入后退栈，再导航到新页面。
    /// 适用于常规“打开新页面”的场景，支持后退到上一个页面。
    /// </summary>
    Push,

    /// <summary>
    /// 替换导航：不改变后退栈，直接以新页面替换当前页面。
    /// 适用于不希望生成新历史记录的场景（如列表页到详情页的内部刷新）。
    /// </summary>
    Replace
}

/// <summary>
/// 导航事件参数。
/// </summary>
public sealed class NavigationEventArgs : EventArgs
{
    /// <summary>
    /// 构造导航事件参数。
    /// </summary>
    /// <param name="routeKey">路由键（唯一标识一个页面/视图）。</param>
    /// <param name="title">标题（可选，通常用于显示在 UI 上）。</param>
    /// <param name="content">页面内容对象（通常是一个 View 实例）。</param>
    public NavigationEventArgs(string routeKey, string? title, object? content)
    {
        RouteKey = routeKey;
        Title = title;
        Content = content;
    }

    /// <summary>
    /// 路由键。
    /// </summary>
    public string RouteKey { get; }

    /// <summary>
    /// 页面标题（可为空）。
    /// </summary>
    public string? Title { get; }

    /// <summary>
    /// 页面内容对象（视图）。
    /// </summary>
    public object? Content { get; }
}

/// <summary>
/// 页面导航服务接口，提供路由注册、页面切换以及前进/后退能力。
/// </summary>
public interface INavigationService
{
    /// <summary>
    /// 注册一个路由以及其对应的内容工厂方法。
    /// </summary>
    /// <param name="routeKey">路由键，唯一标识一个页面。</param>
    /// <param name="contentFactory">用于创建页面内容（通常是 View）的工厂方法。</param>
    /// <param name="title">可选标题，用于 UI 展示；当前默认实现未持久化该值，保留作拓展。</param>
    void Register(string routeKey, Func<object> contentFactory, string? title = null);

    /// <summary>
    /// 导航到指定路由。
    /// </summary>
    /// <param name="routeKey">目标路由键。</param>
    /// <param name="parameter">可选参数（当前实现未在内部传递，可通过外部闭包/DI 传入）。</param>
    /// <param name="mode">导航模式，默认 <see cref="NavigationMode.Push"/>。</param>
    void Navigate(string routeKey, object? parameter = null, NavigationMode mode = NavigationMode.Push);

    /// <summary>
    /// 是否可以后退。
    /// </summary>
    bool CanGoBack { get; }

    /// <summary>
    /// 是否可以前进。
    /// </summary>
    bool CanGoForward { get; }

    /// <summary>
    /// 执行后退导航。
    /// </summary>
    void GoBack();

    /// <summary>
    /// 执行前进导航。
    /// </summary>
    void GoForward();

    /// <summary>
    /// 导航开始前触发（即将导航）。
    /// </summary>
    event EventHandler<NavigationEventArgs>? Navigating;

    /// <summary>
    /// 导航完成后触发（已导航）。
    /// </summary>
    event EventHandler<NavigationEventArgs>? Navigated;
}


