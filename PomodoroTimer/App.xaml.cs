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

            ViewModelLocationProvider.SetDefaultViewModelFactory(x => this.Container.Resolve(x));
            this.Container.Resolve<MainWindow>().Show();
        }
    }
}
