using CompanionApp2021.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace CompanionApp2021.Core.Services.Interfaces
{
    public interface IWorldEventRepository
    {
        IEnumerable<WorldEvent> GetWorldEvents();
    }
}
