using System;

using Caliburn.Micro;

using uwpUI.Helpers;
using Windows.UI.Xaml;

namespace uwpUI.ViewModels
{
    public class MainViewModel : Screen
    {
        public MainViewModel()
        {
        }

        private bool _isDay;

        public bool IsDay
        {
            get { return _isDay; }
            set { _isDay = value; }
        }


        private string _time = string.Empty;

        public string Time
        {
            get { return _time; }
            set
            {
                Set(ref _time, $"Game Time: {value}");
            }
        }


        protected override void OnInitialize()
        {
            base.OnInitialize();
            DispatcherTimer clockTimer = new DispatcherTimer();
            clockTimer.Interval = TimeSpan.FromMilliseconds(100);
            clockTimer.Tick += InitTick;
            clockTimer.Start();
        }

        private void InitTick(object sender, object e)
        {
            OnTick(sender, e);

            var timer = (DispatcherTimer)sender;
            timer.Stop();
            timer.Interval = TimeSpan.FromMilliseconds(4444.444444);
            timer.Tick += OnTick;
            timer.Start();
        }

        private void OnTick(object sender, object e)
        {
            //Code used from SomethingLovely.net
            var utcNow = DateTime.UtcNow;
            var startHr = utcNow - new TimeSpan((int)utcNow.DayOfWeek, utcNow.Hour, utcNow.Minute, utcNow.Second, utcNow.Millisecond);
            var rlDayElapsed = (utcNow - startHr).TotalSeconds;
            var secsIntoGameDay = (rlDayElapsed + 200 * 60 + 20 * 60) % (240 * 60);


            int inGameHour;
            int inGameMinute;

            if(secsIntoGameDay >= 12000)
            {
                var secsIntoGameNight = secsIntoGameDay - 12000;
                var pctOfNightDone = secsIntoGameNight / (40 * 60);
                var gameHour = 9 * pctOfNightDone;
                gameHour = gameHour < 2 ? 22 + gameHour : gameHour - 2;
                var secsUntilNightEnd = 40 * 60 - secsIntoGameNight;

                IsDay = false;
                inGameHour = (int)(gameHour / (1 >> 0));
                inGameMinute = (int)(gameHour % 1 * (60 >> 0));
            }
            else
            {
                var secsIntoGameDaytime = secsIntoGameDay;
                var pctOfDayDone = secsIntoGameDay / (200 * 60);
                var gameHour = 7 + (22 - 7) * pctOfDayDone;
                var secsUntilNightStart = 12000 - secsIntoGameDaytime;

                IsDay = true;
                inGameHour = (int)(gameHour / (1 >> 0));
                inGameMinute = (int)(gameHour % 1 * (60 >> 0));
            }

            var inGameTime = new TimeSpan(inGameHour, inGameMinute,0);
            Time = DateTime.Today.Add(inGameTime).ToString("h:m tt");
        }
    }
}
