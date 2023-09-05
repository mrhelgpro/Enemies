using UnityEngine;

namespace Advemachine2D.Gameplay.Core
{
    public enum KeyState { Up, Down, Hold }
    
    public delegate void InputKeyHandler(KeyState state);
    
    [AddComponentMenu("Advemachine2D/Core/Inputable")]
    public class Inputable : MonoBehaviour
    {
        // Values
        public Vector2 moveDirection;
        public Vector2 lookDirection;
        
        // Keys
        public KeyState KeyJump { get; private set; }
        public KeyState KeyDodge { get; private set; }
        public KeyState KeyAttack { get; private set; }
        
        // Events
        public event InputKeyHandler OnJumpEvent;
        public event InputKeyHandler OnDodgeEvent;
        public event InputKeyHandler OnAttackEvent;

        public void InvokeJump(KeyState state) => OnJumpEvent?.Invoke(KeyJump = state);
        public void InvokeDodge(KeyState state) => OnDodgeEvent?.Invoke(KeyDodge = state);
        public void InvokeAttack(KeyState state) => OnAttackEvent?.Invoke(KeyAttack = state);
    }
}