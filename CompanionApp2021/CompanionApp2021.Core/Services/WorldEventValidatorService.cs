using CompanionApp2021.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace CompanionApp2021.Core.Services
{
    public static class WorldEventValidatorService
    {
        public static WorldEvent Validate(string eventName)
        {
            switch (eventName.ToLower())
            {
                case "kzarka":
                    return new WorldEvent
                    {
                        Name = "Kzarka",
                        IsBoss = true,
                    };
                case "karanda":
                    return new WorldEvent
                    {
                        Name = "Karanda",
                        IsBoss = true,
                    };
                case "nouver":
                    return new WorldEvent
                    {
                        Name = "Nouver",
                        IsBoss = true,
                    };
                case "garmoth":
                    return new WorldEvent
                    {
                        Name = "Garmoth",
                        IsBoss = true,
                    };
                case "muraka":
                    return new WorldEvent
                    {
                        Name = "Muraka",
                        IsBoss = true,
                    };
                case "offin":
                    return new WorldEvent
                    {
                        Name = "Offin",
                        IsBoss = true,
                    };
                case "vell":
                    return new WorldEvent
                    {
                        Name = "Vell",
                        IsBoss = true,
                    };
                case "kutum":
                    return new WorldEvent
                    {
                        Name = "Kutum",
                        IsBoss = true,
                    };
                case "quint":
                    return new WorldEvent
                    {
                        Name = "Quint",
                        IsBoss = true,
                    };
                default:
                    return null;
            }
        }
    }
}
