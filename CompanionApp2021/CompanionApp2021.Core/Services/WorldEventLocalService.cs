using CompanionApp2021.Core.Models;
using CompanionApp2021.Core.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace CompanionApp2021.Core.Services
{
    public class WorldEventLocalService : IWorldEventRepository
    {
        public IEnumerable<WorldEvent> GetWorldEvents()
        {
            
            return new List<WorldEvent> {
                new WorldEvent
                {
                    Name = "Kzarka",
                    ImageSource = "/Assets/BossImages/Kzarka.png",
                    SpawnTimes = new List<TimeSpan>
                    {
                        new TimeSpan(hours: 3, minutes: 25, seconds: 5),
                        new TimeSpan(hours: 20, minutes: 40, seconds:3),
                        new TimeSpan(hours: 22, minutes: 25, seconds: 5),
                        new TimeSpan(days: 1, hours: 3, minutes: 25, seconds: 5)
                    }
                },
                new WorldEvent
                {
                    Name = "Karanda",
                    ImageSource = "/Assets/BossImages/Karanda.png",
                    SpawnTimes = new List<TimeSpan>
                    {
                        new TimeSpan(hours: 3, minutes: 25, seconds: 5)
                    }
                }
            };
        }
    }
}
