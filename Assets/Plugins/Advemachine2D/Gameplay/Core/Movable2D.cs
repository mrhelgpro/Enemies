using Advemachine2D.Gameplay.Runtime.Interactions;
using UnityEngine;

namespace Advemachine2D.Gameplay.Core
{
    [AddComponentMenu("Advemachine2D/Core/Movable2D")]
    [RequireComponent(typeof(CircleCollider2D))]
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(Inputable))]
    public class Movable2D : MonoBehaviour
    {
        // Get Values
        public bool IsGrounded { get; private set; }
        public GroundType GroundType { get; private set; }
        public Vector2 Velocity { get; private set; }
        public Vector3 Position => _thisTransform.position;
        
        // Stats
        [SerializeField] private float moveSpeed = 160;
        [SerializeField] private float jumpForce = 7;
        [SerializeField, Range(0, 80)] private int maxSurfaceAngle = 40;
        [SerializeField, Range(0, 3)] private int extraJumps;
        [SerializeField, Range(0, 1)] private float levitation = 0.25f;
        
        // Buffer
        private float _gravityScale;
        private float _surfaceAngle;
        private bool _isJumpStart;
        private bool _isJumpFall;
        private bool _isObstacle;
        
        private LayerMask _groundMask;
        private RaycastHit2D _raycastHit;

        // Components
        private Inputable _inputable;
        private Rigidbody2D _rigidbody;
        private CircleCollider2D _groundCollider;
        private Transform _thisTransform;

        // MonoBehaviour
        private void OnEnable()
        {
            _inputable = GetComponent<Inputable>();

            _inputable.OnJumpEvent += Jump;
        }

        private void OnDisable()
        {
            _inputable.OnJumpEvent -= Jump;
        }

        private void Awake()
        {
            _thisTransform = transform;

            _rigidbody = GetComponent<Rigidbody2D>();
            _rigidbody.freezeRotation = true;
            _gravityScale = _rigidbody.gravityScale;

            _groundCollider = GetComponent<CircleCollider2D>();
            _groundCollider.radius = 0.4f * Mathf.Abs(_thisTransform.localScale.x);
            _groundCollider.offset = new Vector2(0, _groundCollider.radius * 3);
            
            _groundMask = LayerMask.GetMask("Default");
        }

        private void FixedUpdate()
        {
            GroundCheck();
            FlipCheck();

            if (IsGrounded)
            {
                if (_isJumpStart == false)
                {
                    if (GroundType == GroundType.Sliding)
                    {
                        Sliding();

                        return;
                    }
                    
                    Move();
                }
            }
            else
            {
                Fall();
            }
        }

        // Ground Check
        private void GroundCheck()
        {
            //IsGrounded = Physics2D.OverlapCircle(Position, 0.25f, _groundMask);
            
            var origin = new Vector2(Position.x, Position.y + 0.5f);
            var direction = Vector2.down;
            const float distance = 2.0f;
            
            // Ground Check
            IsGrounded = _raycastHit = Physics2D.Raycast(origin, direction, distance, _groundMask);
            
            //if(IsGrounded) 
        }

        // Movable
        public void Force(Vector2 value)
        {
            _rigidbody.velocity = Vector2.zero;
            _rigidbody.AddForce(value, ForceMode2D.Impulse);
        }

        // Movement
        private void Sliding()
        {
            var vertical = -0.1f * _surfaceAngle;
            var horizontal = _rigidbody.velocity.x;
            var velocity = new Vector2(horizontal, vertical);
            
            _rigidbody.velocity = Velocity = velocity;
        }
        
        private void Move()
        {
            var horizontal = _inputable.moveDirection.x * moveSpeed * Time.fixedDeltaTime;
            var vertical = _raycastHit.point.y;
           
            // Anchoring to the ground
            var position = new Vector2(_rigidbody.position.x + horizontal, vertical);
            _rigidbody.MovePosition(position);
            _rigidbody.velocity = Vector2.zero;
            _rigidbody.gravityScale = 0;
        }

        
        private void Fall()
        {
            var horizontal = _inputable.moveDirection.x * 50 * moveSpeed * Time.fixedDeltaTime;
            var vertical = _rigidbody.velocity.y;
            var velocity = new Vector2(horizontal, vertical);

            _rigidbody.velocity = Velocity = velocity;
            _rigidbody.gravityScale = 1;
        }

        
        private void Jump(KeyState state)
        {
            if (state == KeyState.Down)
            {
                _rigidbody.gravityScale -= _gravityScale * levitation;
                    
                if (_isJumpFall == false)
                {
                    Force(new Vector2(0, jumpForce));

                    _isJumpStart = true;
                    _isJumpFall = true;

                    Invoke(nameof(ResetJump), 0.1f);
                }
            }

            if (state == KeyState.Up)
            {
                _rigidbody.gravityScale += _gravityScale * levitation;
            }
        }

        private void ResetJump() => _isJumpStart = false;

        // Flip
        private void FlipCheck()
        {
            var direction = _inputable.moveDirection.x;

            switch (_thisTransform.localScale.x)
            {
                case < 0 when direction > 0:
                case > 0 when direction < 0:
                    Flip();
                    break;
            }
        }

        private void Flip()
        {
            var scaler = _thisTransform.localScale;
            scaler.x *= -1;
            _thisTransform.localScale = scaler;
        }
    }
}