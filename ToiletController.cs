using Reptile;
using UnityEngine;
using System.Collections.Generic;
using System.Reflection;

namespace BetterToilet
{
    public class ToiletController
    {
        public static ToiletController Instance;
        private const float DistanceToOpenToilets = 2f;
        // If we're too close to a toilet when it opens we'll be forced into it, creating an infinite toilet entering loop.
        private HashSet<PublicToilet> _toiletsToOpenOncePlayerIsFarAwayEnough = new HashSet<PublicToilet>();

        public ToiletController()
        {
            if (Instance != null)
                return;
            Instance = this;
            StageManager.OnStageInitialized += StageManager_OnStageInitialized;
        }

        // Since we are on a new stage any toilets stored aren't valid anymore.
        private void StageManager_OnStageInitialized()
        {
            _toiletsToOpenOncePlayerIsFarAwayEnough.Clear();
        }

        // Uses reflection to call the private OpenDoor method of the PublicToilet class.
        private void OpenToilet(PublicToilet toilet)
        {
            MethodInfo openDoorMethodInfo = typeof(PublicToilet).GetMethod("OpenDoor", BindingFlags.NonPublic | BindingFlags.Instance);
            var args = new object[] { false };
            openDoorMethodInfo.Invoke(toilet, args);
        }

        public void Update()
        {
            var worldHandler = WorldHandler.instance;
            if (worldHandler == null)
                return;
            var player = worldHandler.GetCurrentPlayer();
            if (player == null)
                return;
            var toilets = new HashSet<PublicToilet>(_toiletsToOpenOncePlayerIsFarAwayEnough);
            foreach(var toilet in toilets)
            {
                if (toilet == null)
                {
                    // Toilet is null, remove it and do nothing.
                    _toiletsToOpenOncePlayerIsFarAwayEnough.Remove(toilet);
                    continue;
                }
                var distanceToPlayer = Vector3.Distance(toilet.transform.position, player.transform.position);
                if (distanceToPlayer >= DistanceToOpenToilets)
                {
                    //Plugin.Instance.GetLogger().LogInfo($"Opened toilet, distance: {distanceToPlayer}");
                    OpenToilet(toilet);
                    _toiletsToOpenOncePlayerIsFarAwayEnough.Remove(toilet);
                }
            }
        }

        public void OpenToiletOncePlayerIsFarAwayEnough(PublicToilet toilet)
        {
            if (_toiletsToOpenOncePlayerIsFarAwayEnough.Contains(toilet))
                return;
            _toiletsToOpenOncePlayerIsFarAwayEnough.Add(toilet);
        }
    }
}
