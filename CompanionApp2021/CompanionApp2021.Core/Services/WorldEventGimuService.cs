using CompanionApp2021.Core.Enums;
using CompanionApp2021.Core.Mappers;
using CompanionApp2021.Core.Models;
using CompanionApp2021.Core.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CompanionApp2021.Core.Services
{
    public class WorldEventGimuService : IWorldEventRepository
    {
        private const string _baseUrl = "https://raw.githubusercontent.com/Gimu/companion-for-bdo/master/bosstimes";

        private const string _naUri = "na.json";
        private const string _euUri = "/eu.json";
        private const string _seaUri = "/sea.json";
        private const string _xboxNaUri = "/xbox-na.json";
        private const string _xboxEuUri = "/xbox-eu.json";

        private HttpDataService httpDataService;

        public WorldEventGimuService()
        {
            httpDataService = new HttpDataService(_baseUrl);
        }

        public async Task<IEnumerable<WorldEvent>> GetWorldEventsAsync(ServerRegion region = ServerRegion.PCNA)
        {
            string uri = MapServerToUri(region);

            var response = await httpDataService.GetAsync<GimuResponse>(uri);

            return GimuResponseToWorldEvent.Map(response);
        }

        private string MapServerToUri(ServerRegion region)
        {
            switch (region)
            {
                case ServerRegion.PCNA:
                    return _naUri;
                case ServerRegion.PCEU:
                    return _euUri;
                case ServerRegion.XBOXNA:
                    return _xboxNaUri;
                case ServerRegion.XBOXEU:
                    return _xboxEuUri;
                case ServerRegion.PCSEA:
                    return _seaUri;
                default:
                    return _naUri;
            }
        }
    }
}
