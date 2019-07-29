using Microsoft.Toolkit.Uwp.Notifications;
using System;
using System.Collections.Generic;
using System.IO;
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
        public static int NotifyTime { get; set; } = 0;

        public static async Task InitializeAsync()
        {
            NotifyTime = await LoadNotifyTimeFromSettingsAsync();
        }

        private static async Task<int> LoadNotifyTimeFromSettingsAsync()
        {
            int cacheNotifyTime = 0;//Default
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

        public static void DisableAllBossNotifications()
        {
            ToastNotifier notifier = ToastNotificationManager.CreateToastNotifier();
            IReadOnlyList<ScheduledToastNotification> scheduledToasts = notifier.GetScheduledToastNotifications();

            foreach (var toast in scheduledToasts)
            {
                switch (toast.Group)
                {
                    case "Kzarka": notifier.RemoveFromSchedule(toast); break;
                    case "Karanda": notifier.RemoveFromSchedule(toast); break;
                    case "Garmoth": notifier.RemoveFromSchedule(toast); break;
                    case "Kutum": notifier.RemoveFromSchedule(toast); break;
                    case "Offin": notifier.RemoveFromSchedule(toast); break;
                    case "Vell": notifier.RemoveFromSchedule(toast); break;
                    case "Muraka": notifier.RemoveFromSchedule(toast); break;
                    case "Quint": notifier.RemoveFromSchedule(toast); break;
                    case "Nouver": notifier.RemoveFromSchedule(toast); break;
                    default: break;
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

        private static DateTime CreateSpawnTime(DayOfWeek day, TimeSpan timeOfDay)
        {
            TimeZoneInfo timeZoneInfo = null;

            if (RegionSelectorService.Region == ServerRegion.PCNA
            || RegionSelectorService.Region == ServerRegion.XBOXNA)
            {
                timeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById("Pacific Standard Time");
            }
            else if (RegionSelectorService.Region == ServerRegion.XBOXEU)
            {
                timeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById("W. Europe Standard Time");
            }
            else if (RegionSelectorService.Region == ServerRegion.PCEU)
            {
                timeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById("Central Europe Standard Time");
            }
            else
            {
                throw new Exception("Invalid Server region was used");
            }

            DateTime now = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, timeZoneInfo);
            DateTime startOfWeek = now - new TimeSpan((int)now.DayOfWeek, now.Hour, now.Minute, now.Second, now.Millisecond);

            DateTime TimeAppear = startOfWeek + new TimeSpan((int)day, 0, 0, 0) + timeOfDay;
            TimeAppear = TimeZoneInfo.ConvertTimeToUtc(TimeAppear, timeZoneInfo);

            if (TimeAppear < DateTime.UtcNow)
            {
                TimeAppear = TimeAppear + new TimeSpan(7, 0, 0, 0);
            }


            return TimeAppear;
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

        public static bool IsBossTimerEnabled(string bossName)
        {
            IReadOnlyList<ScheduledToastNotification> scheduledToasts =
            ToastNotificationManager.CreateToastNotifier().GetScheduledToastNotifications();

            foreach (var toast in scheduledToasts)
            {
                if (toast.Group == bossName)
                {
                    return true;
                }
            }

            return false;
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
                                Text = $"{boss.Name} has spawned"
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
                // TODO WTS: Set a unique identifier for this notification within the notification group. (optional)
                // More details at https://docs.microsoft.com/uwp/api/windows.ui.notifications.toastnotification.tag
                Group = boss.Name,

            };

            return toast;
        }

    }
}
