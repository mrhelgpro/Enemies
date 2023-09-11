using Advemachine2D.Gameplay.Core;
using UnityEngine;

namespace Advemachine2D.Gameplay.Input
{
    [AddComponentMenu("Advemachine2D/Input/InputJumpController")]
    [RequireComponent(typeof(Inputable))]
    //[RequireComponent(typeof(Movable))]
    public class InputJumpController : MonoBehaviour
    {
        public enum  Mode { Button, Auto }
        public Mode mode;

        private Inputable _inputable;
        private Movable _movable;

        private void Awake()
        {
            _inputable = GetComponent<Inputable>();
            _movable = GetComponent<Movable>();
        }

        private void Update()
        {
            if (mode == Mode.Button)
            {
                ButtonCheck();
            }
            else if (mode == Mode.Auto)
            {
                AutoCheck();
            }
        }

        private void ButtonCheck()
        {
            if (UnityEngine.Input.GetKeyDown(KeyCode.Space))
            {
                _inputable.InvokeJump(KeyState.Down);
            }
            
            if (UnityEngine.Input.GetKeyUp(KeyCode.Space))
            {
                _inputable.InvokeJump(KeyState.Up);
            }
        }
        
        private void AutoCheck()
        {
            // Check on the Ground
            const float offsetVertical = 0.5f;
            const float length = 0.75f + offsetVertical;
            
            var origin = new Vector3(_movable.Position.x, _movable.Position.y + offsetVertical, _movable.Position.z);
            var hit = Physics2D.Raycast(origin, Vector3.down, length, _movable.GroundMask);

            var isInputJump = hit.collider == null && _inputable.moveDirection.x != 0;
                
            // Check on the Air or Sliding
            if (_movable.GroundType is GroundType.None or GroundType.Sliding)
            {
                isInputJump = _inputable.moveDirection.magnitude > 0;
            }

            // Get input Jump
            var state = isInputJump ? KeyState.Down : KeyState.Up;

            if (state != _inputable.KeyJump)
            {
                _inputable.InvokeJump(isInputJump ? KeyState.Down : KeyState.Up);
            }
        }
    }
}