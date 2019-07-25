using System;

using Caliburn.Micro;

using uwpUI.Helpers;
using Windows.UI.Xaml;

namespace uwpUI.ViewModels
{
    public class MainViewModel : Screen
    {
        public int Incrementer { get; set; } = 0;
        public MainViewModel()
        {
        }

        private string _time = "0";

        public string Time
        {
            get { return _time; }
            set
            {
                Set(ref _time, value);
            }
        }


        protected override void OnInitialize()
        {
            base.OnInitialize();
            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += OnTick;
            timer.Start();
        }

        private void OnTick(object sender, object e)
        {
            Incrementer++;
            Time = Incrementer.ToString();
            //NotifyOfPropertyChange(Time);
        }
    }
}
