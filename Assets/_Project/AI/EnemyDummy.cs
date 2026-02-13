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
            if (combatant.health && combatant.health.IsDead)
                gameObject.SetActive(false);
        }
    }
}
