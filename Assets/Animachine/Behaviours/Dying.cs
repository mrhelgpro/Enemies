using UnityEngine;

namespace Animachine.Behaviours
{
    public class Dying : AnimachineState
    {
        protected override IAnimachineState GetState(Transform transform) => transform.GetComponentInParent<IDying>();
    }
}