using UnityEngine;

namespace AshfallFrontier.Combat
{
    /// <summary>
    /// Tiny debug helper: draws current HP over an actor in screen space.
    /// Attach to enemies while prototyping.
    /// </summary>
    [RequireComponent(typeof(Health))]
    public class HealthDebugLabel : MonoBehaviour
    {
        public Vector3 worldOffset = new Vector3(0, 2.0f, 0);
        public Color color = Color.red;
        public bool hideWhenDead = true;

        private Health _health;

        void Awake()
        {
            _health = GetComponent<Health>();
        }

        void OnGUI()
        {
            if (_health == null) return;
            if (hideWhenDead && _health.IsDead) return;

            var cam = Camera.main;
            if (!cam) return;

            Vector3 world = transform.position + worldOffset;
            Vector3 screen = cam.WorldToScreenPoint(world);
            if (screen.z < 0.01f) return;

            // Unity GUI space origin is top-left; screen point is bottom-left.
            float x = screen.x;
            float y = Screen.height - screen.y;

            var style = new GUIStyle(GUI.skin.label);
            style.normal.textColor = color;
            style.fontStyle = FontStyle.Bold;

            GUI.Label(new Rect(x - 40, y - 10, 120, 20), $"HP: {(int)_health.hp}/{(int)_health.maxHp}", style);
        }
    }
}
