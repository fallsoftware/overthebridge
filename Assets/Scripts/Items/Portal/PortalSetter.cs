using UnityEngine;
using System.Collections;
using UnityEditorInternal;

public class PortalSetter : MonoBehaviour {
    public Vector2 position;
    public float MaxRadius = 1f;
    public float MinRadius = 0f;   

    private GameObject _portal;
    private GameObject _player;
    private Animator _animator;
    private PortalPhysics _portalPhysics;

    void Start() {
        this._player = GameObject.FindGameObjectWithTag("Player");
        this._portal = GameObject.FindGameObjectWithTag("Portal");
        this._animator = this.GetComponent<Animator>();
        this._portalPhysics = this.GetComponent<PortalPhysics>();
        this._portalPhysics.ComputeColliders(false);
    }

    void Update() {
        this.handleStates();
    }

    private void handleStates() {
        AnimatorStateInfo animStateInfo = this._animator.GetCurrentAnimatorStateInfo(0);

        if (Input.GetButtonDown("PortalClick")) {
            if (animStateInfo.IsName("NotSet")) {
                this._animator.SetBool("BeingSet", true);
                this.handlePosition();
                this._portalPhysics.DestroyColliders();
                this._portalPhysics.ComputeColliders(false);
            }
        }

        if (animStateInfo.IsName("BeingSet")) {
            this.handlePosition();
        }

        if (Input.GetButtonUp("PortalClick")) {
            if (animStateInfo.IsName("BeingSet")) {
                this._animator.SetBool("BeingSet", false);
                this._animator.SetBool("Set", true);
                this.handlePosition();
                this._portalPhysics.ComputeColliders(true);
            } else if (animStateInfo.IsName("Set")) {
                this._animator.SetBool("BeingSet", false);
                this._animator.SetBool("Set", false);
                this.handleLimit();
                this._portalPhysics.ComputeColliders(false);
            }
        }
    }

    private void handlePosition() {
        Vector3 mousePosition 
            = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0;
        Vector3 playerPosition = this._player.transform.position;

        Vector3 playerPortal 
            = mousePosition - playerPosition;
        float distance = playerPortal.magnitude;
        distance = Mathf.Clamp(distance, 
            this.MinRadius, 
            this.MaxRadius);
        playerPortal = playerPortal.normalized*distance;
        this.transform.position = playerPosition + playerPortal;
    }

    private void handleLimit() {
        
    }
}
