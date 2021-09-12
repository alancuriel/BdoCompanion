using CompanionApp2021.Core.Models;
using CompanionApp2021.Core.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace CompanionApp2021.Core.Mappers
{
    public static class GimuResponseToWorldEvent
    {
        public static IEnumerable<WorldEvent> Map(GimuResponse gimuResponse)
        {
            var worldEvents = new List<WorldEvent>();

            Type type = gimuResponse.GetType();
            PropertyInfo[] props = type.GetProperties();


            foreach (PropertyInfo prop in props)
            {
                WorldEvent worldEvent = WorldEventValidatorService.Validate(prop.Name);

                if (worldEvent != null)
                {
                    worldEvents.Add(worldEvent);
                }
            }

            return worldEvents;
        }
    }
}
