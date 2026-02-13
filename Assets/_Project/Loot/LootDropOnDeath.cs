using UnityEngine;
using AshfallFrontier.Combat;

namespace AshfallFrontier.Loot
{
    /// <summary>
    /// Attach to enemies. When Health reaches 0 for the first time, spawns a LootPickup.
    /// </summary>
    [RequireComponent(typeof(Health))]
    public class LootDropOnDeath : MonoBehaviour
    {
        public GameObject lootPickupPrefab;
        public string itemId = "test_item";
        public int qty = 1;
        public Vector3 spawnOffset = new Vector3(0, 0.2f, 0);

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

        void SpawnLoot()
        {
            if (!lootPickupPrefab)
            {
                Debug.LogWarning("[LootDropOnDeath] Missing lootPickupPrefab.");
                return;
            }

            var go = Instantiate(lootPickupPrefab, transform.position + spawnOffset, Quaternion.identity);
            var lp = go.GetComponent<LootPickup>();
            if (lp)
            {
                lp.itemId = itemId;
                lp.qty = qty;
            }
        }
    }
}
