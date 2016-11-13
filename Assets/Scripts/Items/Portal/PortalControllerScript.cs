using UnityEngine;
using System.Collections;
using UnityEditorInternal;

public class PortalControllerScript : MonoBehaviour {
    public Vector2 position;
    public float MaxRadius = 1f;
    public float MinRadius = 0f;
    public GameObject PortalControllerSurface;

    private GameObject _player;
    private Animator _animator;
    private PortalPhysics _portalPhysics;

    void Start() {
        this._player = GameObject.FindGameObjectWithTag("Player");
        this._animator = this.GetComponent<Animator>();
        this._portalPhysics = this.GetComponent<PortalPhysics>();
        this._portalPhysics.ComputeColliders(false);
        this.PortalControllerSurface.transform.localScale = Vector3.one * 3.4f;
    }

    void Update() {
        this.handleStates();
    }

    private void handleStates() {
        AnimatorStateInfo animStateInfo 
            = this._animator.GetCurrentAnimatorStateInfo(0);

        if (Input.GetButtonDown("PortalClick")) {
            if (animStateInfo.IsName("NotSet")) {
                this._animator.SetBool("BeingSet", true);
                this.handlePosition();
                this.displayPortalControllerSurface(true);
                this._portalPhysics.DestroyColliders();
                this._portalPhysics.ComputeColliders(false);
            }
        }

        if (animStateInfo.IsName("BeingSet")) {
            this.handlePosition();
        }

        if (Input.GetButtonUp("PortalClick")) {
            if (animStateInfo.IsName("BeingSet")) {
                this.displayPortalControllerSurface(false);
                this._animator.SetBool("BeingSet", false);
                this._animator.SetBool("Set", true);
                this._portalPhysics.ComputeColliders(true);
            } else if (animStateInfo.IsName("Set")) {
                this._animator.SetBool("BeingSet", false);
                this.displayPortalControllerSurface(false);
                this._animator.SetBool("Set", false);
                this._portalPhysics.DestroyColliders();
                this._portalPhysics.ComputeColliders(false);
                this.handleLimit();
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

    private void displayPortalControllerSurface(bool display) {
        if (display) {
            this.PortalControllerSurface.SetActive(true);
            this.renderPortalControllerSurface();
        } else {
            this.PortalControllerSurface.SetActive(false);
        }
    }

    private void renderPortalControllerSurface() {
        this.PortalControllerSurface.transform.position 
            = this._player.transform.position;
    }

    private void handleLimit() {
        
    }
}
