using Animachine.Scripts.Interfaces;
using UnityEngine;

namespace Animachine.Scripts.Components
{
    public class DetectionSystem : MonoBehaviour, IDetectable
    {
        private const float LossTime = 5.0f;

        private Transform _detected;
        private float _lossTimer;

        private void Update()
        {
            if (Target && _detected == null)
            {
                // When the target is out of view, turn on the timer
                _lossTimer += Time.deltaTime;
                if (_lossTimer > LossTime == false) return;
                
                // Remove a target after the "LossTime" has passed
                _lossTimer = 0;
                Target = null;

                return;
            }
            
            _lossTimer = 0;
        }
        
        // IDetectable
        public Transform Target { get; set; }

        public void SetDetected(Transform detected)
        {
            Target = detected == null ? Target : detected;
            _detected = detected;
        }
    }
}