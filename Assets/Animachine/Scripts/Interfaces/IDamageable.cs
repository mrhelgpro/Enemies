using UnityEngine;

namespace Animachine.Scripts.Interfaces
{
    public interface IDamageable
    {
        public bool IsDamage { get; }
        public void TakeDamage(Transform dealer, float value, float shockTime = 0.25f);
    }
}
