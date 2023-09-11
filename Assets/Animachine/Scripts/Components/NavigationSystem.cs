using Animachine.Scripts.Interfaces;
using UnityEngine;

namespace Animachine.Scripts.Components
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class NavigationSystem : MonoBehaviour, INavigation
    {
        // Set Movable
        private float _speed;
        private Vector3 _destination;

        // Buffer
        private LayerMask _groundLayer;
        private Transform _thisTransform;
        private Rigidbody2D _rigidbody;

        // Unity Methods
        private void Awake()
        {
            _thisTransform = transform;
            _rigidbody = GetComponent<Rigidbody2D>();
            _groundLayer = LayerMask.GetMask("Default");
        }

        private void FixedUpdate()
        {
            ParametersCheck();
            MovementCheck();
        }

        // INavigation
        public Vector3 Position => _thisTransform.position;
        public Vector3 Velocity => _rigidbody.velocity;
        public Vector3 Direction => IsDestination ? new Vector3(Mathf.Sign(_thisTransform.localScale.x),0,0) : (_destination - Position).normalized;
        public bool IsGrounded { get; private set; }
        public bool IsDestination => Distance < 0.25f;
        public float Distance { get; private set; }
        public void SetSpeed(float value) => _speed = value;
        public void SetDestination(Vector3 vector) => _destination = vector;
        public void SetLookDirection(Vector3 vector)
        {
            var horizontal = vector.x == 0 ? 0 : Mathf.Sign(vector.x);

            switch (_thisTransform.localScale.x)
            {
                case < 0 when horizontal > 0:
                case > 0 when horizontal < 0:
                    Flip();
                    break;
            }
        }

        // Private Methods
        private void ParametersCheck()
        {
            IsGrounded = Physics2D.OverlapCircle(Position, 0.2f, _groundLayer);
            Distance = Vector2.Distance(_destination, Position);
        }

        private void MovementCheck()
        {
            if (IsDestination)
            {
                _rigidbody.velocity = Vector2.zero;
                _rigidbody.MovePosition(Position);
                return;
            }
            
            MoveToPosition();
        }

        private void MoveToPosition()
        {
            _rigidbody.velocity = new Vector2(Direction.x * (_speed * Time.fixedDeltaTime), Velocity.y);
            
            /*
            Vector2 velocity = Direction * (_speed * Time.fixedDeltaTime);
            var targetPosition = _rigidbody.position + velocity;
            _rigidbody.MovePosition(targetPosition);
            */
        }

        // Flip
        private void Flip()
        {
            var scaler = _thisTransform.localScale;
            scaler.x *= -1;
            _thisTransform.localScale = scaler;
        }
    }
}