using BepInEx;
using HarmonyLib;
using System;
using BepInEx.Logging;
using BepInEx.Configuration;

namespace BetterToilet
{
    [BepInPlugin("org.lazyduchess.plugins.brc.bettertoilet", "Better Toilet", "1.0.1.0")]
    [BepInProcess("Bomb Rush Cyberfunk.exe")]
    public class Plugin : BaseUnityPlugin
    {
        public static Plugin Instance;
        public ConfigEntry<bool> OpenToiletsUsedWhileWanted;
        private ToiletController _toiletController;

        public ManualLogSource GetLogger()
        {
            return Logger;
        }

        private void Awake()
        {
            OpenToiletsUsedWhileWanted = Config.Bind("General",
                "OpenToiletsUsedWhileWanted",
                false,
                "Whether to also keep toilets that were used with a heat level open.");

            Instance = this;

            try
            {
                Harmony harmony = new Harmony("org.lazyduchess.plugins.brc.bettertoilet");
                harmony.PatchAll();
                _toiletController = new ToiletController();
                Logger.LogInfo($"Loaded Better Toilet plugin.");
            }
            catch(Exception e)
            {
                Logger.LogInfo($"Problem loading Better Toilet plugin: {e}");
            }
        }

        private void Update()
        {
            _toiletController.Update();
        }
    }
}
