using System;
using System.Collections.ObjectModel;
using System.Linq;
using Caliburn.Micro;
using uwpUI.Core.Helpers;
using uwpUI.Core.Models;
using uwpUI.Helpers;
using uwpUI.Services;

namespace uwpUI.ViewModels
{
    public class BossNotifcationsViewModel : Screen
    {
        private ObservableCollection<WorldBoss> _worldBosses = new ObservableCollection<WorldBoss>();

        public BossNotifcationsViewModel()
        {
            NotificationTime = new BindableCollection<int>(Enumerable.Range(0, 91));
        }

        protected override async void OnInitialize()
        {
            base.OnInitialize();
            var bosses = await BossNotificationService.GetRegionBossesAsync();
            bosses.ForEach(boss =>
           {
               WorldBosses.Add(boss);
           });

            NotifyOfPropertyChange(() => WorldBosses);
        }

        //protected override void OnViewLoaded(object view)
        //{
        //    base.OnViewLoaded(view);
        //    NotifyOfPropertyChange(() => WorldBosses);
        //}

        public WorldBoss SelectedBoss { get; set; }

        public ObservableCollection<WorldBoss> WorldBosses
        {
            get
            {
                return _worldBosses;
            }
            set
            {
                Set(ref _worldBosses, value);
            }
        }
        //= new ObservableCollection<WorldBoss>() {
        //        new WorldBoss()
        //        {
        //            Name = "Karanda",
        //            Image = "ms-appx:///Assets/Karanda.png",
        //            Location = "Karanda Ridge"
        //        },
        //        new WorldBoss()
        //        {
        //            Name = "Kzarka",
        //            Image = "ms-appx:///Assets/Kzarka.png",
        //            Location = "Serendia Shrine"
        //        }
        //    };

        public void EnableTimer2Async(WorldBoss boss)
        {
            if (boss != null)
            {
                EnableTimerAsync(boss);

                NotifyOfPropertyChange(() => WorldBosses);
            }
        }

        public async void EnableTimerAsync(WorldBoss boss)
        {
            if (boss != null)
            {
                if (boss.IsTimerEnabled)
                {
                    await BossNotificationService.DisableNotifications(boss);
                    boss.IsTimerEnabled = false;
                }
                else
                {
                    await BossNotificationService.EnableNotifications(boss);
                    boss.IsTimerEnabled = true;
                }
            }
        }

        public BindableCollection<int> NotificationTime { get; private set; }

        private int _selectedNotificationTime = BossNotificationService.NotifyTime;

        public int SelectedNotificationTime
        {
            get { return _selectedNotificationTime; }
            set
            {
                DisableAll();
                SwitchNotifyTime(value);
                Set(ref _selectedNotificationTime, value);
            }
        }

        public async void SwitchNotifyTime(int min)
        {
            await BossNotificationService.SetNotifyTimeAsync(min);
        }

        public void NotificationTest() => Singleton<ToastNotificationsService>.Instance.ShowToastNotificationSample();

        public void DisableAll()
        {
            BossNotificationService.DisableAllBossNotifications();
            foreach (var boss in WorldBosses) { boss.IsTimerEnabled = false; }
            NotifyOfPropertyChange(() => WorldBosses);
        }
        public async void EnableAll()
        {
            if(WorldBosses?.Count > 0)
            {
                BossNotificationService.DisableAllBossNotifications();
                foreach (var boss in WorldBosses)
                {
                    if (!boss.IsTimerEnabled)
                    {
                        await BossNotificationService.EnableNotifications(boss);
                        boss.IsTimerEnabled = true;
                    }
                }

                NotifyOfPropertyChange(() => WorldBosses);
            }
        }
    }
}
