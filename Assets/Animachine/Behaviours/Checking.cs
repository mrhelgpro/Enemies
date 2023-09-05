using UnityEngine;

namespace Animachine.Behaviours
{
    public class Checking : AnimachineState
    {
        protected override IAnimachineState GetState(Transform transform) => transform.GetComponentInParent<IChecking>();
    }
}