using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using CompanionApp2021.Core.Helpers;
using CompanionApp2021.Core.Models;
using CompanionApp2021.Core.Services;
using CompanionApp2021.Core.Services.Interfaces;
using CompanionApp2021.Models;
using Microsoft.Toolkit.Mvvm.ComponentModel;

namespace CompanionApp2021.ViewModels
{
    public class World_BossesViewModel : ObservableObject
    {
        private static IWorldEventRepository worldEventService = Singleton<WorldEventGimuService>.Instance;

        private ObservableCollection<WorldEventModel> worldEventModels = new ObservableCollection<WorldEventModel>();
        public ObservableCollection<WorldEventModel> WorldEventModels
        {
            get
            {
                return worldEventModels;
            }
            set
            {
                SetProperty(ref worldEventModels, value);
            }
        }

        private bool canToggle = true;

        public World_BossesViewModel()
        {
        }

        public async Task InitializeAsync()
        {
            bool d = true;

            var events = await worldEventService.GetWorldEventsAsync();

            foreach(var e in events)
            {
                e.ImageSource = $"/Assets/BossImages/{e.Name}.png";

                WorldEventModels.Add(new WorldEventModel(d)
                {
                    WorldEvent = e
                });
            }
                
                      
        }

        public void wow()
        {
            WorldEventModel worldEvent = WorldEventModels.ElementAt(0);

            ToggleLock(() =>
            {
                worldEvent.IsNotificationEnabled = false;
                worldEvent.NotifyNotificationChanged();
            });
            
        }

        public void Switch(WorldEventModel worldEvent, bool switchedOn)
        {
            if (canToggle)
            {
                Debug.WriteLine("Toggled: " + switchedOn);
            }
        }

        
        private void ToggleLock(Action doWork)
        {
            canToggle = false;

            doWork.Invoke();

            canToggle = true;
        }


    }
}
