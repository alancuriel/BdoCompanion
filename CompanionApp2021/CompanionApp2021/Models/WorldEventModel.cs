using CompanionApp2021.Core.Models;
using Microsoft.Toolkit.Mvvm.ComponentModel;

namespace CompanionApp2021.Models
{
    public class WorldEventModel : ObservableObject
    {

        public WorldEventModel(bool enabled)
        {
            _isNotificationEnabled = enabled;
        }

        public WorldEvent WorldEvent { get; set; }

        private bool _isNotificationEnabled;
        public bool IsNotificationEnabled
        {
            get
            {
                return _isNotificationEnabled;
            }
            set
            {

                _isNotificationEnabled = value;
            }
        }

        public void NotifyNotificationChanged()
        {
            OnPropertyChanged("IsNotificationEnabled");
        }

        private string _timeTillNextSpawn;
        public string TimeTillNextSpawn
        {
            get
            {
                return _timeTillNextSpawn;
            }
            set
            {
                SetProperty(ref _timeTillNextSpawn, value);
            }
        }

   
    }
}
