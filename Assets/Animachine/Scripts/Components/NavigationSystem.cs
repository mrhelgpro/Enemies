using Animachine.Scripts.Interfaces;
using UnityEngine;

namespace Animachine.Scripts.Components
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class NavigationSystem : MonoBehaviour, INavigation
    {
        [SerializeField] private NavigationMode mode;

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
            _rigidbody.gravityScale = mode is NavigationMode.Fly ? 0 : 1;
            _groundLayer = LayerMask.GetMask("Default");
        }

        private void FixedUpdate()
        {
            ParametersCheck();
            MovementCheck();
        }

        // INavigation
        public NavigationMode Mode => mode;
        public Vector3 Position => _thisTransform.position;
        public Vector3 Velocity => _rigidbody.velocity;
        public Vector3 Direction => IsDestination ? new Vector3(Mathf.Sign(_thisTransform.localScale.x),0,0) : (_destination - Position).normalized;
        public bool IsGrounded { get; private set; }
        public bool IsDestination => Distance < 0.25f;
        public float Distance { get; private set; }
        public void SetSpeed(float value) => _speed = value;

        public void SetDestination(Vector3 vector)
        {
            var height = mode is NavigationMode.Fly ? 1 : 0; // IT NEEDS TO BE FIXED!
            var offset = new Vector3(0, height, 0);
            _destination = vector + offset;
        }

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
            
            // Check mode
            if (mode is NavigationMode.Horizontal)
            {
                MoveByHorizontal();
            }
            else
            {
                MoveByFly();
            }
        }

        private void MoveByHorizontal()
        {
            _rigidbody.velocity = new Vector2(Direction.x * (_speed * Time.fixedDeltaTime), Velocity.y);
            
            /*
            Vector2 velocity = Direction * (_speed * Time.fixedDeltaTime);
            var targetPosition = _rigidbody.position + velocity;
            _rigidbody.MovePosition(targetPosition);
            */
        }

        private void MoveByFly()
        {
            var velocity = Direction * (_speed * Time.fixedDeltaTime);
            _rigidbody.velocity = velocity;
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