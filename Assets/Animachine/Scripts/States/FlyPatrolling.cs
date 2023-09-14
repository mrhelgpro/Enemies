using Animachine.Scripts.Core;
using Animachine.Scripts.Interfaces;
using UnityEngine;

namespace Animachine.Scripts.States
{
    public class FlyPatrolling : AnimachineState
    {
        [SerializeField] private float moveSpeed = 100;
        [SerializeField] private int radiusPatrolling = 10;
        
        // Buffer
        private bool _isInitiated;
        private Vector3 _startPosition;
        private Vector3 _destination;
        private float _timeStuck;
        private int _moveDirection;

        private INavigation _navigation;

        // Animachine Methods
        protected override void OnEnter()
        {
            if (_isInitiated == false)
            {
                _startPosition = ThisTransform.position;
                _navigation = ThisTransform.GetComponentInParent<INavigation>();

                _isInitiated = true;
            }
            
            SetRandomDestination();
        }

        protected override void OnUpdate()
        {
            Patrolling();
            SetDestination();
        }

        protected override void OnExit()
        {
            _navigation.SetDestination(_navigation.Position);
        }

        // Fly Patrol
        private void Patrolling() // IT NEEDS TO BE FIXED!
        {
            // Setting the destination
            if (_navigation.IsDestination)
            {
                SetRandomDestination();
            }
            
            // Check for a stuck 
            if (Mathf.Approximately(_navigation.Velocity.x, 0) == false) return;

            // Duration of stuck time
            _timeStuck += Time.deltaTime;
            if (_timeStuck > 0.1f == false) return;

            // Change of direction
            SetRandomDestination();
            _timeStuck = 0;
        }

        private void SetRandomDestination()
        {
            var randomDirection = Random.insideUnitCircle;
            var randomPosition = new Vector3(randomDirection.x, randomDirection.y, 0) * radiusPatrolling; 
            _destination = _startPosition + randomPosition; 
        }

        private void SetDestination()
        {
            _navigation.SetSpeed(moveSpeed);
            _navigation.SetDestination(_destination);
            _navigation.SetLookDirection(_navigation.Direction);
        }
    }
}