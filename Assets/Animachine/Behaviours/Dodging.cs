using UnityEngine;

namespace Animachine.Behaviours
{
    public class Dodging : AnimachineState
    {
        protected override IAnimachineState GetState(Transform transform) => transform.GetComponentInParent<IDodging>();
    }
}