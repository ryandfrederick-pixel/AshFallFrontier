using UnityEngine;

namespace AshfallFrontier.Core
{
    /// <summary>
    /// Marker for player respawn location.
    /// Place one in the zone scene.
    /// </summary>
    public class RespawnPoint : MonoBehaviour
    {
        public static RespawnPoint FindBest()
        {
            return FindFirstObjectByType<RespawnPoint>();
        }
    }
}
