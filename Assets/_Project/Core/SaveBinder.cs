using System.Linq;
using UnityEngine;
using AshfallFrontier.Items;

namespace AshfallFrontier.Core
{
    /// <summary>
    /// Binds Inventory + Player transform to SaveSystem.
    /// Attach to Player prefab (recommended).
    /// </summary>
    [RequireComponent(typeof(Inventory))]
    public class SaveBinder : MonoBehaviour
    {
        private Inventory _inv;

        void Awake()
        {
            _inv = GetComponent<Inventory>();
        }

        void Start()
        {
            var ss = FindFirstObjectByType<SaveSystem>();
            if (!ss) return;

            ss.LoadInto(this);
        }

        void OnApplicationQuit()
        {
            var ss = FindFirstObjectByType<SaveSystem>();
            if (!ss) return;
            ss.SaveFrom(this);
        }

        public void Apply(SaveData data)
        {
            if (data == null) return;

            // Position
            transform.position = data.PlayerPos();

            // Inventory
            var entries = data.items.Select(r => new InventoryEntry { itemId = r.id, qty = r.qty }).ToList();
            _inv.SetAll(entries);
        }

        public SaveData Extract()
        {
            var data = new SaveData();
            data.SetPlayerPos(transform.position);

            foreach (var e in _inv.entries)
            {
                data.items.Add(new SaveData.ItemRow { id = e.itemId, qty = e.qty });
            }

            return data;
        }
    }
}
