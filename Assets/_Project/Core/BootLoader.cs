using UnityEngine;
using UnityEngine.SceneManagement;

namespace AshfallFrontier.Core
{
    /// <summary>
    /// Boots the game by ensuring the main zone scene is loaded additively.
    /// Attach this to a GameObject in Boot.unity.
    /// </summary>
    public class BootLoader : MonoBehaviour
    {
        [Tooltip("Scene to load additively as the main playable zone.")]
        public string zoneSceneName = "Zone_AshfallFrontier";

        [Tooltip("If true, attempts to set the loaded zone as active scene.")]
        public bool setZoneActive = true;

        void Awake()
        {
            if (string.IsNullOrWhiteSpace(zoneSceneName)) return;

            // Already loaded?
            for (int i = 0; i < SceneManager.sceneCount; i++)
            {
                var s = SceneManager.GetSceneAt(i);
                if (s.name == zoneSceneName) return;
            }

            var op = SceneManager.LoadSceneAsync(zoneSceneName, LoadSceneMode.Additive);
            if (op != null && setZoneActive)
            {
                op.completed += _ =>
                {
                    var zone = SceneManager.GetSceneByName(zoneSceneName);
                    if (zone.IsValid() && zone.isLoaded)
                        SceneManager.SetActiveScene(zone);
                };
            }
        }
    }
}
