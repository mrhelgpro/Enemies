using UnityEngine;

namespace Animachine.Runtime.Core
{
    public class DetectorCollider2D : MonoBehaviour
    {
        private IDetectable _detectable;

        private void Awake()
        {
            _detectable = GetComponentInParent<IDetectable>();
        }
        
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.transform.CompareTag("Player"))
            {
                _detectable.SetDetected(other.transform);
            }
        }
        
        private void OnTriggerExit2D(Collider2D other)
        {
            if (_detectable.Target == other.transform)
            {
                _detectable.SetDetected(null);
            }
        }
    }
}