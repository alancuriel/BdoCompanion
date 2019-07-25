using Caliburn.Micro;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using uwpUI.Core.Models;
using uwpUI.Services;

namespace uwpUI.ViewModels
{
    public class SecondViewModel : Screen
    {
        public SecondViewModel()
        {
        }

        protected override async void OnViewLoaded(object view)
        {
            ViewLoaded = false;
            base.OnViewLoaded(view);
            await CheckBossTimersAsync();
            ViewLoaded = true;
        }

        public bool ViewLoaded { get; set; }

        private ObservableCollection<BossModel> _bosses = new ObservableCollection<BossModel>()
        {
            new BossModel
            {
                Name = "Karanda",
                Img = "ms-appx:///Assets/Karanda.png",
                Location = "Karanda Ridge",
                
            },
            new BossModel
            {
                Name = "Kzarka",
                Img = "ms-appx:///Assets/Kzarka.png",
                Location = "Serendia Shrine"
            },
            new BossModel
            {
                Name = "Garmoth",
                Img = "ms-appx:///Assets/Garmoth.png",
                Location = "Tshira Ruins"
            },
            new BossModel
            {
                Name = "Nouver",
                Img = "ms-appx:///Assets/Nouver.png",
                Location = "The Desert"
            },
            new BossModel
            {
                Name = "Kutum",
                Img = "ms-appx:///Assets/Kutum.png",
                Location = "Scarlet Sand Chamber"
            },
            new BossModel
            {
                Name = "Offin",
                Img = "ms-appx:///Assets/Offin.png",
                Location = "Holo Forest"
            },
            new BossModel
            {
                Name = "Vell",
                Img = "ms-appx:///Assets/Vell.png",
                Location = "The Vell Sea"
            },
            new BossModel
            {
                Name = "Muraka",
                Img = "ms-appx:///Assets/Muraka.png",
                Location = "Mansha Forest"
            },
            new BossModel
            {
                Name = "Quint",
                Img = "ms-appx:///Assets/Quint.png",
                Location = "Quint Hill"
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

        public async void ActivateBossTimerAsync(BossModel boss)
        {
            if (boss != null && ViewLoaded)
            {
                if (boss.IsTimerEnabled)
                {
                    BossNotificationService.DisableBossNotifications(boss);

                }
                else
                {
                    await BossNotificationService.EnableBossNotificationsAsync(boss);

                }

            }
        }

        public async Task CheckBossTimersAsync()
        {
            foreach (var boss in Bosses)
            {
                boss.IsTimerEnabled = BossNotificationService.IsBossTimerEnabled(boss);
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
