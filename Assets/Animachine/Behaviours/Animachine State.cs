using UnityEngine;

namespace Animachine.Behaviours
{
    public abstract class AnimachineState : StateMachineBehaviour
    {
        private float _stateTime;
        private IAnimachineState _state;
        
        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            _state = GetState(animator.transform);
            
            animator.SetFloat(AnimachineConst.StateTime, _stateTime = 0);
            
            _state?.OnEnter();
        }
        
        public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            animator.SetFloat(AnimachineConst.StateTime, _stateTime += Time.deltaTime);
            
            _state?.OnUpdate();
        }
        
        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            animator.SetFloat(AnimachineConst.StateTime, _stateTime = 0);
            
            _state?.OnExit();
        }

        protected abstract IAnimachineState GetState(Transform transform);
    }
    
    // Interface State Machine
    public interface IAnimachineState
    {
        public void OnEnter();
        public void OnUpdate();
        public void OnExit();
    }
    
    public interface IIdling : IAnimachineState { }          // Inactive status
    public interface IPatrolling : IAnimachineState { }      // Moving from point to point
    public interface IChecking : IAnimachineState { }        // When the target has lost from view
    public interface IChasing : IAnimachineState { }         // Distance reduction to the target
    public interface IHiding : IAnimachineState { }          // Not visible on scene for some time
    public interface IAppearing : IAnimachineState { }       // Appearance in a set position
    public interface IAttack : IAnimachineState { }
    public interface IProtecting : IAnimachineState { }
    public interface IDodging : IAnimachineState { }
    public interface IDying : IAnimachineState { }
    public interface IDisable : IAnimachineState { }
}