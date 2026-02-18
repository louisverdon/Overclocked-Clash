using System;

namespace OverclockedClash.Core
{
    /// <summary>
    /// Types de ports disponibles dans le système de circuits
    /// </summary>
    public enum PortType
    {
        Bool,       // Valeur booléenne (true/false)
        Number,     // Valeur numérique (float)
        Event,      // Événement déclenché
        HUD_Number  // Nombre affiché dans le HUD
    }

    public static class PortTypeExtensions
    {
        public static PortType FromString(string type)
        {
            if (string.IsNullOrEmpty(type)) return PortType.Number;
            switch (type.ToLowerInvariant())
            {
                case "bool": return PortType.Bool;
                case "number": return PortType.Number;
                case "event": return PortType.Event;
                case "hud_number": return PortType.HUD_Number;
                default: return PortType.Number;
            }
        }
    }
}

