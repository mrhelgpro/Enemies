using Animachine.Scripts.Core;
using Animachine.Scripts.Interfaces;
using UnityEngine;

namespace Animachine.Scripts.States
{
    public class Patrolling : AnimachineState
    {
        [SerializeField] private float speed;
        
        // Buffer
        private float _timeStuck;
        private int _moveDirection;
        private Vector3 _destination;
        private INavigation _navigation;

        // Animachine Methods
        protected override void OnEnter()
        {
            _navigation = ThisTransform.GetComponentInParent<INavigation>();
            _moveDirection = (int)Mathf.Sign(ThisTransform.lossyScale.x);
        }

        protected override void OnUpdate()
        {
            FreeDirectionCheck();
            SetDestination();
        }

        protected override void OnExit()
        {
            _navigation.SetDestination(_navigation.Position);
        }

        // Patrol Methods
        private void FreeDirectionCheck()
        {
            // Setting the destination
            var horizontal = _navigation.Position.x + _moveDirection;
            var vertical = _navigation.Position.y;
            _destination = new Vector3(horizontal, vertical, 0);
            
            // Check for a stuck 
            if (Mathf.Approximately(_navigation.Velocity.x, 0) == false) return;

            // Duration of stuck time
            _timeStuck += Time.deltaTime;
            if (_timeStuck > 0.1f == false) return;

            // Change of direction
            _moveDirection *= -1;
            _timeStuck = 0;
        }

        private void SetDestination()
        {
            _navigation.SetSpeed(speed);
            _navigation.SetDestination(_destination);
            _navigation.SetLookDirection(_navigation.Direction);
        }
    }
}