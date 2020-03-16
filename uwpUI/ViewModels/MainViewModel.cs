using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Caliburn.Micro;
using uwpUI.Core.Models;
using uwpUI.Core.Services;
using uwpUI.Helpers;
using uwpUI.Services;
using Windows.UI.Core;
using Windows.UI.Xaml;

namespace uwpUI.ViewModels
{
    public class MainViewModel : Screen
    {
        public MainViewModel()
        {
        }

        private DispatcherTimer clockTimer;
        private DispatcherTimer countdownTimer;

        private NewsItem _selectedNewsItem;

        public NewsItem SelectedNewsItem
        {
            get { return _selectedNewsItem; }
            set
            {
                Set(ref _selectedNewsItem, value);
            }
        }


        private ObservableCollection<NewsItem> _news;
        public ObservableCollection<NewsItem> News
        {
            get
            {
                return _news;
            }
            set
            {
                Set(ref _news, value);
            }
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

        private string _dailyReset;

        public string DailyReset
        {
            get { return _dailyReset; }
            set
            {
                Set(ref _dailyReset, $"Daily reset in\n{value}");
            }
        }

        private string _imperialReset;

        public string ImperialReset
        {
            get { return _imperialReset; }
            set
            {
                Set(ref _imperialReset, $"Imperial Reset in\n{value}");
            }
        }

        private string _imperialTradeReset;

        public string ImperialTradeReset
        {
            get { return _imperialTradeReset; }
            set
            {
                Set(ref _imperialTradeReset, $"Imperial Trade Reset in\n{value}");
            }
        }

        private string _bSGameReset;

        public string BSGameReset
        {
            get { return _bSGameReset; }
            set
            {
                Set(ref _bSGameReset, $"Black Spirit Adventure Reset in\n{value}");
            }
        }

        protected override void OnDeactivate(bool close)
        {
            base.OnDeactivate(close);

            clockTimer.Stop();
            clockTimer.Tick -= InitTimeTick;

            countdownTimer.Stop();
            countdownTimer.Tick -= InitCountdownTick;
        }


        protected override async void OnInitialize()
        {
            base.OnInitialize();

            var startInterval = TimeSpan.FromMilliseconds(100);

            clockTimer = new DispatcherTimer
            {
                Interval = startInterval
            };
            clockTimer.Tick += InitTimeTick;

            countdownTimer = new DispatcherTimer();
            countdownTimer.Interval += startInterval;
            countdownTimer.Tick += InitCountdownTick;

            clockTimer.Start();
            countdownTimer.Start();

            if (RegionSelectorService.Region == ServerRegion.XBOXEU ||
               RegionSelectorService.Region == ServerRegion.XBOXNA)
            {
                News?.Clear();
                News = new ObservableCollection<NewsItem>(await BdoNewsDataService.GetXboxNews());
            }
            else if (RegionSelectorService.Region == ServerRegion.PCEU ||
                    RegionSelectorService.Region == ServerRegion.PCNA)
            {
                News?.Clear();
                News = new ObservableCollection<NewsItem>(await BdoNewsDataService.GetPcNews());
            }
            else if (RegionSelectorService.Region == ServerRegion.PCSEA)
            {
                News?.Clear();
                News = new ObservableCollection<NewsItem>(await BdoNewsDataService.GetPcNews(isSea: true));
            }
        }

        private void InitCountdownTick(object sender, object e)
        {
            OnCountdownTick(sender, e);

            var timer = (DispatcherTimer)sender;
            timer.Stop();
            timer.Interval = TimeSpan.FromSeconds(20);
            timer.Tick += OnCountdownTick;
            timer.Start();
        }

        private void OnCountdownTick(object sender, object e)
        {
            var utcNow = DateTime.UtcNow;
            DateTime startHr = utcNow - new TimeSpan((int)utcNow.DayOfWeek, utcNow.Hour, utcNow.Minute, utcNow.Second, utcNow.Millisecond);
            double rlDayElapsed = (utcNow - startHr).TotalSeconds;

            var tmmrw = utcNow.AddDays(1).Date;
            var secsUntilDailyReset = tmmrw.Subtract(utcNow);
            DailyReset = secsUntilDailyReset.ToString("%h' hr '%m' min'");

            var secsUntilImperialReset = TimeSpan.FromSeconds(3 * 60 * 60 - rlDayElapsed % (60 * 60 * 3));
            ImperialReset = secsUntilImperialReset.ToString("%h' hr '%m' min'");

            var secsUntilTradeReset = TimeSpan.FromSeconds(4 * 60 * 60 - rlDayElapsed % (60 * 60 * 4));
            ImperialTradeReset = secsUntilTradeReset.ToString("%h' hr '%m' min'");

            var secsUntilBSGameReset = TimeSpan.FromSeconds(60 * 60 * 24 - (rlDayElapsed - 5 * 60 * 60) % (60 * 60 * 24));
            if (secsUntilBSGameReset > TimeSpan.FromSeconds(60 * 60 * 24))
            {
                secsUntilBSGameReset -= TimeSpan.FromSeconds(60 * 60 * 24);
            }
            BSGameReset = secsUntilBSGameReset.ToString("%h' hr '%m' min'");
        }

        private void InitTimeTick(object sender, object e)
        {
            OnTimeTick(sender, e);

            var timer = (DispatcherTimer)sender;
            timer.Stop();
            timer.Interval = TimeSpan.FromMilliseconds(4444.444444);
            timer.Tick += OnTimeTick;
            timer.Start();
        }

        private void OnTimeTick(object sender, object e)
        {
            //Converted code from SomethingLovely.net
            var utcNow = DateTime.UtcNow;
            var startHr = utcNow - new TimeSpan((int)utcNow.DayOfWeek, utcNow.Hour, utcNow.Minute, utcNow.Second, utcNow.Millisecond);
            var rlDayElapsed = (utcNow - startHr).TotalSeconds;
            var secsIntoGameDay = (rlDayElapsed + 200 * 60 + 20 * 60) % (240 * 60);


            int inGameHour;
            int inGameMinute;

            if (secsIntoGameDay >= 12000)
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

            var inGameTime = new TimeSpan(inGameHour, inGameMinute, 0);
            Time = DateTime.Today.Add(inGameTime).ToString("h:mm tt");
        }

        public async void LoadNewsItemAsync()
        {
            if (SelectedNewsItem?.DetailUrl != null)
            {
                await Windows.System.Launcher
                    .LaunchUriAsync(new Uri(SelectedNewsItem.DetailUrl, UriKind.Absolute));
            }
        }


    }
}
