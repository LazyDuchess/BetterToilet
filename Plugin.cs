using BepInEx;
using HarmonyLib;
using System;
using Reptile;
using System.Reflection;
using BepInEx.Logging;

namespace BetterToilet
{
    [BepInPlugin("org.lazyduchess.plugins.brc.bettertoilet", "Better Toilet", "1.0.0.0")]
    [BepInProcess("Bomb Rush Cyberfunk.exe")]
    public class Plugin : BaseUnityPlugin
    {
        public static Plugin Instance;
        private ToiletController _toiletController;
        private void Awake()
        {
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

        public ManualLogSource GetLogger()
        {
            return Logger;
        }
    }
}
