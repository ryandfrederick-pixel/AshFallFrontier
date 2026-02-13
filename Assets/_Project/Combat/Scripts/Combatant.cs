using UnityEngine;

namespace AshfallFrontier.Combat
{
    /// <summary>
    /// Holds basic combat resources and state flags.
    /// This is intentionally simple for M1; we’ll split into modules later.
    /// </summary>
    public class Combatant : MonoBehaviour
    {
        [Header("Health")]
        public Health health;

        [Header("Resources")]
        public ResourcePool stamina = new ResourcePool { max = 100f, current = 100f, regenPerSecond = 20f, regenDelaySeconds = 1.0f };
        public ResourcePool mana = new ResourcePool { max = 60f, current = 60f, regenPerSecond = 10f, regenDelaySeconds = 1.25f };

        [Header("Defense")]
        public bool isBlocking;
        [Range(0f, 1f)] public float blockDamageMultiplier = 0.35f;
        public float blockStaminaPerDamage = 0.6f;

        void Awake()
        {
            if (!health) health = GetComponent<Health>();
            stamina.InitFull();
            mana.InitFull();
        }

        void Update()
        {
            float dt = Time.deltaTime;
            stamina.Tick(dt);
            mana.Tick(dt);
        }

        public void TakeDamage(float amount)
        {
            if (!health || health.IsDead) return;

            float final = amount;
            if (isBlocking && stamina.current > 0f)
            {
                final *= blockDamageMultiplier;
                // Drain stamina proportional to prevented damage.
                float prevented = amount - final;
                stamina.Spend(prevented * blockStaminaPerDamage);
            }

            health.Damage(final);
        }
    }
}
