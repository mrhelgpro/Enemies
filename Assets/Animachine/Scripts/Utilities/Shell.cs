using UnityEngine;

namespace Animachine.Scripts.Utilities
{
    public class Shell : MonoBehaviour
    {
        [SerializeField] private float speed;
        [SerializeField] private float lifeTime;
        private Vector3 _direction;
        private Transform _thisTransform;
        
        public void Construct(Transform target)
        {
            _thisTransform = transform;
            _direction = (target.position - _thisTransform.position).normalized;
            
            Destroy(gameObject, lifeTime);
        }

        private void Update()
        {
            _thisTransform.Translate(_direction * (speed * Time.deltaTime));
        }
    }
}