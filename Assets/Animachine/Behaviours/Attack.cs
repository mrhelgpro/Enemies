using UnityEngine;

namespace Animachine.Behaviours
{
    public class Attack : AnimachineState
    {
        protected override IAnimachineState GetState(Transform transform) => transform.GetComponentInParent<IAttack>();
    }
}