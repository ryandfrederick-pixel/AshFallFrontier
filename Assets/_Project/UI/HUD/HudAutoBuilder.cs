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
            var go = new GameObject(label);
            go.transform.SetParent(parent, false);

            var rt = go.AddComponent<RectTransform>();
            rt.anchorMin = new Vector2(0, 1);
            rt.anchorMax = new Vector2(0, 1);
            rt.pivot = new Vector2(0, 1);
            rt.anchoredPosition = topLeft;
            rt.sizeDelta = size;

            var bg = go.AddComponent<Image>();
            bg.color = new Color(1, 1, 1, 0.12f);

            var slider = go.AddComponent<Slider>();
            slider.direction = Slider.Direction.LeftToRight;
            slider.transition = Selectable.Transition.None;

            // Fill
            var fillGo = new GameObject("Fill");
            fillGo.transform.SetParent(go.transform, false);
            var fillRt = fillGo.AddComponent<RectTransform>();
            fillRt.anchorMin = new Vector2(0, 0);
            fillRt.anchorMax = new Vector2(1, 1);
            fillRt.offsetMin = new Vector2(2, 2);
            fillRt.offsetMax = new Vector2(-2, -2);
            var fillImg = fillGo.AddComponent<Image>();
            fillImg.color = fillColor;

            slider.targetGraphic = bg;
            slider.fillRect = fillRt;

            // Label text removed for Unity 6 default-font compatibility (keeps Console clean).
            return slider;
        }
    }
}
