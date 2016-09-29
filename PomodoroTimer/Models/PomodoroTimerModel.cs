using Prism.Mvvm;
using Reactive.Bindings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Concurrency;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Practices.Unity;
using System.Reactive.Subjects;
using System.Reactive.Linq;
using System.IO;
using Newtonsoft.Json;
using System.Collections.ObjectModel;
using Reactive.Bindings.Extensions;
using Reactive.Bindings.Helpers;

namespace PomodoroTimer.Models
{
    public class PomodoroTimerModel : BindableBase
    {
        ReactiveTimer _timer;

        private TimeSpan _timerSpan = TimeSpan.Zero;
        private TimeSpan _pomodoroSpan = TimeSpan.Zero;
        private TimeSpan _restSpan = TimeSpan.Zero;
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

        private JobState _jobState;
        public JobState JobState
        {
            get { return _jobState; }
            set { SetProperty(ref _jobState, value); }
        }

        private Subject<string> _modelSubject = new Subject<string>();

        public IObservable<string> TimerFinished
        {
            get { return this._modelSubject.AsObservable(); }
        }

        public void NotifyTimerFinished(string message)
        {
            this._modelSubject.OnNext(message);
        }

        private int _pomodoroCount = 0;
        public int PomodoroCount
        {
            get { return _pomodoroCount; }
            set { SetProperty(ref _pomodoroCount, value); }
        }

        public PomodoroTimerModel(IScheduler scheduler, IPomodoroCounter counter,  TimeSpan pomodoroSpan, TimeSpan restSpan)
        {
            _timer = new ReactiveTimer(TimeSpan.FromSeconds(1), scheduler);
            _timer.Subscribe((count)=> {
                RemainingTime = _timerSpan - TimeSpan.FromSeconds(count);
                if(RemainingTime == TimeSpan.Zero)
                {
                    _timer.Stop();
                    _timer.Reset();
                    string message = string.Empty;
                    if (JobState == JobState.Pomodoro)
                    {
                        JobState = JobState.Rest;
                        message = "ポモドーロ終了！";
                        counter.Save(DateTime.Now);
                        PomodoroCount = counter.Count(DateTime.Now.Date);
                    }
                    else if(JobState == JobState.Rest)
                    {
                        JobState = JobState.Pomodoro;
                        message = "休憩終了！";
                    }

                    ResetTimer();
                    NotifyTimerFinished(message);
                }
            });

            _pomodoroSpan = pomodoroSpan;
            _restSpan = restSpan;

            _jobState = JobState.Pomodoro;
            ResetTimer();

            PomodoroCount = counter.Count(DateTime.Now.Date);
        }

        private void ResetTimer()
        {
            TimerState = TimerState.Ready;
            if (JobState == JobState.Pomodoro)
            {
                _timerSpan = _pomodoroSpan;
            }
            else if (JobState == JobState.Rest)
            {
                _timerSpan = _restSpan;
            }
            RemainingTime = _timerSpan;
        }

        public void Start()
        {
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
            
            JobState = JobState.Pomodoro;
            ResetTimer();
        }
    }
}
