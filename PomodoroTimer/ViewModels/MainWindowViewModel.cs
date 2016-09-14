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

        public DelegateCommand StartCommand { get; }

        public MainWindowViewModel([Dependency] PoromodoTimerModel timer)
        {
            TimerModel = timer;
            StartCommand = new DelegateCommand(() => TimerModel.Start());
            TimerModel.SetSettingTime(TimeSpan.FromSeconds(3));
            RemainingTime = TimerModel
                .ObserveProperty(x => x.RemainingTime)
                .Select(x => x.ToString(@"mm\:ss"))
                .ToReadOnlyReactiveProperty();
        }

    }
}
