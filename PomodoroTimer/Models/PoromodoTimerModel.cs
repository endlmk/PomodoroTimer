using Prism.Mvvm;
using Reactive.Bindings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Concurrency;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Practices.Unity;

namespace PomodoroTimer.Models
{
    public class PoromodoTimerModel : BindableBase
    {
        ReactiveTimer _timer;

        private TimeSpan _settingTime = TimeSpan.Zero;
        private TimeSpan _remainingTime = TimeSpan.Zero;
        public TimeSpan RemainingTime
        {
            get { return _remainingTime; }
            set { SetProperty(ref _remainingTime, value); }
        }

        private TimerState _timerState;
        public TimerState State
        {
            get { return _timerState; }
            set { SetProperty(ref _timerState, value); }
        }

        public PoromodoTimerModel([Dependency] IScheduler scheduler)
        {
            _timer = new ReactiveTimer(TimeSpan.FromSeconds(1), scheduler);
            _timer.Subscribe((count)=> {
                RemainingTime = _settingTime - TimeSpan.FromSeconds(count);
                if(RemainingTime == TimeSpan.Zero)
                {
                    _timer.Stop();
                    _timer.Reset();
                    State = TimerState.Stopped;
                }
            });

            _timerState = TimerState.Stopped;
        }

        public void Start()
        {
            _timer.Start();
            State = TimerState.Running;
        }

        public void Pause()
        {
            _timer.Stop();
            State = TimerState.Pausing;
        }

        public void SetSettingTime(TimeSpan span)
        {
            _settingTime = span;
            RemainingTime = _settingTime;
        }
    }
}
