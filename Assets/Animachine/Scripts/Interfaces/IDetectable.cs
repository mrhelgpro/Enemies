using UnityEngine;

namespace Animachine.Scripts.Interfaces
{
    public interface IDetectable
    {
        public Transform Target { get; set; }
        public void SetDetected(Transform detected);
    }
}