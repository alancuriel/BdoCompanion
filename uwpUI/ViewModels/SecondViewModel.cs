using Caliburn.Micro;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using uwpUI.Core.Models;
using uwpUI.Services;

namespace uwpUI.ViewModels
{
    public class SecondViewModel : Screen
    {
        public SecondViewModel()
        {
            NotificationTime = new BindableCollection<int>(Enumerable.Range(0, 91));           
        }
        protected override async void OnInitialize()
        {
            ViewLoaded = false;
            
            await CheckBossTimersAsync();
            ViewLoaded = true;
            base.OnInitialize();
        }

        //protected override async void OnViewLoaded(object view)
        //{
        //    ViewLoaded = false;
        //    base.OnViewLoaded(view);
        //    await CheckBossTimersAsync();
        //    ViewLoaded = true;
        //}

        public bool ViewLoaded { get; set; } = false;

        private ObservableCollection<BossModel> _bosses = new ObservableCollection<BossModel>()
        {
            new BossModel
            {
                Name = "Karanda",
                Img = "ms-appx:///Assets/Karanda.png",
                Location = "Karanda Ridge",
                IsTimerEnabled = BossNotificationService.IsBossTimerEnabled("Karanda")   
            },
            new BossModel
            {
                Name = "Kzarka",
                Img = "ms-appx:///Assets/Kzarka.png",
                Location = "Serendia Shrine",
                IsTimerEnabled = BossNotificationService.IsBossTimerEnabled("Kzarka")
            },
            new BossModel
            {
                Name = "Garmoth",
                Img = "ms-appx:///Assets/Garmoth.png",
                Location = "Tshira Ruins",
                IsTimerEnabled = BossNotificationService.IsBossTimerEnabled("Garmoth")
            },
            new BossModel
            {
                Name = "Nouver",
                Img = "ms-appx:///Assets/Nouver.png",
                Location = "The Desert",
                IsTimerEnabled = BossNotificationService.IsBossTimerEnabled("Nouver")
            },
            new BossModel
            {
                Name = "Kutum",
                Img = "ms-appx:///Assets/Kutum.png",
                Location = "Scarlet Sand Chamber",
                IsTimerEnabled = BossNotificationService.IsBossTimerEnabled("Kutum")
            },
            new BossModel
            {
                Name = "Offin",
                Img = "ms-appx:///Assets/Offin.png",
                Location = "Holo Forest",
                IsTimerEnabled = BossNotificationService.IsBossTimerEnabled("Offin")
            },
            new BossModel
            {
                Name = "Vell",
                Img = "ms-appx:///Assets/Vell.png",
                Location = "The Vell Sea",
                IsTimerEnabled = BossNotificationService.IsBossTimerEnabled("Offin")
            },
            new BossModel
            {
                Name = "Muraka",
                Img = "ms-appx:///Assets/Muraka.png",
                Location = "Mansha Forest",
                IsTimerEnabled = BossNotificationService.IsBossTimerEnabled("Muraka")
            },
            new BossModel
            {
                Name = "Quint",
                Img = "ms-appx:///Assets/Quint.png",
                Location = "Quint Hill",
                IsTimerEnabled = BossNotificationService.IsBossTimerEnabled("Quint")
            }
        };
        public ObservableCollection<BossModel> Bosses
        {
            get
            {
                return _bosses;
            }
            set
            {
                Set(ref _bosses, value);
            }
        }

        public  BindableCollection<int> NotificationTime { get; private set; }

        private int _selectedNotificationTime = BossNotificationService.NotifyTime;

        public int SelectedNotificationTime
        {
            get { return _selectedNotificationTime; }
            set
            {
                BossNotificationService.DisableAllBossNotifications();
                foreach(var boss in Bosses) { boss.IsTimerEnabled = false; }
                NotifyOfPropertyChange(() => Bosses);
                SwitchNotifyTime(value);
                Set(ref _selectedNotificationTime, value);
            }
        }

        public async void SwitchNotifyTime(int min)
        {
            await BossNotificationService.SetNotifyTimeAsync(min);
        }

        private BossModel _selectedBoss;

        public BossModel SelectedBoss
        {
            get
            {
                return _selectedBoss;
            }
            set
            {
                Set(ref _selectedBoss, value);
            }
        }

        private string _testString;

        public string TestString
        {
            get { return _testString; }
            set
            {
                Set(ref _testString, value);
            }
        }

        public async void ActivateBossTimerAsync(BossModel boss)
        {
            if (boss != null && ViewLoaded)
            {
                if (boss.IsTimerEnabled)
                {
                    BossNotificationService.DisableBossNotifications(boss);
                    boss.IsTimerEnabled = false;
                }
                else
                {
                    await BossNotificationService.EnableBossNotificationsAsync(boss);
                    boss.IsTimerEnabled = true;
                }

            }
        }

        public async Task CheckBossTimersAsync()
        {
            foreach (var boss in Bosses)
            {
                //boss.IsTimerEnabled = BossNotificationService.IsBossTimerEnabled(boss);
                boss.Show = await BossNotificationService.IsBossInRegionAsync(boss);
            }

            int j = Bosses.Count;

            for (int i = 0; i < j; i++)
            {
                if (Bosses[i].Show)
                {
                    Bosses.Move(i, 0);
                }
            }
            NotifyOfPropertyChange(() => Bosses);
        }

    }
}
