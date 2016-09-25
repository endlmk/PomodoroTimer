using Prism.Mvvm;
using PomodoroTimer.Models;
using Microsoft.Practices.Unity;
using System.Reactive.Concurrency;
using Prism.Commands;
using Reactive.Bindings;
using System;
using Reactive.Bindings.Extensions;
using System.Reactive.Linq;
using System.Windows.Media;
using Prism.Interactivity.InteractionRequest;

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

        public ReadOnlyReactiveProperty<SolidColorBrush> DispColor { get; }

        public InteractionRequest<Notification> NotificationRequest { get; } = new InteractionRequest<Notification>();

        public MainWindowViewModel([Dependency] PoromodoTimerModel timer)
        {
            TimerModel = timer;

            StartCommand = TimerModel
                .ObserveProperty(x => x.TimerState)
                .Select(s => (s == TimerState.Ready) || (s == TimerState.Pausing))
                .ToReactiveCommand();
            StartCommand.Subscribe((x) => { TimerModel.Start(); });

            PauseCommand = TimerModel
                .ObserveProperty(x => x.TimerState)
                .Select(s => s == TimerState.Running)
                .ToReactiveCommand();
            PauseCommand.Subscribe((x) => { TimerModel.Pause(); });

            SkipCommand = TimerModel
                .ObserveProperty(x => x.TimerState)
                .Select(s => (s != TimerState.Ready))
                .ToReactiveCommand();
            SkipCommand.Subscribe(x => TimerModel.Skip());

            RemainingTime = TimerModel
                .ObserveProperty(x => x.RemainingTime)
                .Select(x => x.ToString(@"mm\:ss"))
                .ToReadOnlyReactiveProperty();

            DispColor = TimerModel
                .ObserveProperty(x => x.JobState)
                .Select(x =>
                {
                    if (x == JobState.Pomodoro) { return Brushes.Red; }
                    else { return Brushes.Blue; }
                })
                .ToReadOnlyReactiveProperty();

           TimerModel.TimerFinished.Subscribe((message) =>
           {
               this.NotificationRequest.Raise(new Notification { Title = "Alert", Content = message });
           }
           );
        }

    }
}
