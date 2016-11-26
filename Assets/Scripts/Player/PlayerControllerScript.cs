using UnityEngine;
using System.Collections;

public class PlayerControllerScript : MonoBehaviour {
    public float MaxSpeed = 100f;
    public Transform GroundCheck;
    public LayerMask WhatIsGround;
    public float JumpForce = 7000f;
    public MenuManager MenuManager;

    private bool _facingRight = true;
    private Animator _animator;
    private bool _grounded = false;
    private float _groundRadius = 0.2f;
    private bool _doubleJump = false;

	void Start() {
        this._animator = this.GetComponent<Animator>();
	}
	
	void Update() {
        if (this.MenuManager.IsPause) return;

        this._animator.SetBool("Jump", !this._grounded);

        if ((!this._grounded && this._doubleJump) 
            || !Input.GetButtonDown("Jump")) return;

	    this._animator.SetBool("Ground", false);

	    Rigidbody2D rigidbody2D = this.GetComponent<Rigidbody2D>();

	    rigidbody2D.AddForce(
	        Vector2.up * this.JumpForce, 
	        ForceMode2D.Impulse);

	    if (!this._doubleJump && !this._grounded) {
	        this._doubleJump = true;
	    }
	}

    void FixedUpdate() {
        Rigidbody2D rigidbody2D = this.GetComponent<Rigidbody2D>();

        this._grounded = Physics2D.OverlapCircle(
            this.GroundCheck.position, 
            this._groundRadius, 
            this.WhatIsGround);

        if (this._grounded) {
            this._doubleJump = false;
        }

        this._animator.SetBool("Ground", this._grounded);
        this._animator.SetFloat("vSpeed", rigidbody2D.velocity.y);
        float move = Input.GetAxis("Horizontal");
        this._animator.SetFloat("Speed", Mathf.Abs(move));
        rigidbody2D.velocity = new Vector2(
            move * this.MaxSpeed, 
            rigidbody2D.velocity.y);

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
