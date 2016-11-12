using UnityEngine;
using System.Collections;

public class PortalBeingSet : StateMachineBehaviour {
    public float Alpha = .2f;
    private float _oldAlpha = 1f;
    private SpriteRenderer _spriteRenderer;

    override public void OnStateEnter(
        Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        base.OnStateEnter(animator, stateInfo, layerIndex);

        this._spriteRenderer 
            = animator.gameObject.GetComponent<SpriteRenderer>();
        this._oldAlpha = this._spriteRenderer.color.a;
        Color oldColor = this._spriteRenderer.color;
        this._spriteRenderer.color = new Color(oldColor.r,
            oldColor.g,
            oldColor.b,
            this.Alpha);
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and 
    // OnStateExit callbacks
    //override public void OnStateUpdate(
    //    Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
    //
    //}

    override public void OnStateExit(
        Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        Color oldColor = this._spriteRenderer.color;
        this._spriteRenderer.color = new Color(oldColor.r,
            oldColor.g,
            oldColor.b,
            this._oldAlpha);
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