using UnityEngine;

namespace Animachine.Scripts.Interfaces
{
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
}