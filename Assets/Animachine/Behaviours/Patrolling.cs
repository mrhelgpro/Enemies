using UnityEngine;

namespace Animachine.Behaviours
{
    public class Patrolling : AnimachineState
    {
        protected override IAnimachineState GetState(Transform transform) => transform.GetComponentInParent<IPatrolling>();
    }
}
