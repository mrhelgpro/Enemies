using Advemachine2D.Gameplay.Runtime.Interactions;
using UnityEngine;

namespace Advemachine2D.Gameplay.Core
{
    public enum GroundType { None, Static, Dynamic, Sliding, Wall, Under, Water }

    [AddComponentMenu("Advemachine2D/Core/Movable")]
    [RequireComponent(typeof(CircleCollider2D))]
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(Inputable))]
    public class Movable : MonoBehaviour
    {
        // Get Values
        public bool IsGrounded { get; private set; }
        public int GroundMask { get; private set; }
        public GroundType GroundType { get; private set; }
        public Vector2 Velocity { get; private set; }
        public Vector3 Position => _thisTransform.position;
        
        // Stats
        [SerializeField] private float moveSpeed = 160;
        [SerializeField] private float jumpForce = 7;
        [SerializeField, Range(0, 80)] private int maxSurfaceAngle = 40;
        [SerializeField, Range(0, 3)] private int extraJumps;
        [SerializeField, Range(0, 1)] private float levitation = 0.25f;
        
        // Logic
        private float _gravityScale;
        private float _surfaceAngle;
        private bool _isJumpStart;
        private bool _isJumpFall;
        private bool _isObstacle;

        // Components
        private Inputable _inputable;
        private Rigidbody2D _rigidbody;
        private CircleCollider2D _groundCollider;
        private PhysicsMaterial2D _frictionMaterial;
        private PhysicsMaterial2D _slidingMaterial;
        private Transform _thisTransform;

        private void OnEnable()
        {
            _inputable = GetComponent<Inputable>();

            _inputable.OnJumpEvent += Jump;
        }

        private void OnDisable()
        {
            _inputable.OnJumpEvent -= Jump;
        }

        // MonoBehaviour
        private void Awake()
        {
            _thisTransform = transform;

            _rigidbody = GetComponent<Rigidbody2D>();
            _rigidbody.freezeRotation = true;
            _gravityScale = _rigidbody.gravityScale;

            _groundCollider = GetComponent<CircleCollider2D>();
            _groundCollider.radius = 0.1f * Mathf.Abs(_thisTransform.localScale.x);
            _groundCollider.offset = new Vector2(0, _groundCollider.radius);

            GroundMask = 1 << LayerMask.NameToLayer("Default");

            _frictionMaterial = Resources.Load<PhysicsMaterial2D>("Physic2D/Friction 2D");
            _slidingMaterial = Resources.Load<PhysicsMaterial2D>("Physic2D/Sliding 2D");
            
            if (_frictionMaterial == null || _slidingMaterial == null)
            {
                Debug.LogWarning(gameObject.name + " - Resource (Path:Physic2D/Friction) 2D is not found");
                enabled = false;
            }
        }

        private void FixedUpdate()
        {
            CheckFlip();
            CheckObstacle();

            if (IsGrounded)
            {
                if (_isJumpStart == false)
                {
                    if (GroundType == GroundType.Sliding)
                    {
                        Sliding();

                        return;
                    }

                    if (_inputable.moveDirection.x == 0 || _isObstacle)
                    {
                        Stop();

                        return;
                    }
                }
            }

            Move();
        }

        private void Update()
        {
            UpdateMaterial();
        }

        // Ground Check
        private void OnCollisionStay2D(Collision2D collision)
        {
            foreach (ContactPoint2D contact in collision.contacts)
            {
                const int minWallAngle = 80;
                var angleInRadians = Mathf.Atan2(contact.normal.y, contact.normal.x);
                _surfaceAngle = Mathf.Abs(Mathf.Round(angleInRadians * Mathf.Rad2Deg - 90));

                if (_surfaceAngle < minWallAngle)
                {
                    if (_surfaceAngle > maxSurfaceAngle)
                    {
                        // Check Sliding
                        GroundType = GroundType.Sliding;
                    }
                    else
                    {
                        // Check Static or Dynamic 
                        var elevator = contact.collider.GetComponent<Dynamic>();
                        GroundType = elevator == null ? GroundType.Static : GroundType.Dynamic;
                    }

                    if (_isJumpStart == false)
                    {
                        _isJumpFall = false;
                    }

                    IsGrounded = true;

                    break;
                }
            }
        }

        private void OnCollisionExit2D()
        {
            IsGrounded = false;
            GroundType = GroundType.None;
        }

        // Movable
        public void Force(Vector2 value)
        {
            _rigidbody.velocity = Vector2.zero;
            _rigidbody.AddForce(value, ForceMode2D.Impulse);
        }
        
        private void CheckObstacle()
        {
            var offsetHorizontal = _thisTransform.localScale.x * 0.25f;
            const float offsetVertical = 0.25f;
            const float length = 1 + offsetVertical;

            var position = _thisTransform.position;
            var origin = new Vector3(position.x + offsetHorizontal, position.y + offsetVertical, position.z);
            RaycastHit2D hit = Physics2D.Raycast(origin, Vector3.down, length, GroundMask);

            var angleInRadians = Mathf.Atan2(hit.normal.y, hit.normal.x);
            var surfaceAngle = hit.collider == null ? 0 : Mathf.Abs(Mathf.Round(angleInRadians * Mathf.Rad2Deg - 90));

            var directionToSlope = Mathf.Approximately(Mathf.Sign(_inputable.moveDirection.x),Mathf.Sign(_thisTransform.localScale.x));
            //Mathf.Sign(_inputable.moveDirection.x) == Mathf.Sign(_thisTransform.localScale.x);

            _isObstacle = directionToSlope && surfaceAngle > maxSurfaceAngle;
        }

        private void UpdateMaterial()
        {
            if (IsGrounded)
            {
                _groundCollider.sharedMaterial = GroundType == GroundType.Dynamic ? _frictionMaterial : _slidingMaterial;
            }
            else
            {
                _groundCollider.sharedMaterial = _slidingMaterial;
            }
        }

        // Movement
        private void Stop()
        {
            if (GroundType == GroundType.Dynamic)
            {
                _rigidbody.constraints = RigidbodyConstraints2D.FreezeRotation;
            }
            else
            {
                SetVelocity(Vector2.zero, RigidbodyConstraints2D.FreezeAll);
            }
        }

        private void Sliding()
        {
            var vertical = -0.1f * _surfaceAngle;
            var horizontal = _rigidbody.velocity.x;
            var velocity = new Vector2(horizontal, vertical);
            
            SetVelocity(velocity, RigidbodyConstraints2D.FreezeRotation);
        }

        private void Move()
        {
            var vertical = _rigidbody.velocity.y;

            var speed = moveSpeed * Time.fixedDeltaTime;
            var horizontalGrounded = _inputable.moveDirection.x * speed;
            var horizontalSlope = _thisTransform.localScale.x * speed;
            var horizontal = GroundType == GroundType.Sliding ? horizontalSlope : horizontalGrounded;
            var velocity = new Vector2(horizontal, vertical);
            
            SetVelocity(velocity, RigidbodyConstraints2D.FreezeRotation);
        }

        private void SetVelocity(Vector2 velocity, RigidbodyConstraints2D constraints)
        {
            _rigidbody.velocity = Velocity = velocity;
            _rigidbody.constraints = constraints;
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
        private void CheckFlip()
        {
            var directionVelocity = Velocity.x;
            var directionRigidbody = Mathf.Round(_rigidbody.velocity.x);
            var direction = directionVelocity == 0 ? directionRigidbody : directionVelocity;

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