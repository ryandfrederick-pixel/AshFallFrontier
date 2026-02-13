using System.Collections.Generic;
using UnityEngine;

namespace AshfallFrontier.Items
{
    /// <summary>
    /// Simple runtime database that loads ItemDefinitions from Resources/Items.
    /// MVP-friendly: avoids a central registry asset.
    /// </summary>
    public static class ItemDatabase
    {
        private static bool _loaded;
        private static readonly Dictionary<string, ItemDefinition> _byId = new();

        public static void EnsureLoaded()
        {
            if (_loaded) return;
            _loaded = true;

            _byId.Clear();
            var defs = Resources.LoadAll<ItemDefinition>("Items");
            foreach (var d in defs)
            {
                if (!d) continue;
                if (string.IsNullOrWhiteSpace(d.id))
                {
                    Debug.LogWarning($"[ItemDatabase] ItemDefinition '{d.name}' missing id. It will not be loadable by save files.");
                    continue;
                }

                if (_byId.ContainsKey(d.id))
                {
                    Debug.LogWarning($"[ItemDatabase] Duplicate item id '{d.id}' (asset '{d.name}'). Keeping first, ignoring this one.");
                    continue;
                }

                _byId.Add(d.id, d);
            }
        }

        public static ItemDefinition Get(string id)
        {
            EnsureLoaded();
            if (string.IsNullOrWhiteSpace(id)) return null;
            _byId.TryGetValue(id, out var d);
            return d;
        }
    }
}
