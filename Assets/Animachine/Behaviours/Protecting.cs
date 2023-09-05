using UnityEngine;

namespace Animachine.Behaviours
{
    public class Protecting : AnimachineState
    {
        protected override IAnimachineState GetState(Transform transform) => transform.GetComponentInParent<IProtecting>();
    }
}
