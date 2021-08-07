using System.ComponentModel;

namespace uwpUI.Core.Models
{
    public class BossModel : INotifyPropertyChanged
    {
        public string Name { get; set; }
        public string Img { get; set; }
        public string Location { get; set; }


        private bool _show = true;

        public bool Show
        {
            get { return _show; }
            set
            {
                _show = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Show)));
            }
        }


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
