using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PomodoroTimer.Models
{
    public interface IPomodoroCounter
    {
        int Count(DateTime date);

        void Save(DateTime finishedDateTime);
    }
}
