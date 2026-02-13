using UnityEngine;

namespace AshfallFrontier.Combat
{
    public class Health : MonoBehaviour
    {
        public float maxHp = 100f;
        public float hp = 100f;

        public bool IsDead => hp <= 0f;

        void Awake()
        {
            hp = Mathf.Clamp(hp, 0f, maxHp);
        }

        public void Heal(float amount)
        {
            if (amount <= 0f) return;
            hp = Mathf.Min(maxHp, hp + amount);
        }

        public void Damage(float amount)
        {
            if (amount <= 0f) return;
            hp = Mathf.Max(0f, hp - amount);
        }
    }
}
