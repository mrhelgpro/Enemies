using UnityEngine;

namespace Animachine.Behaviours
{
    [RequireComponent(typeof(Animator))]
    public abstract class AnimatorConfiguratorBehaviour : MonoBehaviour
    {
        // Contract
        protected abstract bool IsGrounded { get; }
        protected abstract bool IsDestination { get; }
        protected abstract bool IsTarget { get; }
        protected abstract bool IsDamage { get; }
        protected abstract Vector3 Velocity { get; }
        protected abstract float Distance { get; }
        protected abstract void Initializer();
        
        // Buffer
        private Animator _animator;
        
        private void Awake()
        {
            _animator = GetComponentInChildren<Animator>();
            
            Initializer();
        }

        private void Update()
        {
            _animator.SetBool(AnimachineConst.IsGrounded, IsGrounded);
            _animator.SetFloat(AnimachineConst.Velocity, Mathf.Abs(Velocity.magnitude));
            _animator.SetBool(AnimachineConst.IsDestination, IsDestination);
            _animator.SetBool(AnimachineConst.IsDamage, IsDamage);
            _animator.SetFloat(AnimachineConst.Distance, Distance);
            _animator.SetBool(AnimachineConst.IsTarget, IsTarget);
        }
    }
}
