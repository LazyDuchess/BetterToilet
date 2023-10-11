using HarmonyLib;
using Reptile;

namespace BetterToilet
{
    [HarmonyPatch(typeof(PublicToilet))]
    public class ToiletPatch
    {
        [HarmonyPostfix]
        [HarmonyPatch(nameof(PublicToilet.DoSequence))]
        // This is called when you enter a toilet, before the heat is cleared so the wanted check is appropriate here.
        public static void DoSequence_Postfix(PublicToilet __instance)
        {
            if (Plugin.Instance.OpenToiletsUsedWhileWanted.Value)
            {
                ToiletController.Instance.OpenToiletOncePlayerIsFarAwayEnough(__instance);
                return;
            }
            if (!WantedManager.instance.Wanted)
                ToiletController.Instance.OpenToiletOncePlayerIsFarAwayEnough(__instance);
        }
    }
}
