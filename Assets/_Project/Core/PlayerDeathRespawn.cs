using System.Collections;
using UnityEngine;
using AshfallFrontier.Combat;

namespace AshfallFrontier.Core
{
    /// <summary>
    /// MVP death loop:
    /// - When Health hits 0, disables movement/combat
    /// - Waits respawnDelaySeconds
    /// - Teleports to RespawnPoint (or origin)
    /// - Restores HP + resources
    /// - Saves
    /// 
    /// Attach to the Player GameObject.
    /// </summary>
    [RequireComponent(typeof(Health))]
    public class PlayerDeathRespawn : MonoBehaviour
    {
        public float respawnDelaySeconds = 2.0f;
        public bool saveAfterRespawn = true;

        private Health _health;
        private Combatant _combatant;
        private PlayerMotor _motor;
        private PlayerCombat _combat;
        private CharacterController _cc;

        private bool _deadHandled;

        void Awake()
        {
            _health = GetComponent<Health>();
            _combatant = GetComponent<Combatant>();
            _motor = GetComponent<PlayerMotor>();
            _combat = GetComponent<PlayerCombat>();
            _cc = GetComponent<CharacterController>();
        }

        void Update()
        {
            if (_deadHandled) return;
            if (_health == null) return;
            if (!_health.IsDead) return;

            _deadHandled = true;
            StartCoroutine(RespawnRoutine());
        }

        IEnumerator RespawnRoutine()
        {
            // Disable controls
            if (_motor) _motor.enabled = false;
            if (_combat) _combat.enabled = false;

            yield return new WaitForSeconds(respawnDelaySeconds);

            // Find respawn
            var rp = RespawnPoint.FindBest();
            Vector3 pos = rp ? rp.transform.position : Vector3.zero;
            Quaternion rot = rp ? rp.transform.rotation : Quaternion.identity;

            // Teleport (CharacterController-safe)
            if (_cc)
            {
                _cc.enabled = false;
                transform.SetPositionAndRotation(pos, rot);
                _cc.enabled = true;
            }
            else
            {
                transform.SetPositionAndRotation(pos, rot);
            }

            // Restore stats
            _health.hp = _health.maxHp;
            if (_combatant != null)
            {
                _combatant.isBlocking = false;
                _combatant.stamina.InitFull();
                _combatant.mana.InitFull();
            }

            // Re-enable controls
            if (_motor) _motor.enabled = true;
            if (_combat) _combat.enabled = true;

            _deadHandled = false;

            if (saveAfterRespawn)
            {
                var ss = FindFirstObjectByType<SaveSystem>();
                var binder = GetComponent<SaveBinder>();
                if (ss && binder)
                    ss.SaveFrom(binder);
            }
        }
    }
}
