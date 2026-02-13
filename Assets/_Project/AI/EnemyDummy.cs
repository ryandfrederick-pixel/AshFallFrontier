using UnityEngine;
using AshfallFrontier.Combat;

namespace AshfallFrontier.AI
{
    /// <summary>
    /// M1 test enemy: just stands there and can be damaged.
    /// </summary>
    [RequireComponent(typeof(Combatant))]
    public class EnemyDummy : MonoBehaviour
    {
        public Combatant combatant;

        void Awake()
        {
            if (!combatant) combatant = GetComponent<Combatant>();
            if (!combatant.health) combatant.health = GetComponent<Health>();
        }

        void Update()
        {
            // For M1 setup/testing, don't auto-disable the object; just freeze it visually.
            if (combatant.health && combatant.health.IsDead)
            {
                var r = GetComponentInChildren<Renderer>();
                if (r) r.material.color = new Color(0.25f, 0.25f, 0.25f);
                enabled = false;
            }
        }
    }
}
