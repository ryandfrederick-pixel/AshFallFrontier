using UnityEngine;

namespace AshfallFrontier.Combat
{
    /// <summary>
    /// Very simple overlap hitbox for M1.
    /// Attach to an empty child transform in front of the player or enemy.
    /// </summary>
    public class MeleeHitbox : MonoBehaviour
    {
        public float radius = 0.9f;
        public float damage = 12f;
        public LayerMask hitMask;
        public bool debugDraw = true;

        public void Fire()
        {
            // Prevent self-hits by ignoring Combatant on the same root.
            var owner = GetComponentInParent<Combatant>();

            var hits = Physics.OverlapSphere(transform.position, radius, hitMask, QueryTriggerInteraction.Ignore);
            foreach (var h in hits)
            {
                var c = h.GetComponentInParent<Combatant>();
                if (c == null) continue;
                if (owner != null && c == owner) continue;

                c.TakeDamage(damage);
            }
        }

        void OnDrawGizmosSelected()
        {
            if (!debugDraw) return;
            Gizmos.color = new Color(1f, 0.2f, 0.2f, 0.4f);
            Gizmos.DrawWireSphere(transform.position, radius);
        }
    }
}
