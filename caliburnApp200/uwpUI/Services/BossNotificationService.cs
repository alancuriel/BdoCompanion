using Microsoft.Toolkit.Uwp.Notifications;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using uwpUI.Core.Helpers;
using uwpUI.Core.Models;
using uwpUI.Helpers;
using Windows.Storage;
using Windows.UI.Notifications;

namespace uwpUI.Services
{
    public static class BossNotificationService
    {
        static string RegionBossScheduleDir = "ms-appx:///Assets/BossSchedules/";

        private const string BossNotifcationTimeKey = "AppBackgroundRequestedNotificationTime";

        internal static async Task<List<WorldBoss>> GetRegionBossesAsync()
        {
            var scheduleFile = RegionBossScheduleDir + GetRegionScheduleFileName();

            return await ParseFileAsync(scheduleFile);
        }

        private static async Task<List<WorldBoss>> ParseFileAsync(string scheduleFile)
        {
            var bosses = new List<WorldBoss>();
            StorageFile file = await StorageFile.GetFileFromApplicationUriAsync(new Uri(scheduleFile));
            var stream = await file.OpenAsync(FileAccessMode.Read);

            string line;
            using (StreamReader reader = new StreamReader(stream.AsStream()))
            {
                while ((line = reader.ReadLine()) != null)
                {
                    if (line.StartsWith("--"))
                    {
                        continue;
                    }

                    string[] scheduleValues = line.Split(' ');

                    var boss = bosses.Find(b => b.Name == scheduleValues[0]);

                    if (boss == null)
                    {
                        boss = CreateBoss(scheduleValues[0]);
                        bosses.Add(boss);
                    }

                    boss.SpawnTimes.Add(
                            new TimeSpan((int)StringToDayOfWeek(scheduleValues[1]), 0, 0, 0) +
                            TimeSpan.Parse(scheduleValues[2]));
                }
            }

            CheckBossTimersEnabled(bosses);
            return bosses;
        }

        private static WorldBoss CreateBoss(string v)
        {
            switch (v)
            {
                case "Karanda":
                    return new WorldBoss
                    {
                        Name = "Karanda",
                        Image = "ms-appx:///Assets/Karanda.png",
                        Location = "Karanda Ridge",
                        SpawnTimes = new List<TimeSpan>(),
                        IsTimerEnabled = false
                    };
                case "Kzarka":
                    return new WorldBoss
                    {
                        Name = "Kzarka",
                        Image = "ms-appx:///Assets/Kzarka.png",
                        Location = "Serendia Shrine",
                        SpawnTimes = new List<TimeSpan>(),
                        IsTimerEnabled = false
                    };
                case "Garmoth":
                    return new WorldBoss
                    {
                        Name = "Garmoth",
                        Image = "ms-appx:///Assets/Garmoth.png",
                        Location = "Tshira Ruins",
                        SpawnTimes = new List<TimeSpan>(),
                        IsTimerEnabled = false
                    };
                case "Nouver":
                    return new WorldBoss
                    {
                        Name = "Nouver",
                        Image = "ms-appx:///Assets/Nouver.png",
                        Location = "The Desert",
                        SpawnTimes = new List<TimeSpan>(),
                        IsTimerEnabled = false
                    };
                case "Kutum":
                    return new WorldBoss
                    {
                        Name = "Kutum",
                        Image = "ms-appx:///Assets/Kutum.png",
                        Location = "Scarlet Sand Chamber",
                        SpawnTimes = new List<TimeSpan>(),
                        IsTimerEnabled = false
                    };
                case "Offin":
                    return new WorldBoss
                    {
                        Name = "Offin",
                        Image = "ms-appx:///Assets/Offin.png",
                        Location = "Holo Forest",
                        SpawnTimes = new List<TimeSpan>(),
                        IsTimerEnabled = false
                    };
                case "Vell":
                    return new WorldBoss
                    {
                        Name = "Vell",
                        Image = "ms-appx:///Assets/Vell.png",
                        Location = "The Vell Sea",
                        SpawnTimes = new List<TimeSpan>(),
                        IsTimerEnabled = false
                    };
                case "Muraka":
                    return new WorldBoss
                    {
                        Name = "Muraka",
                        Image = "ms-appx:///Assets/Muraka.png",
                        Location = "Mansha Forest",
                        SpawnTimes = new List<TimeSpan>(),
                        IsTimerEnabled = false
                    };
                case "Quint":
                    return new WorldBoss
                    {
                        Name = "Quint",
                        Image = "ms-appx:///Assets/Quint.png",
                        Location = "Quint Hill",
                        SpawnTimes = new List<TimeSpan>(),
                        IsTimerEnabled = false
                    };
                default:
                    throw new Exception("Boss Does Not Exist");
            }
        }

        private static bool BossExists(string s)
        {
            return s == "Kzarka" || s == "Karanda"
                || s == "Garmoth" || s == "Nouver"
                || s == "Kutum" || s == "Offin"
                || s == "Vell" || s == "Muraka"
                || s == "Quint";
        }

        public static int NotifyTime { get; set; } = 0;

        public static async Task InitializeAsync()
        {
            NotifyTime = await LoadNotifyTimeFromSettingsAsync();
        }

        private static async Task<int> LoadNotifyTimeFromSettingsAsync()
        {
            int cacheNotifyTime = 0; // 0 is Default
            string notifyTimeName = await ApplicationData.Current.LocalSettings.ReadAsync<string>(BossNotifcationTimeKey);

            if (!string.IsNullOrEmpty(notifyTimeName))
            {
                Int32.TryParse(notifyTimeName, out cacheNotifyTime);
            }

            return cacheNotifyTime;
        }

        public static async Task SetNotifyTimeAsync(int min)
        {
            NotifyTime = min;

            await SaveNotifyTimeInSettingsAsync(min);
        }

        private static async Task SaveNotifyTimeInSettingsAsync(int min)
        {
            await ApplicationData.Current.LocalSettings.SaveAsync(BossNotifcationTimeKey, min.ToString());
        }

        public static async Task EnableNotifications(WorldBoss boss)
        {
            var spawnDateTimes = CreateSpawnTimes(boss.SpawnTimes);

            ScheduleNotifications(boss, spawnDateTimes);
            await Task.CompletedTask;
        }

        public static async Task EnableBossNotificationsAsync(BossModel boss)
        {
            string regionBossSchedule = RegionBossScheduleDir;

            regionBossSchedule += GetRegionScheduleFileName();

            List<DateTime> spawnTimes = await GetSpawnTimesAsync(boss, regionBossSchedule);

            ScheduleBossSpawns(boss, spawnTimes);

        }

        public static string GetRegionScheduleFileName()
        {
            switch (RegionSelectorService.Region)
            {
                case ServerRegion.PCNA:
                    return "PC-NA-BossSchedule-PDT.txt";
                case ServerRegion.XBOXNA:
                    return "Xbox-Na-BossSchedule-PDT.txt";
                case ServerRegion.PCEU:
                    return "PC-EU-BossSchedule-CEST.txt";
                case ServerRegion.XBOXEU:
                    return "Xbox-Eu-BossSchedule-UTC+1.txt";
                case ServerRegion.PCSEA:
                    return "PC-SEA-BossSchedule-WITA.txt";
                default:
                    throw new Exception("An error occured getting the region boss schedule file");
            }
        }

        public static void DisableBossNotifications(BossModel boss)
        {
            ToastNotifier notifier = ToastNotificationManager.CreateToastNotifier();

            IReadOnlyList<ScheduledToastNotification> scheduledToasts = notifier.GetScheduledToastNotifications();

            foreach (var toast in scheduledToasts)
            {
                if (toast.Group == boss.Name)
                {
                    notifier.RemoveFromSchedule(toast);
                }
            }
        }

        public static async Task DisableNotifications(WorldBoss boss)
        {
            var tm = ToastNotificationManager.CreateToastNotifier();

            foreach (var toast in tm.GetScheduledToastNotifications())
            {
                if (toast.Group == boss.Name)
                {
                    tm.RemoveFromSchedule(toast);
                }
            }
            await Task.CompletedTask;
        }

        public static void DisableAllBossNotifications()
        {
            ToastNotifier notifier = ToastNotificationManager.CreateToastNotifier();
            IReadOnlyList<ScheduledToastNotification> scheduledToasts = notifier.GetScheduledToastNotifications();

            foreach (var toast in scheduledToasts)
            {
                if(BossExists(toast.Group))
                {
                    notifier.RemoveFromSchedule(toast);
                }
            }
        }

        private static async Task<List<DateTime>> GetSpawnTimesAsync(BossModel boss, string bossSchedule)
        {
            List<DateTime> spawnTimes = new List<DateTime>();

            StorageFile file = await StorageFile.GetFileFromApplicationUriAsync(new Uri(bossSchedule));
            var stream = await file.OpenAsync(FileAccessMode.Read);

            string line;
            using (StreamReader reader = new StreamReader(stream.AsStream()))
            {
                while ((line = reader.ReadLine()) != null)
                {
                    if (line == "--")
                    {
                        continue;
                    }

                    string[] scheduleValues = line.Split(' ');

                    if (scheduleValues[0] == boss.Name)
                    {

                        spawnTimes.Add(
                            CreateSpawnTime(StringToDayOfWeek(scheduleValues[1]),
                            TimeSpan.Parse(scheduleValues[2])));
                    }
                }
            }

            return spawnTimes;
        }

        private static TimeZoneInfo GetRegionTimeZoneInfo()
        {
            if (RegionSelectorService.Region == ServerRegion.PCNA
            || RegionSelectorService.Region == ServerRegion.XBOXNA)
            {
                return TimeZoneInfo.FindSystemTimeZoneById("Pacific Standard Time");
            }
            else if (RegionSelectorService.Region == ServerRegion.XBOXEU)
            {
                return TimeZoneInfo.CreateCustomTimeZone("UTC+01", TimeSpan.FromHours(1), "UTC + 1", "UTC + 1 Time");
            }
            else if (RegionSelectorService.Region == ServerRegion.PCEU)
            {
                return TimeZoneInfo.FindSystemTimeZoneById("Central Europe Standard Time");
            }
            else if (RegionSelectorService.Region == ServerRegion.PCSEA)
            {
                return TimeZoneInfo.FindSystemTimeZoneById("China Standard Time");
            }
            else
            {
                throw new Exception("Invalid Server region was used");
            }
        }

        private static List<DateTime> CreateSpawnTimes(IEnumerable<TimeSpan> timeOfWeeks)
        {
            TimeZoneInfo timeZoneInfo = GetRegionTimeZoneInfo();

            var spawnDateTimes = new List<DateTime>();

            for (int i = 0; i < 4; i++/*3 weeks */)
            {
                foreach (var timeOfWeek in timeOfWeeks)
                {
                    DateTime now = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, timeZoneInfo);
                    DateTime startOfWeek = now - new TimeSpan((int)now.DayOfWeek, now.Hour, now.Minute, now.Second, now.Millisecond);

                    DateTime TimeAppear = startOfWeek + timeOfWeek;
                    if (i > 0) { TimeAppear += new TimeSpan(i*7,0, 0, 0, 0); }
                    // (now.IsDaylightSavingTime() ? timeOfDay : timeOfDay.Add(TimeSpan.FromHours(1)));

                    TimeAppear = TimeZoneInfo.ConvertTimeToUtc(TimeAppear, timeZoneInfo) - TimeSpan.FromMinutes(NotifyTime);

                    if (TimeAppear < DateTime.UtcNow)
                    {
                        continue;
                    }

                    spawnDateTimes.Add(TimeAppear);
                }
            }

            return spawnDateTimes;
        }

        private static DateTime CreateSpawnTime(DayOfWeek day, TimeSpan timeOfDay)
        {
            TimeZoneInfo timeZoneInfo = GetRegionTimeZoneInfo();

            DateTime now = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, timeZoneInfo);
            DateTime startOfWeek = now - new TimeSpan((int)now.DayOfWeek, now.Hour, now.Minute, now.Second, now.Millisecond);

            DateTime TimeAppear = startOfWeek + new TimeSpan((int)day, 0, 0, 0) + timeOfDay;
            // (now.IsDaylightSavingTime() ? timeOfDay : timeOfDay.Add(TimeSpan.FromHours(1)));

            TimeAppear = TimeZoneInfo.ConvertTimeToUtc(TimeAppear, timeZoneInfo) - TimeSpan.FromMinutes(NotifyTime);

            if (TimeAppear < DateTime.UtcNow)
            {
                TimeAppear = TimeAppear + new TimeSpan(7, 0, 0, 0);
            }


            return TimeAppear ;
        }

        private static DayOfWeek StringToDayOfWeek(string str)
        {
            string[] day = { "Sun", "Mon", "Tue", "Wed", "Thu", "Fri", "Sat" };

            for (int i = 0; i < day.Length; i++)
            {
                if (day[i] == str)
                    return (DayOfWeek)i;
            }

            return DayOfWeek.Sunday;
        }

        public static void CheckBossTimersEnabled(List<WorldBoss> worldBosses)
        {
            var scheduledNotifications = ToastNotificationManager.CreateToastNotifier()
                .GetScheduledToastNotifications();

            var dict = worldBosses.ToDictionary(x => x.Name, x => x.IsTimerEnabled);

            foreach (var notification in scheduledNotifications)
            {
                if (!dict.Any(x => !x.Value))
                {
                    break;
                }

                if (dict.ContainsKey(notification.Group))
                {
                    dict[notification.Group] = true;
                }
            }

            foreach (var boss in worldBosses)
            {
                if (dict[boss.Name])
                {
                    boss.IsTimerEnabled = true;
                }
            }
        }

        public static bool IsBossTimerEnabled(string bossName)
        {
            return ToastNotificationManager.CreateToastNotifier()
                .GetScheduledToastNotifications().Any(t => t.Group == bossName);
        }

        public static async Task<bool> IsBossInRegionAsync(BossModel boss)
        {
            string scheduleFile = RegionBossScheduleDir;
            scheduleFile += GetRegionScheduleFileName();

            StorageFile file = await StorageFile.GetFileFromApplicationUriAsync(new Uri(scheduleFile));

            string text = await Windows.Storage.FileIO.ReadTextAsync(file);

            bool inRegion = text.Contains(boss.Name);
            return inRegion;
        }

        private static void ScheduleNotifications(WorldBoss boss, IEnumerable<DateTime> dateTimes)
        {
            var toastContentXml = CreateToastContent(boss).GetXml();
            var toastService = Singleton<ToastNotificationsService>.Instance;
            
            var toastSchduledNotifications = dateTimes.Select(x => new  ScheduledToastNotification(toastContentXml, new DateTimeOffset(x))
            {
                Group = boss.Name
            }).ToList();
            
            toastService.ScheduleToastNotifications(toastSchduledNotifications);

            //foreach (var dateTime in dateTimes)
            //{
                
            //    toastService.ScheduleToastNotification(new ScheduledToastNotification(toastContentXml, dateTime)
            //    {
            //        Group = boss.Name
            //    });
            //}
        }

        private static ToastContent CreateToastContent(WorldBoss boss)
        {
            return new ToastContent()
            {
                Visual = new ToastVisual()
                {
                    BindingGeneric = new ToastBindingGeneric()
                    {
                        Children =
                        {
                            new AdaptiveText()
                            {
                                HintMaxLines = 2,
                                Text = NotifyTime > 0 ? $"{boss.Name} will spawn in {NotifyTime} minutes." :$"{boss.Name} has spawned!"
                            },

                            new AdaptiveText()
                            {
                                 Text = $"Located at {boss.Location}"
                            }
                        },

                        AppLogoOverride = new ToastGenericAppLogo()
                        {
                            Source = boss.Image
                        }
                    },
                }
            };
        }

        public static TimeSpan GetTimeTillNextSpawn(WorldBoss boss)
        {
            var appearTimes = new List<TimeSpan>();
            var timeZoneInfo = GetRegionTimeZoneInfo();
            foreach (var time in boss.SpawnTimes)
            {
                DateTime now = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, timeZoneInfo);
                DateTime startOfWeek =
                    now - new TimeSpan((int)now.DayOfWeek, now.Hour, now.Minute, now.Second, now.Millisecond);

                var timeAppear = startOfWeek + time;

                if (timeAppear > now)
                {
                    return timeAppear - now;
                }
                else
                {
                    appearTimes.Add(timeAppear.AddDays(7) - now);
                }
            }
            return appearTimes.FirstOrDefault();
        }

        private static void ScheduleBossSpawns(BossModel boss, List<DateTime> spawns)
        {
            foreach (var time in spawns)
            {
                var bossToast = CreateBossToastNotification(boss, time);
                Singleton<ToastNotificationsService>.Instance.ScheduleToastNotification(bossToast);
            }

        }

        private static ScheduledToastNotification CreateBossToastNotification(BossModel boss, DateTime spawnTime)
        {
            // Create the toast content
            var content = new ToastContent()
            {
                // More about the Launch property at https://docs.microsoft.com/dotnet/api/microsoft.toolkit.uwp.notifications.toastcontent


                Visual = new ToastVisual()
                {
                    BindingGeneric = new ToastBindingGeneric()
                    {
                        Children =
                        {
                            new AdaptiveText()
                            {
                                HintMaxLines = 2,
                                Text = NotifyTime > 0 ? $"{boss.Name} will spawn in {NotifyTime} minutes." :$"{boss.Name} has spawned!"
                            },

                            new AdaptiveText()
                            {
                                 Text = $"Located at {boss.Location}"
                            }
                        },

                        AppLogoOverride = new ToastGenericAppLogo()
                        {
                            Source = boss.Img
                        }
                    },
                }
            };

            // Add the content to the toast
            var toast = new ScheduledToastNotification(content.GetXml(), spawnTime)
            {
                Group = boss.Name,
            };

            return toast;
        }

    }
}
