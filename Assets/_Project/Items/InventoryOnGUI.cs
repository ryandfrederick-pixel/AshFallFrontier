using UnityEngine;

namespace AshfallFrontier.Items
{
    /// <summary>
    /// Minimal inventory UI for MVP testing.
    /// Toggle with I.
    /// </summary>
    [RequireComponent(typeof(Inventory))]
    public class InventoryOnGUI : MonoBehaviour
    {
        public KeyCode toggleKey = KeyCode.I;
        public bool open;

        private Inventory _inv;

        void Awake()
        {
            _inv = GetComponent<Inventory>();
        }

        void Update()
        {
            if (Input.GetKeyDown(toggleKey))
                open = !open;
        }

        void OnGUI()
        {
            if (!open) return;
            if (_inv == null) return;

            const int w = 320;
            const int h = 260;
            var r = new Rect(20, 120, w, h);
            GUI.Box(r, "Inventory");

            GUILayout.BeginArea(new Rect(r.x + 10, r.y + 25, r.width - 20, r.height - 35));
            if (_inv.entries.Count == 0)
            {
                GUILayout.Label("(empty)");
            }
            else
            {
                foreach (var e in _inv.entries)
                {
                    var def = ItemDatabase.Get(e.itemId);
                    string name = def ? def.displayName : e.itemId;
                    GUILayout.Label($"{name} x{e.qty}");
                }
            }
            GUILayout.EndArea();
        }
    }
}
