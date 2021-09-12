using CompanionApp2021.Core.Enums;
using CompanionApp2021.Helpers;
using System;
using System.Threading.Tasks;
using Windows.Storage;

namespace CompanionApp2021.Services
{
    public static class RegionSelectorService
    {

        private const string SettingsKey = "AppBackgroundRequestedRegion";
        public static ServerRegion Region { get; set; } = ServerRegion.PCNA;


        public static async Task InitializeAsync()
        {
            Region = await LoadRegionFromSettingsAsync();
        }

        public static async Task SetRegionAsync(ServerRegion region)
        {
            Region = region;

            await SaveThemeInSettingsAsync(Region);
        }


        private static async Task<ServerRegion> LoadRegionFromSettingsAsync()
        {
            ServerRegion cacheRegion = ServerRegion.PCNA;
            string regionName = await ApplicationData.Current.LocalSettings.ReadAsync<string>(SettingsKey);

            if (!string.IsNullOrEmpty(regionName))
            {
                Enum.TryParse(regionName, out cacheRegion);
            }

            return cacheRegion;
        }

        private static async Task SaveThemeInSettingsAsync(ServerRegion region)
        {
            await ApplicationData.Current.LocalSettings.SaveAsync(SettingsKey, region.ToString());
        }

    }
}
