using CompanionApp2021.Core.Enums;
using CompanionApp2021.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CompanionApp2021.Core.Services.Interfaces
{
    public interface IWorldEventRepository
    {
       Task<IEnumerable<WorldEvent>> GetWorldEventsAsync(ServerRegion region = ServerRegion.PCNA);
    }
}
