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
        public TimerState TimerState
        {
            get { return _timerState; }
            set { SetProperty(ref _timerState, value); }
        }

        private PomodoroState _pomodoroState;
        public PomodoroState PomodoroState
        {
            get { return _pomodoroState; }
            set { SetProperty(ref _pomodoroState, value); }
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
                    TimerState = TimerState.Stopped;
                    if (PomodoroState == PomodoroState.PomodoroRunning) { PomodoroState = PomodoroState.RestReady; }
                    else if (PomodoroState == PomodoroState.RestRunning) { PomodoroState = PomodoroState.PomodoroReady; }
                    RemainingTime = _settingTime;
                }
            });

            _pomodoroState = PomodoroState.PomodoroReady;
            _timerState = TimerState.Stopped;
        }

        public void Start()
        {
            if(PomodoroState == PomodoroState.PomodoroReady) { PomodoroState = PomodoroState.PomodoroRunning; }
            else if (PomodoroState == PomodoroState.RestReady) { PomodoroState = PomodoroState.RestRunning; }

            if (TimerState == TimerState.Pausing)
            {
                _timer.Start(TimeSpan.FromSeconds(1));
            }
            else
            {
                _timer.Start();
            }
            TimerState = TimerState.Running;
        }

        public void Pause()
        {
            _timer.Stop();
            TimerState = TimerState.Pausing;
        }

        public void Skip()
        {
            _timer.Stop();
            _timer.Reset();
            if(PomodoroState == PomodoroState.PomodoroRunning) { PomodoroState = PomodoroState.PomodoroReady; }
            else if (PomodoroState == PomodoroState.RestRunning) { PomodoroState = PomodoroState.RestReady; }
            TimerState = TimerState.Stopped;
            RemainingTime = _settingTime;
        }

        public void SetSettingTime(TimeSpan span)
        {
            _settingTime = span;
            RemainingTime = _settingTime;
        }
    }
}
