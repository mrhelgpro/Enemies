using UnityEngine;

namespace Animachine.Behaviours
{
    public class Idling : AnimachineState
    {
        protected override IAnimachineState GetState(Transform transform) => transform.GetComponentInParent<IIdling>();
    }
}