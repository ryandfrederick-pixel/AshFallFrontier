using System;
using System.Collections.Generic;
using UnityEngine;

namespace AshfallFrontier.Items
{
    [Serializable]
    public class InventoryEntry
    {
        public string itemId;
        public int qty;
    }

    /// <summary>
    /// Minimal inventory: list of itemId+qty. Works with ItemDatabase for definitions.
    /// </summary>
    public class Inventory : MonoBehaviour
    {
        public List<InventoryEntry> entries = new();

        public event Action OnChanged;

        public int GetQuantity(string itemId)
        {
            foreach (var e in entries)
                if (e.itemId == itemId) return e.qty;
            return 0;
        }

        public void Add(string itemId, int qty)
        {
            if (string.IsNullOrWhiteSpace(itemId) || qty <= 0) return;

            for (int i = 0; i < entries.Count; i++)
            {
                if (entries[i].itemId == itemId)
                {
                    entries[i].qty += qty;
                    OnChanged?.Invoke();
                    return;
                }
            }

            entries.Add(new InventoryEntry { itemId = itemId, qty = qty });
            OnChanged?.Invoke();
        }

        public bool Remove(string itemId, int qty)
        {
            if (string.IsNullOrWhiteSpace(itemId) || qty <= 0) return false;

            for (int i = 0; i < entries.Count; i++)
            {
                if (entries[i].itemId != itemId) continue;
                if (entries[i].qty < qty) return false;

                entries[i].qty -= qty;
                if (entries[i].qty <= 0)
                    entries.RemoveAt(i);

                OnChanged?.Invoke();
                return true;
            }

            return false;
        }

        public void SetAll(List<InventoryEntry> newEntries)
        {
            entries = newEntries ?? new();
            OnChanged?.Invoke();
        }
    }
}
