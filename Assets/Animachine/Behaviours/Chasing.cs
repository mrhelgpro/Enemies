using UnityEngine;

namespace Animachine.Behaviours
{
    public class Chasing : AnimachineState
    {
        protected override IAnimachineState GetState(Transform transform) => transform.GetComponentInParent<IChasing>();
    }
}