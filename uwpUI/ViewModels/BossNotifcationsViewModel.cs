using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Caliburn.Micro;
using uwpUI.Core.Helpers;
using uwpUI.Core.Models;
using uwpUI.Helpers;
using uwpUI.Services;
using Windows.UI.Xaml;

namespace uwpUI.ViewModels
{
    public class BossNotifcationsViewModel : Screen
    {
        private ObservableCollection<WorldBoss> _worldBosses = new ObservableCollection<WorldBoss>();

        public BossNotifcationsViewModel()
        {
            NotificationTime = new BindableCollection<int>(Enumerable.Range(0, 91));
        }

        readonly private ServerRegion _serverRegion = RegionSelectorService.Region;

        public ServerRegion ServerRegion
        {
            get { return _serverRegion; }
        }

        private bool _isLoading = false;

        public bool IsLoading
        {
            get { return _isLoading; }
            set
            {
                Set(ref _isLoading, value);
            }
        }


        private DispatcherTimer bossSpawnTimer;

        protected override async void OnInitialize()
        {
            base.OnInitialize();
            var bosses = await BossNotificationService.GetRegionBossesAsync();
            bosses.ForEach(boss =>
           {
               WorldBosses.Add(boss);
           });

            bossSpawnTimer = new DispatcherTimer
            {
                Interval = TimeSpan.FromSeconds(1)
            };
            bossSpawnTimer.Tick += BossSpawnTimer_Tick;

            bossSpawnTimer.Start();
            NotifyOfPropertyChange(() => WorldBosses);
        }

        protected override void OnDeactivate(bool close)
        {
            base.OnDeactivate(close);

            bossSpawnTimer.Stop();
            bossSpawnTimer.Tick -= BossSpawnTimer_Tick;
        }

        private void BossSpawnTimer_Tick(object sender, object e)
        {
            foreach (var boss in WorldBosses)
            {
                boss.NextSpawnTime = BossNotificationService.GetTimeTillNextSpawn(boss);

                if (boss.NextSpawnTime.Days > 0)
                {
                    boss.TimeTillNextSpawn = boss.NextSpawnTime.ToString(@"d\.hh\:mm\:ss");
                }
                else
                {
                    boss.TimeTillNextSpawn = boss.NextSpawnTime.ToString(@"hh\:mm\:ss");
                }

            }
            WorldBosses.OrderBy(b => b.NextSpawnTime);
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
                DisableAllAsync();
                SwitchNotifyTime(value);
                Set(ref _selectedNotificationTime, value);
            }
        }

        public async void SwitchNotifyTime(int min)
        {
            await BossNotificationService.SetNotifyTimeAsync(min);
        }

        public void NotificationTest() => Singleton<ToastNotificationsService>.Instance.ShowToastNotificationSample();

        public async void DisableAllAsync()
        {
            await Task.Run( () =>
            {
                IsLoading = true;
                NotifyOfPropertyChange(() => IsLoading);
            });
            

            BossNotificationService.DisableAllBossNotifications();
            foreach (var boss in WorldBosses) { boss.IsTimerEnabled = false; }
            NotifyOfPropertyChange(() => WorldBosses);


            await Task.Run(() =>
            {
                IsLoading = false;
                NotifyOfPropertyChange(() => IsLoading);
            });

            await Task.CompletedTask;
        }
        public async void EnableAllAsync()
        {
            if(WorldBosses?.Count > 0)
            {
                await Task.Run(() =>
                {
                    IsLoading = true;
                    NotifyOfPropertyChange(() => IsLoading);
                });

                BossNotificationService.DisableAllBossNotifications();

                Task.WaitAll(EnableAllTasks().ToArray());

                //foreach (var boss in WorldBosses)
                //{
                //        await BossNotificationService.EnableNotifications(boss);
                //        boss.IsTimerEnabled = true;              
                //}

                NotifyOfPropertyChange(() => WorldBosses);

                await Task.Run(() =>
                {
                    IsLoading = false;
                    NotifyOfPropertyChange(() => IsLoading);
                });

                await Task.CompletedTask;
            }
        }

        public IEnumerable<Task> EnableAllTasks()
        {
            //var tasks = new Task[WorldBosses.Count];
            //for(short i = 0; i < WorldBosses.Count; i++)
            //{
            //     tasks[i] = BossNotificationService.EnableNotifications(WorldBosses[i]);
            //    WorldBosses[i].IsTimerEnabled = true;
            //}
            //return tasks;

            foreach(var boss in WorldBosses)
            {
                boss.IsTimerEnabled = true;
                yield return BossNotificationService.EnableNotifications(boss);
            }

        }

    }
}
