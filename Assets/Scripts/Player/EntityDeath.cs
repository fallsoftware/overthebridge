using UnityEngine;
using System.Collections;

public class EntityDeath : MonoBehaviour {
    private Animator _animator;
    private Rigidbody2D _player;
    private RigidbodyConstraints2D _previousRigidbodyConstraints2D;

	// Use this for initialization
	void Start () {
	    this._animator = this.GetComponent<Animator>();
	    this._player = this.GetComponent<Rigidbody2D>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public IEnumerator MakeEntityDie() {
        this._previousRigidbodyConstraints2D = this._player.constraints;
        Collider2D[] colliders = this.GetComponents<Collider2D>();
        foreach(Collider2D c in colliders)
        {
            c.enabled = false;
        }
        this._player.constraints = RigidbodyConstraints2D.FreezeAll;

        this._animator.SetBool("Dead", true);


        while (!this._animator.GetCurrentAnimatorStateInfo(0).IsName("Dead")) {
            yield return null;
        }

        while (
            this._animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1) {
            yield return null;
        }
    }

    public void MakeEntityLive() {
        this._animator.SetBool("Dead", false);
        this._player.constraints = this._previousRigidbodyConstraints2D;
        Collider2D[] colliders = this.GetComponents<Collider2D>();
        foreach (Collider2D c in colliders)
        {
            c.enabled = true;
        }
    }
}
