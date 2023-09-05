using Animachine.Behaviours;
using UnityEngine;

namespace Animachine.Runtime.States
{
    public class Chasing : MonoBehaviour, IChasing
    {
        [SerializeField] private float speed;
        
        // Buffer
        private IDetectable _detectable;
        private INavigation _navigation;

        private void Awake()
        {
            _navigation = GetComponentInParent<INavigation>();
            _detectable = GetComponentInParent<IDetectable>();
        }

        public void OnEnter()
        {

        }

        public void OnUpdate()
        {
            if (_detectable.Target)
            {
                var destination = _detectable.Target.position;
                
                _navigation.SetSpeed(speed);
                _navigation.SetDestination(destination);
                _navigation.SetLookDirection(_navigation.Direction);
            }
        }
        public void OnExit()
        {
            _navigation.SetDestination(_navigation.Position);
        }
    }
}