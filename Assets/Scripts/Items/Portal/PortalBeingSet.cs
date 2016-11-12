using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PortalBeingSet : StateMachineBehaviour {
    public float Alpha = .19f;
    private PortalSpriteHandler _portalSpriteHandler;

    override public void OnStateEnter(
        Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        base.OnStateEnter(animator, stateInfo, layerIndex);
        this._portalSpriteHandler 
            = animator.gameObject.GetComponent<PortalSpriteHandler>();
        this._portalSpriteHandler.SetAlpha(this.Alpha);
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and 
    // OnStateExit callbacks
    //override public void OnStateUpdate(
    //    Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
    //
    //}

    override public void OnStateExit(
        Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        base.OnStateEnter(animator, stateInfo, layerIndex);
        this._portalSpriteHandler.RevertAlpha();
    }

    // OnStateMove is called right after Animator.OnAnimatorMove(). 
    // Code that processes and affects root motion should be implemented here
    //override public void OnStateMove(
    //    Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
    //
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK().
    // Code that sets up animation IK (inverse kinematics) 
    // should be implemented here.
    //override public void OnStateIK(
    //    Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
    //
    //}
}