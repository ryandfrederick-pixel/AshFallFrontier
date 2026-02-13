using System.Collections;
using UnityEngine;

namespace AshfallFrontier.Combat
{
    /// <summary>
    /// M1 combat loop: light/heavy melee placeholder + dodge + block.
    /// No animations yet; uses timers and a simple hitbox.
    /// </summary>
    [RequireComponent(typeof(Combatant))]
    public class PlayerCombat : MonoBehaviour
    {
        [Header("Refs")]
        public Combatant combatant;
        public CharacterController controller;
        public MeleeHitbox hitbox;

        [Header("Costs")]
        public float dodgeStaminaCost = 22f;
        public float lightStaminaCost = 8f;
        public float heavyStaminaCost = 14f;

        [Header("Timings")]
        public float lightWindup = 0.12f;
        public float heavyWindup = 0.28f;
        public float attackLockSeconds = 0.35f;
        public float dodgeDuration = 0.28f;
        public float dodgeSpeed = 9.5f;

        [Header("Controls")]
        public KeyCode blockKey = KeyCode.LeftShift;
        public KeyCode dodgeKey = KeyCode.Space;

        private bool _locked;
        private bool _dodging;

        void Awake()
        {
            if (!combatant) combatant = GetComponent<Combatant>();
            if (!controller) controller = GetComponent<CharacterController>();
        }

        void Update()
        {
            if (!combatant || !controller) return;

            // Block
            combatant.isBlocking = Input.GetKey(blockKey) && !_dodging;

            if (_locked) return;

            // Dodge
            if (!_dodging && Input.GetKeyDown(dodgeKey))
            {
                if (combatant.stamina.Spend(dodgeStaminaCost))
                    StartCoroutine(Dodge());
            }

            // Attacks (placeholder)
            if (!_dodging && Input.GetMouseButtonDown(0))
            {
                if (combatant.stamina.Spend(lightStaminaCost))
                    StartCoroutine(Attack(lightWindup));
            }
            if (!_dodging && Input.GetMouseButtonDown(1))
            {
                // Heavy attack (RMB).
                if (combatant.stamina.Spend(heavyStaminaCost))
                    StartCoroutine(Attack(heavyWindup, heavy: true));
            }
        }

        IEnumerator Attack(float windup, bool heavy = false)
        {
            _locked = true;

            // Windup
            yield return new WaitForSeconds(windup);

            if (hitbox)
            {
                hitbox.damage = heavy ? hitbox.damage * 1.6f : hitbox.damage;
                hitbox.Fire();
            }

            yield return new WaitForSeconds(Mathf.Max(0f, attackLockSeconds - windup));
            _locked = false;
        }

        IEnumerator Dodge()
        {
            _dodging = true;
            combatant.isBlocking = false;

            float t = 0f;
            Vector3 dir = transform.forward;

            while (t < dodgeDuration)
            {
                // Simple forward dodge. Later: allow input direction + i-frames + cancel windows.
                var delta = dir * dodgeSpeed * Time.deltaTime;
                controller.Move(delta);
                t += Time.deltaTime;
                yield return null;
            }

            _dodging = false;
        }
    }
}
