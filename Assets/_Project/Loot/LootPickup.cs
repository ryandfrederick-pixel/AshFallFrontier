using UnityEngine;
using AshfallFrontier.Items;

namespace AshfallFrontier.Loot
{
    /// <summary>
    /// World pickup: player presses E while in trigger to add to Inventory.
    /// </summary>
    [RequireComponent(typeof(Collider))]
    public class LootPickup : MonoBehaviour
    {
        public string itemId;
        public int qty = 1;

        public KeyCode pickupKey = KeyCode.E;
        public bool destroyOnPickup = true;

        void Reset()
        {
            var c = GetComponent<Collider>();
            if (c) c.isTrigger = true;
        }

        void OnTriggerStay(Collider other)
        {
            if (!other.CompareTag("Player")) return;

            if (Input.GetKeyDown(pickupKey))
            {
                var inv = other.GetComponentInParent<Inventory>();
                if (!inv)
                {
                    Debug.LogWarning("[LootPickup] Player has no Inventory component.");
                    return;
                }

                inv.Add(itemId, qty);
                Debug.Log($"[LootPickup] Picked up {itemId} x{qty}");

                if (destroyOnPickup)
                    Destroy(gameObject);
            }
        }

        void OnGUI()
        {
            // MVP: no world-space prompt. We rely on testing discipline / debug logs for now.
        }
    }
}
