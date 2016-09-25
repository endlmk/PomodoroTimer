using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Interactivity;
using Prism.Common;
using Prism.Interactivity.InteractionRequest;
using System.Windows;
using Hardcodet.Wpf.TaskbarNotification;

namespace PomodoroTimer.Interaction
{
    public class ShowBalloonWindowAction : TriggerAction<FrameworkElement>
    {

        protected override void Invoke(object parameter)
        {
            var args = parameter as InteractionRequestedEventArgs;
            if(args == null)
            {
                return;
            }

            var tb =  this.AssociatedObject as TaskbarIcon;
            if(tb == null)
            {
                return;
            }

            tb.ShowBalloonTip(args.Context.Title, (string)args.Context.Content, BalloonIcon.Info);
        }
    }
}
