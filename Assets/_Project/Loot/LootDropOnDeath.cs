using UnityEngine;
using AshfallFrontier.Combat;

namespace AshfallFrontier.Loot
{
    /// <summary>
    /// Attach to enemies. When Health reaches 0 for the first time, spawns a LootPickup.
    /// 
    /// Spawn placement tries to be sane regardless of enemy pivot by using collider bounds,
    /// falling back to transform.position.
    /// </summary>
    [RequireComponent(typeof(Health))]
    public class LootDropOnDeath : MonoBehaviour
    {
        public GameObject lootPickupPrefab;
        public string itemId = "test_item";
        public int qty = 1;

        [Header("Spawn")]
        [Tooltip("Extra offset applied after calculating a ground-ish spawn point.")]
        public Vector3 spawnOffset = new Vector3(0, 0.15f, 0);

        [Tooltip("If true, spawn near collider bottom (feet). If false, use transform.position.")]
        public bool spawnAtColliderBottom = true;

        private Health _health;
        private bool _dropped;

        void Awake()
        {
            _health = GetComponent<Health>();
        }

        void Update()
        {
            if (_dropped) return;
            if (!_health) return;
            if (!_health.IsDead) return;

            _dropped = true;
            SpawnLoot();
        }

        Vector3 ComputeSpawnPoint()
        {
            if (!spawnAtColliderBottom)
                return transform.position;

            // Prefer a collider on this object; else any child collider.
            var col = GetComponent<Collider>();
            if (!col) col = GetComponentInChildren<Collider>();

            if (col)
            {
                var b = col.bounds;
                // Bounds center XZ, bottom Y.
                return new Vector3(b.center.x, b.min.y, b.center.z);
            }

            return transform.position;
        }

        void SpawnLoot()
        {
            if (!lootPickupPrefab)
            {
                Debug.LogWarning("[LootDropOnDeath] Missing lootPickupPrefab.");
                return;
            }

            var pos = ComputeSpawnPoint() + spawnOffset;
            var go = Instantiate(lootPickupPrefab, pos, Quaternion.identity);
            var lp = go.GetComponent<LootPickup>();
            if (lp)
            {
                lp.itemId = itemId;
                lp.qty = qty;
            }
        }
    }
}
