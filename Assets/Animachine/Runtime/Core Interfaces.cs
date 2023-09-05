using UnityEngine;

namespace Animachine.Runtime
{
    public interface IDamageable
    {
        public bool IsDamage { get; }
        public void TakeDamage(Transform dealer, float value, float shockTime = 0.25f);
    }
    
    public interface INavigation
    {
        public Vector3 Position { get; }
        public Vector3 Velocity { get; }
        public bool IsGrounded { get; }
        public bool IsDestination { get; }
        public Vector3 Direction { get; }
        public float Distance { get; }
        public void SetSpeed(float value);
        public void SetDestination(Vector3 vector);
        public void SetLookDirection(Vector3 vector);
    }
    
    public interface IDetectable
    {
        public Transform Target { get; set; }
        public void SetDetected(Transform detected);
    }
}