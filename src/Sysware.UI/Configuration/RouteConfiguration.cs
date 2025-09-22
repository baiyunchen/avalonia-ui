using System;
using Microsoft.Extensions.DependencyInjection;
using Sysware.UI.Services;
using Sysware.UI.ViewModels;
using Sysware.UI.Views;

namespace Sysware.UI.Configuration;

/// <summary>
/// 路由配置类，统一管理所有路由注册
/// </summary>
public static class RouteConfiguration
{
    /// <summary>
    /// 配置所有应用程序路由
    /// </summary>
    /// <param name="navigationService">导航服务实例</param>
    /// <param name="serviceProvider">服务提供者</param>
    public static void ConfigureRoutes(INavigationService navigationService, IServiceProvider serviceProvider)
    {
        // 日志管理路由
        navigationService.Register("LogManagement", () =>
        {
            var vm = serviceProvider.GetRequiredService<LogManagementViewModel>();
            return new LogManagementView(vm);
        }, title: "日志管理");

        // 在这里添加更多路由注册
        // 示例：
        // navigationService.Register("UserManagement", () =>
        // {
        //     var vm = serviceProvider.GetRequiredService<UserManagementViewModel>();
        //     return new UserManagementView(vm);
        // }, title: "用户管理");

        // navigationService.Register("SystemSettings", () =>
        // {
        //     var vm = serviceProvider.GetRequiredService<SystemSettingsViewModel>();
        //     return new SystemSettingsView(vm);
        // }, title: "系统设置");

        // navigationService.Register("PermissionManagement", () =>
        // {
        //     var vm = serviceProvider.GetRequiredService<PermissionManagementViewModel>();
        //     return new PermissionManagementView(vm);
        // }, title: "权限管理");
    }
}
