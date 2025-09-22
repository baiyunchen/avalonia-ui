using System;

namespace Sysware.UI.Services;

public sealed class StatusBarManager : IStatusBarManager
{
    private string _text = "准备就绪";
    private bool _isBusy;
    private double _progressValue = -1; // 不确定进度
    private bool _isOnline = true;

    public string Text
    {
        get => _text;
        set
        {
            if (_text == value) return;
            _text = value;
            Changed?.Invoke(this, EventArgs.Empty);
        }
    }

    public bool IsBusy
    {
        get => _isBusy;
        set
        {
            if (_isBusy == value) return;
            _isBusy = value;
            Changed?.Invoke(this, EventArgs.Empty);
        }
    }

    public double ProgressValue
    {
        get => _progressValue;
        set
        {
            if (Math.Abs(_progressValue - value) < 0.0001) return;
            _progressValue = value;
            Changed?.Invoke(this, EventArgs.Empty);
        }
    }

    public bool IsOnline
    {
        get => _isOnline;
        set
        {
            if (_isOnline == value) return;
            _isOnline = value;
            Changed?.Invoke(this, EventArgs.Empty);
        }
    }

    public event EventHandler? Changed;
}


