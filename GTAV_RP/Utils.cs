using System;
using GTANetworkAPI;

namespace GTAV_RP
{
    public class Utils : Script
    {
        public static void ScreenFade(Client player)
        {
            NAPI.ClientEvent.TriggerClientEvent(player, "ScreenFadeOutFadeIn", 500, 1000);
        }

        public static void Log(string logText)
        {
            NAPI.Util.ConsoleOutput($"[{DateTime.Now.ToString("HH:mm:ss")}] [SERVER] {logText}");
        }
    }
}
