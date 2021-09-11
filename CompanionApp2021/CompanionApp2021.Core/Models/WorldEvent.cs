using System;
using System.Collections.Generic;
using System.Text;

namespace CompanionApp2021.Core.Models
{
    public class WorldEvent
    {
        public string Name { get; set; }
        public string ImageSource { get; set; }
        public List<TimeSpan> SpawnTimes { get; set; }
        public TimeSpan NextSpawnTime { get; set; }
        public bool IsBoss { get; set; }
    }
}
