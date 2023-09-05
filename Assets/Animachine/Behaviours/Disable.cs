using UnityEngine;

namespace Animachine.Behaviours
{
    public class Disable : AnimachineState
    {
        protected override IAnimachineState GetState(Transform transform) => transform.GetComponentInParent<IDisable>();
    }
}