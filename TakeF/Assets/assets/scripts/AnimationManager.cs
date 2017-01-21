using UnityEngine;
using System.Collections;

public class AnimationManager : MonoBehaviour { 


    public enum AnimationState {flying,land,takeOff,glide,die,walk,idle,hit,eat,grab };
    public AnimationState state = AnimationState.idle;

    public Animator anim;
    public string prevState;


   void Update()
    {

        
        switch (state)
        {
            case AnimationState.flying:
                //ResetBools("flying",prevState);
                break;
            case AnimationState.land:
                //ResetBools("land", prevState);
                break;
            case AnimationState.takeOff:
                //ResetBools("takeOff", prevState);
                break;
            case AnimationState.glide:
                //ResetBools("glide", prevState);
                break;
            case AnimationState.die:
                //ResetBools("die", prevState);
                break;
            case AnimationState.walk:
                //ResetBools("walk", prevState);
                break;
            case AnimationState.idle:
                //ResetBools("idle", prevState);
                break;
            case AnimationState.grab:
                //ResetBools("catch", prevState);
                break;
            case AnimationState.hit:
                //ResetBools("hit", prevState);
                break;
            case AnimationState.eat:
                //ResetBools("eat", prevState);
                break;
            default:
                break;
        }
    }

    void ResetBools(string state, string prev)
    {
        anim.SetBool(state, true);
        anim.SetBool(prev, false);

    }

   public void ResetAllBools()
    {
        anim.SetBool("idle", false);
        anim.SetBool("land", false);
        anim.SetBool("takeOff", false);
        anim.SetBool("eat", false);
        anim.SetBool("walk", false);
        anim.SetBool("flying", false);
        anim.SetBool("glide", false);
        anim.SetBool("catch", false);
    }

    public void setBool(string bOOl, bool trueOrFalse)
    {
        
        anim.SetBool(bOOl, trueOrFalse);
    }

    public void ChangeState(string current, string changeTo)
    {

        state = (AnimationState)System.Enum.Parse(typeof(AnimationState), changeTo);
        prevState = current;

        ResetBools(changeTo, current);
    }

  

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    //override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
    //
    //}

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    //override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
    //
    //}

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    //override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
    //
    //}

    // OnStateMove is called right after Animator.OnAnimatorMove(). Code that processes and affects root motion should be implemented here
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
    //
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK(). Code that sets up animation IK (inverse kinematics) should be implemented here.
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
    //
    //}
}
