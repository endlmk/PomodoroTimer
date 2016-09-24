using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PomodoroTimer.Models
{
    public enum TimerState
    {
        Ready,
        Running,
        Pausing,
    };

    public enum JobState
    {
        Pomodoro,
        Rest,
    };
}
