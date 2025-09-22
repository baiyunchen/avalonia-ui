using System;
using System.Reactive.Linq;
using ReactiveUI;
using Avalonia.Threading;
using Sysware.UI.Services;

namespace Sysware.UI.ViewModels;

public class StatusBarViewModel : ViewModelBase, IDisposable
{
    private readonly IStatusBarManager _manager;
    private readonly IDisposable _timerSubscription;
    private string _currentTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

    public StatusBarViewModel(IStatusBarManager manager)
    {
        _manager = manager;
        _manager.Changed += OnManagerChanged;

        // 定时更新当前时间
        _timerSubscription = Observable.Interval(TimeSpan.FromSeconds(1))
            .ObserveOn(RxApp.MainThreadScheduler)
            .Subscribe(_ => CurrentTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
    }

    public string Text
    {
        get => _manager.Text;
        set
        {
            if (_manager.Text == value) return;
            _manager.Text = value;
            this.RaisePropertyChanged();
        }
    }

    public bool IsBusy
    {
        get => _manager.IsBusy;
        set
        {
            if (_manager.IsBusy == value) return;
            _manager.IsBusy = value;
            this.RaisePropertyChanged();
        }
    }

    public double ProgressValue
    {
        get => _manager.ProgressValue;
        set
        {
            if (Math.Abs(_manager.ProgressValue - value) < 0.0001) return;
            _manager.ProgressValue = value;
            this.RaisePropertyChanged();
        }
    }

    public bool IsOnline
    {
        get => _manager.IsOnline;
        set
        {
            if (_manager.IsOnline == value) return;
            _manager.IsOnline = value;
            this.RaisePropertyChanged();
        }
    }

    public string CurrentTime
    {
        get => _currentTime;
        private set => this.RaiseAndSetIfChanged(ref _currentTime, value);
    }

    private void OnManagerChanged(object? sender, EventArgs e)
    {
        if (Dispatcher.UIThread.CheckAccess())
        {
            this.RaisePropertyChanged(nameof(Text));
            this.RaisePropertyChanged(nameof(IsBusy));
            this.RaisePropertyChanged(nameof(ProgressValue));
            this.RaisePropertyChanged(nameof(IsOnline));
        }
        else
        {
            Dispatcher.UIThread.Post(() =>
            {
                this.RaisePropertyChanged(nameof(Text));
                this.RaisePropertyChanged(nameof(IsBusy));
                this.RaisePropertyChanged(nameof(ProgressValue));
                this.RaisePropertyChanged(nameof(IsOnline));
            });
        }
    }

    public void Dispose()
    {
        _manager.Changed -= OnManagerChanged;
        _timerSubscription.Dispose();
    }
}


