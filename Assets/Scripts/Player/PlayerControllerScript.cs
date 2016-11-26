﻿using UnityEngine;
using System.Collections;

public class PlayerControllerScript : MonoBehaviour {
    public float MaxSpeed = 100f;
    public Transform GroundCheck;
    public LayerMask WhatIsGround;
    public float JumpForce = 7000f;
    public float jumpSpeed = 50f;
    public MenuManager MenuManager;
    public bool Grounded = false;

    private bool _facingRight = true;
    private Animator _animator; 
    private float _groundRadius = 0.2f;
    private bool _doubleJump = false;

	void Start() {
        this._animator = this.GetComponent<Animator>();
	}
	
	void Update() {
        if (this.MenuManager.IsPause) return;

        this._animator.SetBool("Jump", !this.Grounded);

        if ((!this.Grounded && this._doubleJump) 
            || !Input.GetButtonDown("Jump")) return;

	    this._animator.SetBool("Ground", false);

	    Rigidbody2D rigidbody2D = this.GetComponent<Rigidbody2D>();
        rigidbody2D.velocity = new Vector2(
       rigidbody2D.velocity.x,
       jumpSpeed);
        
        if (!this._doubleJump && !this.Grounded) {
	        this._doubleJump = true;
	    }
        this.Grounded = false;
    }

    void FixedUpdate() {
        Rigidbody2D rigidbody2D = this.GetComponent<Rigidbody2D>();

        if (this.Grounded) {
            this._doubleJump = false;
        }

        this._animator.SetBool("Ground", this.Grounded);
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
