using IuvoUnity._StateMachine;
using UnityEngine;

namespace IuvoUnity
{

    [System.Serializable]
    public abstract class GenericState : ScriptableObject, IStateMachineCondition
    {
        public Animator animator;
        public AnimationClip clip;
        public string animationName;
        public float crossFadeTime = 0.2f;

        public virtual bool CanEnter(GenericStateMachine stateMachine)
        {
            return true;
        }
        public abstract void OnEnter(GenericStateMachine stateMachine);
        public abstract void OnUpdate(GenericStateMachine stateMachine);
        public abstract void OnFixedUpdate(GenericStateMachine stateMachine);
        public abstract void OnExit(GenericStateMachine stateMachine);
        public abstract bool IsConditionMet(GenericStateMachine stateMachine);
    }

}