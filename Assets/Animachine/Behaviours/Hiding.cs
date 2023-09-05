using UnityEngine;

namespace Animachine.Behaviours
{
    public class Hiding : AnimachineState
    {
        protected override IAnimachineState GetState(Transform transform) => transform.GetComponentInParent<IHiding>();
    }
}
