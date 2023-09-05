using Animachine.Behaviours;
using UnityEngine;

namespace Animachine.Runtime.States
{
    public class Patrolling : MonoBehaviour, IPatrolling
    {
        [SerializeField] private float speed;
        
        // Buffer
        private float _timeStuck;
        private int _moveDirection;
        private Vector3 _destination;
        private INavigation _navigation;

        private void Awake()
        {
            _navigation = GetComponentInParent<INavigation>();
        }

        // Animachine Methods
        public void OnEnter()
        {
            _moveDirection = (int)Mathf.Sign(transform.lossyScale.x);
        }

        public void OnUpdate()
        {
            FreeDirectionCheck();
            SetDestination();
        }

        public void OnExit()
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