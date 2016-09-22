using Prism.Mvvm;
using PomodoroTimer.Models;
using Microsoft.Practices.Unity;
using System.Reactive.Concurrency;
using Prism.Commands;
using Reactive.Bindings;
using System;
using Reactive.Bindings.Extensions;
using System.Reactive.Linq;

namespace PomodoroTimer.ViewModels
{
    public class MainWindowViewModel : BindableBase
    {
        private string _title = "Prism Unity Application";
        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }
        
        public PoromodoTimerModel TimerModel { private get; set; }

        public ReadOnlyReactiveProperty<String> RemainingTime { get; }

        public ReactiveCommand StartCommand { get; }

        public ReactiveCommand PauseCommand { get; }

        public ReactiveCommand SkipCommand { get; }

        public MainWindowViewModel([Dependency] PoromodoTimerModel timer)
        {
            TimerModel = timer;

            StartCommand = TimerModel
                .ObserveProperty(x => x.TimerState)
                .Select(s => (s == TimerState.Stopped) || (s == TimerState.Pausing))
                .ToReactiveCommand();
            StartCommand.Subscribe((x) => { TimerModel.Start(); });

            PauseCommand = TimerModel
                .ObserveProperty(x => x.TimerState)
                .Select(s => s == TimerState.Running)
                .ToReactiveCommand();
            PauseCommand.Subscribe((x) => { TimerModel.Pause(); });

            SkipCommand = TimerModel
                .ObserveProperty(x => x.PomodoroState)
                .Select(p => (p == PomodoroState.PomodoroRunning) || (p == PomodoroState.RestRunning))
                .ToReactiveCommand();
            SkipCommand.Subscribe(x => TimerModel.Skip());

            TimerModel.SetSettingTime(TimeSpan.FromSeconds(3));
            RemainingTime = TimerModel
                .ObserveProperty(x => x.RemainingTime)
                .Select(x => x.ToString(@"mm\:ss"))
                .ToReadOnlyReactiveProperty();
        }

    }
}
