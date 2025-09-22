using System;

namespace Sysware.UI.Services;

public interface IStatusBarManager
{
    string Text { get; set; }
    bool IsBusy { get; set; }
    double ProgressValue { get; set; } // 0-100, -1 表示不确定
    bool IsOnline { get; set; }

    event EventHandler? Changed;
}


