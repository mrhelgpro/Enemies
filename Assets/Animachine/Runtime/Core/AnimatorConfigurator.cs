using Animachine.Behaviours;
using UnityEngine;

namespace Animachine.Runtime.Core
{
    public class AnimatorConfigurator : AnimatorConfiguratorBehaviour
    {
        private IDamageable _damageable;
        private INavigation _navigation;
        private IDetectable _detectable;
        
        protected override bool IsGrounded => _navigation.IsGrounded;
        protected override bool IsDestination => _navigation.IsDestination;
        protected override bool IsDamage => _damageable.IsDamage;
        protected override Vector3 Velocity => _navigation.Velocity;
        protected override float Distance => _navigation.Distance;
        protected override bool IsTarget => _detectable.Target;

        protected override void Initializer()
        {
            _damageable = GetComponentInParent<IDamageable>();
            _navigation = GetComponentInParent<INavigation>();
            _detectable = GetComponentInParent<IDetectable>();
        }
    }
}