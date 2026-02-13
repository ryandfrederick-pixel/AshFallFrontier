using UnityEngine;
using UnityEngine.AI;
using AshfallFrontier.Combat;

namespace AshfallFrontier.AI
{
    /// <summary>
    /// M1 melee grunt:
    /// - Aggro radius
    /// - Chase via NavMeshAgent
    /// - Telegraph windup
    /// - Melee hit via overlap sphere
    ///
    /// Requires: NavMeshAgent + Combatant (+ Health)
    /// Scene needs baked NavMesh (Window -> AI -> Navigation / or AI NavMesh Components in newer versions).
    /// </summary>
    [RequireComponent(typeof(NavMeshAgent))]
    [RequireComponent(typeof(Combatant))]
    public class EnemyMeleeGrunt : MonoBehaviour
    {
        [Header("Target")]
        public Transform target;

        [Header("Aggro")]
        public float aggroRange = 12f;
        public float disengageRange = 18f;

        [Header("Attack")]
        public float attackRange = 1.9f;
        public float windupSeconds = 0.55f;
        public float cooldownSeconds = 1.25f;
        public float damage = 10f;
        public float hitRadius = 1.0f;
        public LayerMask hitMask;

        [Header("Telegraph (visual)")]
        public Color telegraphColor = new Color(1f, 0.4f, 0.1f);

        private NavMeshAgent _agent;
        private Combatant _combatant;
        private float _nextAttackTime;
        private bool _hasAggro;
        private Renderer _rend;
        private Color _baseColor;

        void Awake()
        {
            _agent = GetComponent<NavMeshAgent>();
            _combatant = GetComponent<Combatant>();
            _rend = GetComponentInChildren<Renderer>();
            if (_rend) _baseColor = _rend.material.color;

            if (!_combatant.health) _combatant.health = GetComponent<Health>();
        }

        void Start()
        {
            if (!target)
            {
                var p = GameObject.FindWithTag("Player");
                if (p) target = p.transform;
            }
        }

        void Update()
        {
            if (!_combatant || (_combatant.health && _combatant.health.IsDead))
            {
                _agent.isStopped = true;
                return;
            }

            if (!target) return;

            float d = Vector3.Distance(transform.position, target.position);

            if (!_hasAggro)
            {
                if (d <= aggroRange) _hasAggro = true;
                else return;
            }
            else
            {
                if (d >= disengageRange)
                {
                    _hasAggro = false;
                    _agent.isStopped = true;
                    return;
                }
            }

            // Face target a bit
            var to = target.position - transform.position;
            to.y = 0;
            if (to.sqrMagnitude > 0.01f)
            {
                var desired = Quaternion.LookRotation(to.normalized, Vector3.up);
                transform.rotation = Quaternion.Slerp(transform.rotation, desired, 1f - Mathf.Exp(-10f * Time.deltaTime));
            }

            bool inRange = d <= attackRange;

            if (!inRange)
            {
                _agent.isStopped = false;
                _agent.SetDestination(target.position);
                return;
            }

            _agent.isStopped = true;

            if (Time.time < _nextAttackTime) return;

            // Attack
            _nextAttackTime = Time.time + cooldownSeconds;
            StartCoroutine(AttackRoutine());
        }

        System.Collections.IEnumerator AttackRoutine()
        {
            // Telegraph: tint
            if (_rend) _rend.material.color = telegraphColor;

            float t = 0f;
            while (t < windupSeconds)
            {
                t += Time.deltaTime;
                yield return null;
            }

            if (_rend) _rend.material.color = _baseColor;

            // Damage sphere in front
            Vector3 center = transform.position + transform.forward * 1.0f + Vector3.up * 0.9f;
            var hits = Physics.OverlapSphere(center, hitRadius, hitMask, QueryTriggerInteraction.Ignore);
            foreach (var h in hits)
            {
                var c = h.GetComponentInParent<Combatant>();
                if (c != null && c != _combatant)
                {
                    c.TakeDamage(damage);
                }
            }
        }

        void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, aggroRange);
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, attackRange);
        }
    }
}
