using UnityEngine;
using UnityEngine.UI;
using AshfallFrontier.Combat;

namespace AshfallFrontier.UI
{
    /// <summary>
    /// Minimal HUD (M1): HP/Stamina/Mana bars.
    /// Expects references to UI Sliders.
    /// </summary>
    public class PlayerHud : MonoBehaviour
    {
        public Combatant player;

        [Header("Bars")]
        public Slider hp;
        public Slider stamina;
        public Slider mana;

        void Update()
        {
            if (!player) return;
            if (!player.health) return;

            if (hp)
            {
                hp.maxValue = player.health.maxHp;
                hp.value = player.health.hp;
            }

            if (stamina)
            {
                stamina.maxValue = player.stamina.max;
                stamina.value = player.stamina.current;
            }

            if (mana)
            {
                mana.maxValue = player.mana.max;
                mana.value = player.mana.current;
            }
        }
    }
}
