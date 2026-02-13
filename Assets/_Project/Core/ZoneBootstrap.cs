using UnityEngine;
using AshfallFrontier.Combat;
using AshfallFrontier.UI;

namespace AshfallFrontier.Core
{
    /// <summary>
    /// Minimal zone bootstrapper.
    /// Drop one of these in Zone_AshfallFrontier.
    /// 
    /// Responsibilities:
    /// - Spawn player prefab at spawn point
    /// - Ensure camera targets player
    /// - Spawn HUD prefab (or create HUD via HudAutoBuilder) and bind to player
    /// - (Optional) spawn enemies at provided spawn points
    /// 
    /// Keep this intentionally simple for M1 so scenes don't require manual wiring.
    /// </summary>
    public class ZoneBootstrap : MonoBehaviour
    {
        [Header("Prefabs")]
        public GameObject playerPrefab;
        public GameObject hudPrefab;
        public GameObject enemyPrefab;

        [Header("Spawn Points")]
        public Transform playerSpawn;
        public Transform[] enemySpawns;

        [Header("Camera")]
        [Tooltip("If empty, will use Camera.main.")]
        public ThirdPersonCamera thirdPersonCamera;

        [Header("Options")]
        public bool spawnEnemies = true;
        public bool dontDestroyPlayer = false;

        private Combatant _playerCombatant;

        void Awake()
        {
            Bootstrap();
        }

        [ContextMenu("Bootstrap Now")]
        public void Bootstrap()
        {
            EnsurePlayer();
            EnsureCamera();
            EnsureHud();
            if (spawnEnemies) EnsureEnemies();
        }

        void EnsurePlayer()
        {
            // If a player already exists, use it.
            var existing = GameObject.FindWithTag("Player");
            if (existing)
            {
                _playerCombatant = existing.GetComponent<Combatant>();
                return;
            }

            if (!playerPrefab)
            {
                Debug.LogWarning("[ZoneBootstrap] Missing playerPrefab; cannot spawn player.");
                return;
            }

            Vector3 pos = playerSpawn ? playerSpawn.position : Vector3.zero;
            Quaternion rot = playerSpawn ? playerSpawn.rotation : Quaternion.identity;

            var go = Instantiate(playerPrefab, pos, rot);
            if (!go.CompareTag("Player"))
                Debug.LogWarning("[ZoneBootstrap] Spawned player prefab is not tagged 'Player'. Please tag the prefab for auto-wiring.");

            _playerCombatant = go.GetComponent<Combatant>();

            if (dontDestroyPlayer)
                DontDestroyOnLoad(go);
        }

        void EnsureCamera()
        {
            if (!_playerCombatant) return;

            if (!thirdPersonCamera)
            {
                var cam = Camera.main;
                if (cam) thirdPersonCamera = cam.GetComponent<ThirdPersonCamera>();
            }

            if (!thirdPersonCamera) return;

            if (!thirdPersonCamera.target)
                thirdPersonCamera.target = _playerCombatant.transform;

            // Also wire PlayerMotor camera ref if present
            var motor = _playerCombatant.GetComponent<PlayerMotor>();
            if (motor && !motor.cam)
                motor.cam = thirdPersonCamera;
        }

        void EnsureHud()
        {
            if (!_playerCombatant) return;

            // If a PlayerHud already exists and is bound, do nothing.
            var existingHud = FindFirstObjectByType<PlayerHud>();
            if (existingHud && existingHud.player) return;

            GameObject hudGo = null;
            if (hudPrefab)
            {
                hudGo = Instantiate(hudPrefab);
            }
            else
            {
                // Fall back to runtime HUD builder.
                hudGo = new GameObject("UIRoot");
                hudGo.AddComponent<HudAutoBuilder>();
            }

            // Try bind HudAutoBuilder to player.
            var builder = hudGo.GetComponent<HudAutoBuilder>();
            if (builder) builder.player = _playerCombatant;
        }

        void EnsureEnemies()
        {
            if (!enemyPrefab) return;
            if (enemySpawns == null || enemySpawns.Length == 0) return;

            // If any enemies already exist, don't spawn duplicates.
            if (FindFirstObjectByType<AshfallFrontier.AI.EnemyMeleeGrunt>()) return;

            foreach (var sp in enemySpawns)
            {
                if (!sp) continue;
                Instantiate(enemyPrefab, sp.position, sp.rotation);
            }
        }
    }
}
