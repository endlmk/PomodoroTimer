using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Practices.Unity;
using Prism.Mvvm;
using System.Windows;
using PomodoroTimer.Views;
using System.Reactive.Concurrency;
using PomodoroTimer.Models;
using PomodoroTimer.ViewModels;

namespace PomodoroTimer
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private IUnityContainer Container { get; } = new UnityContainer();
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            Container.RegisterType<IScheduler, DispatcherScheduler>(new InjectionConstructor(App.Current.Dispatcher));
            Container.RegisterType<PomodoroTimerModel>(new InjectionConstructor(Container.Resolve<IScheduler>(), TimeSpan.FromSeconds(5), TimeSpan.FromSeconds(2)));
            Container.RegisterType<MainWindowViewModel>(new InjectionConstructor(Container.Resolve <PomodoroTimerModel>()));
            ViewModelLocationProvider.SetDefaultViewModelFactory(x => this.Container.Resolve(x));
            this.Container.Resolve<MainWindow>().Show();
        }
    }
}
