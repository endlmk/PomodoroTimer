using Newtonsoft.Json;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PomodoroTimer.Models
{
    class PomodoroCountFile : IPomodoroCounter
    {
        private List<DateTime> _pomodoroStroage;

        public PomodoroCountFile()
        {
            try
            {
                var str = File.ReadAllText("PomodoroStorage.json");
                _pomodoroStroage = JsonConvert.DeserializeObject<List<DateTime>>(str);
            }
            catch
            {
                _pomodoroStroage = new List<DateTime>();
            }
        }

        public int Count(DateTime date)
        {
            return _pomodoroStroage.Count(x => x.Date == date);
        }

        public void Save(DateTime finishedDateTime)
        {
            _pomodoroStroage.Add(finishedDateTime);
            File.WriteAllText("PomodoroStorage.json", JsonConvert.SerializeObject(_pomodoroStroage));
        }
    }
}
