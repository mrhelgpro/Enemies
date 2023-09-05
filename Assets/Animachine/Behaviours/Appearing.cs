using UnityEngine;

namespace Animachine.Behaviours
{
    public class Appearing : AnimachineState
    {
        protected override IAnimachineState GetState(Transform transform) => transform.GetComponentInParent<IAppearing>();
    }
}
