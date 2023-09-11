using UnityEngine;

namespace Animachine.Scripts.Core
{
    public abstract class AnimachineState : StateMachineBehaviour
    {
        protected Transform ThisTransform { get; private set; }
        
        private float _stateTime;
        
        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            animator.SetFloat(AnimachineConst.StateTime, _stateTime = 0);
            ThisTransform = animator.transform;
            
            OnEnter();
        }
        
        public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            animator.SetFloat(AnimachineConst.StateTime, _stateTime += Time.deltaTime);
            OnUpdate();
        }
        
        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            animator.SetFloat(AnimachineConst.StateTime, _stateTime = 0);
            OnExit();
        }

        protected virtual void OnEnter() { }
        protected virtual void OnUpdate() { }
        protected virtual void OnExit() { }
    }
}