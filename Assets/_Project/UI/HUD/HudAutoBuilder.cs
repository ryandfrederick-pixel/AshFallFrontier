using UnityEngine;
using UnityEngine.UI;
using AshfallFrontier.Combat;

namespace AshfallFrontier.UI
{
    /// <summary>
    /// M1 convenience: creates a simple Canvas + three Slider bars at runtime.
    /// Drop this on any GameObject in the scene and assign the Player Combatant.
    /// </summary>
    public class HudAutoBuilder : MonoBehaviour
    {
        public Combatant player;

        [Header("Layout")]
        public Vector2 start = new Vector2(20, 20);
        public Vector2 size = new Vector2(260, 18);
        public float gap = 10f;

        void Start()
        {
            if (!player)
            {
                var p = GameObject.FindWithTag("Player");
                if (p) player = p.GetComponent<Combatant>();
            }
            if (!player) player = FindFirstObjectByType<Combatant>();

            var canvasGo = new GameObject("HUD");
            var canvas = canvasGo.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            canvasGo.AddComponent<CanvasScaler>().uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            canvasGo.AddComponent<GraphicRaycaster>();

            // Background panel (optional)
            var panelGo = new GameObject("Panel");
            panelGo.transform.SetParent(canvasGo.transform, false);
            var panel = panelGo.AddComponent<Image>();
            panel.color = new Color(0, 0, 0, 0.35f);
            var prt = panelGo.GetComponent<RectTransform>();
            prt.anchorMin = new Vector2(0, 1);
            prt.anchorMax = new Vector2(0, 1);
            prt.pivot = new Vector2(0, 1);
            prt.anchoredPosition = start + new Vector2(-10, 10);
            prt.sizeDelta = new Vector2(size.x + 20, (size.y + gap) * 3 + 20);

            var hp = MakeBar(panelGo.transform, "HP", start + new Vector2(0, 0), new Color(0.85f, 0.2f, 0.2f));
            var st = MakeBar(panelGo.transform, "Stamina", start + new Vector2(0, -(size.y + gap)), new Color(0.2f, 0.85f, 0.2f));
            var ma = MakeBar(panelGo.transform, "Mana", start + new Vector2(0, -2 * (size.y + gap)), new Color(0.25f, 0.45f, 0.95f));

            var hud = canvasGo.AddComponent<PlayerHud>();
            hud.player = player;
            hud.hp = hp;
            hud.stamina = st;
            hud.mana = ma;
        }

        Slider MakeBar(Transform parent, string label, Vector2 topLeft, Color fillColor)
        {
            // Root
            var root = new GameObject(label);
            root.transform.SetParent(parent, false);

            var rt = root.AddComponent<RectTransform>();
            rt.anchorMin = new Vector2(0, 1);
            rt.anchorMax = new Vector2(0, 1);
            rt.pivot = new Vector2(0, 1);
            rt.anchoredPosition = topLeft;
            rt.sizeDelta = size;

            // Background
            var bg = root.AddComponent<Image>();
            bg.color = new Color(1, 1, 1, 0.12f);

            // Slider
            var slider = root.AddComponent<Slider>();
            slider.direction = Slider.Direction.LeftToRight;
            slider.transition = Selectable.Transition.None;
            slider.minValue = 0;
            slider.maxValue = 1;
            slider.value = 1;

            // Fill Area (Unity slider expects a nested hierarchy)
            var fillArea = new GameObject("Fill Area");
            fillArea.transform.SetParent(root.transform, false);
            var fillAreaRt = fillArea.AddComponent<RectTransform>();
            fillAreaRt.anchorMin = new Vector2(0, 0);
            fillAreaRt.anchorMax = new Vector2(1, 1);
            fillAreaRt.offsetMin = new Vector2(2, 2);
            fillAreaRt.offsetMax = new Vector2(-2, -2);

            var fill = new GameObject("Fill");
            fill.transform.SetParent(fillArea.transform, false);
            var fillRt = fill.AddComponent<RectTransform>();
            fillRt.anchorMin = new Vector2(0, 0);
            fillRt.anchorMax = new Vector2(1, 1);
            fillRt.offsetMin = Vector2.zero;
            fillRt.offsetMax = Vector2.zero;

            var fillImg = fill.AddComponent<Image>();
            fillImg.color = fillColor;

            slider.targetGraphic = bg;
            slider.fillRect = fillRt;

            // No handle for now.
            return slider;
        }
    }
}
