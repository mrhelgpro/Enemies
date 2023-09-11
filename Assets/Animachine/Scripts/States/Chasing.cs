using Animachine.Scripts.Core;
using Animachine.Scripts.Interfaces;
using UnityEngine;

namespace Animachine.Scripts.States
{
    public class Chasing : AnimachineState
    {
        [SerializeField] private float speed;
        
        // Buffer
        private IDetectable _detectable;
        private INavigation _navigation;

        protected override void OnEnter()
        {
            _navigation = ThisTransform.GetComponentInParent<INavigation>();
            _detectable = ThisTransform.GetComponentInParent<IDetectable>();
        }

        protected override void OnUpdate()
        {
            if (_detectable.Target)
            {
                var destination = _detectable.Target.position;
                
                _navigation.SetSpeed(speed);
                _navigation.SetDestination(destination);
                _navigation.SetLookDirection(_navigation.Direction);
            }
        }
        
        protected override void OnExit()
        {
            _navigation.SetDestination(_navigation.Position);
        }
    }
}