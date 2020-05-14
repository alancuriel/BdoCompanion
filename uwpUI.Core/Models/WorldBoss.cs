using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace uwpUI.Core.Models
{
    public class WorldBoss : INotifyPropertyChanged
    {
        public string Name { get; set; }
        public string Location { get; set; }
        public string Image { get; set; }
        public List<TimeSpan> SpawnTimes { get; set; }
        public List<DateTime> NextSpawnTime { get; set; }

        private bool _isTimerEnabled;
        public bool IsTimerEnabled
        {
            get
            {
                return _isTimerEnabled;
            }
            set
            {

                this._isTimerEnabled = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsTimerEnabled)));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
