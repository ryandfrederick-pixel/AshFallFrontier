using System;
using UnityEngine;

namespace AshfallFrontier.Combat
{
    [Serializable]
    public class ResourcePool
    {
        public float max = 100f;
        public float current = 100f;

        [Header("Regen")]
        public float regenPerSecond = 10f;
        public float regenDelaySeconds = 1.25f;

        private float _lastSpendTime;

        public void InitFull()
        {
            current = max;
            _lastSpendTime = -999f;
        }

        public void Tick(float dt)
        {
            if (Time.time - _lastSpendTime < regenDelaySeconds) return;
            if (regenPerSecond <= 0f) return;
            current = Mathf.Min(max, current + regenPerSecond * dt);
        }

        public bool CanSpend(float amount) => current >= amount;

        public bool Spend(float amount)
        {
            if (amount <= 0f) return true;
            if (current < amount) return false;
            current -= amount;
            _lastSpendTime = Time.time;
            return true;
        }

        public void Add(float amount)
        {
            if (amount <= 0f) return;
            current = Mathf.Min(max, current + amount);
        }

        public float Normalized() => max <= 0f ? 0f : Mathf.Clamp01(current / max);
    }
}
