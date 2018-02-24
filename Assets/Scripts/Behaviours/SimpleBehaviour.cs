using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DragonBones;

public class SimpleBehaviour : StateMachineBehaviour
{
    [SerializeField] Aliases.Animation animation;
    [SerializeField] int playTimes = 0;
    [SerializeField] float speed = 1;
    [SerializeField] string exitTrigger = null;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        var uac = animator.GetComponent<UnityArmatureComponent>();
        uac.animation.timeScale = speed;
        uac.animation.FadeIn(Aliases.DragonbonesAnimationNameConverter.Forward(animation), 0, playTimes);
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        var uac = animator.GetComponent<UnityArmatureComponent>();
        if (uac.animation.isCompleted && !String.IsNullOrEmpty(exitTrigger)) {
            animator.SetTrigger(exitTrigger);
        }
    }

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
