using UnityEngine;
using System.Collections;

public class PlayerControllerScript : MonoBehaviour {
    public float MaxSpeed = 100f;
    public Transform GroundCheck;
    public LayerMask WhatIsGround;
    public float jumpSpeed = 50f;
    public MenuManager MenuManager;
    public bool Grounded = false;
    public Vector2 externalForce = Vector2.zero;
    private bool _facingRight = true;
    private Animator _animator;
    private Animator _lightAnimator;
    private float _groundRadius = 0.2f;
    public bool _doubleJump = true;
    private float inertiaTime = 0.1f;
    private float startTime = 0;
    private bool inbeam=false;

    void Start() {
        this._animator = this.GetComponent<Animator>();
        this._lightAnimator = this.transform.GetComponentInChildren<Animator>();

    }
	public void InitBeam(Vector2 BeamVelocity)
    {
        inbeam = true;
        externalForce = BeamVelocity;
    }
	void Update() {
        float timeleft = Time.time - startTime;
        if (inertiaTime - timeleft <= 0)
        {
            inbeam = false;
            externalForce = Vector2.zero;
        }
        if (this.MenuManager.IsPause) return;

        this._animator.SetBool("Jump", !this.Grounded);
        this._lightAnimator.SetBool("Jump", !this.Grounded);
        if ((!this.Grounded && this._doubleJump) 
            || !Input.GetButtonDown("Jump")) return;

	    this._animator.SetBool("Ground", false);
        this._lightAnimator.SetBool("Ground", false);
        Rigidbody2D rigidbody2D = this.GetComponent<Rigidbody2D>();
        rigidbody2D.velocity = new Vector2(
       rigidbody2D.velocity.x,
       jumpSpeed)+ externalForce;
        
        if (!this._doubleJump && !this.Grounded) {
	        this._doubleJump = true;
	    }
        this.Grounded = false;
    }

    void FixedUpdate() {
        Rigidbody2D rigidbody2D = this.GetComponent<Rigidbody2D>();

        if (this.Grounded) {
            this._doubleJump = true;
        }

        this._animator.SetBool("Ground", this.Grounded);
        this._animator.SetFloat("vSpeed", rigidbody2D.velocity.y);
        this._lightAnimator.SetBool("Ground", this.Grounded);
        this._lightAnimator.SetFloat("vSpeed", rigidbody2D.velocity.y);
        float move = Input.GetAxis("Horizontal");
        this._animator.SetFloat("Speed", Mathf.Abs(move));
        this._lightAnimator.SetFloat("Speed", Mathf.Abs(move));
        if (!inbeam) {
            rigidbody2D.velocity = new Vector2(
            move * this.MaxSpeed,
            rigidbody2D.velocity.y) + externalForce;
        }
        else {
            rigidbody2D.velocity = rigidbody2D.velocity = new Vector2(
            move * this.MaxSpeed,
            0)+ externalForce;
        }
        if (move > 0 && !this._facingRight) {
            this.Flip();
        } else if (move < 0 && this._facingRight) {
            this.Flip();
        }
    }

    void Flip() {
        Vector3 scale = this.transform.localScale;
        scale.x *= -1;
        this.transform.localScale = scale;
        this._facingRight = !this._facingRight;
    }
}
