using UnityEngine;

namespace Advemachine2D.Gameplay.Runtime.Interactions
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class Elevator : Dynamic
    {
        private enum Direction { Horizontal, Vertical }

        // Settings
        [SerializeField] private Direction direction = Direction.Horizontal;
        [SerializeField] private float length = 5f;
        [SerializeField] [Range(0, 2)] private float moveSpeed = 1f;

        // Buffer
        private Vector2 _horizontal;
        private Vector2 _vertical;
        private float _maxOffset;
        private float _currentOffset;
        private Vector2 _startPosition;
        private Rigidbody2D _rigidbody;

        private void Start()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
            _rigidbody.gravityScale = 0;
            _rigidbody.freezeRotation = true;
            _startPosition = _rigidbody.position;

            // Constant
            _horizontal = length >= 0 ? Vector2.right : Vector2.left;
            _vertical = length >= 0 ? Vector2.up : Vector2.down;
            _maxOffset = Mathf.Abs(length);
        }

        private void FixedUpdate()
        {
            _currentOffset += moveSpeed * Time.fixedDeltaTime;

            var distance = Mathf.PingPong(_currentOffset, _maxOffset);
            var targetDirection = direction == Direction.Horizontal ? _horizontal : _vertical;
            var targetPosition = _startPosition + targetDirection * distance;

            _rigidbody.MovePosition(targetPosition);
        }
    }
}
