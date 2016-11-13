using UnityEngine;
using System.Collections;
using UnityEditorInternal;

public class PortalControllerScript : MonoBehaviour {
    public Vector2 position;
    public float MaxRadius = 60f;
    public float MinRadius = 0f;
    public GameObject PortalControllerSurface;

    private GameObject _player;
    private Animator _animator;
    private PortalPhysics _portalPhysics;
    private bool _display = false;

    void Start() {
        this._player = GameObject.FindGameObjectWithTag("Player");
        this._animator = this.GetComponent<Animator>();
        this._portalPhysics = this.GetComponent<PortalPhysics>();
        this._portalPhysics.ComputeColliders(false);
        this.PortalControllerSurface.transform.localScale = Vector3.one * 3.4f;
    }

    void Update() {
        this.handleStates();
        this.displayPortalControllerSurface();
    }

    private void handleStates() {
        AnimatorStateInfo animStateInfo 
            = this._animator.GetCurrentAnimatorStateInfo(0);

        if (Input.GetButtonDown("SetPortal") 
            || Mathf.Round(Input.GetAxisRaw("SetPortal")) < 0) {
            if (animStateInfo.IsName("NotSet")) {
                this.changeToBeingSetState();
            } else if (animStateInfo.IsName("Set")) {
                this.changeToBeingSetState();
            }
        }

        if (Input.GetButtonUp("SetPortal") 
            || Mathf.Round(Input.GetAxisRaw("SetPortal")) == 0) {
            if (animStateInfo.IsName("BeingSet")) {
                this.changeToSetState();
            }
        }

        if (animStateInfo.IsName("BeingSet")) {
            this.handlePosition();
        }

        if (Input.GetButtonUp("RemovePortal")) {
            if (animStateInfo.IsName("Set")) {
                this.changeToNotSetState();
            } else if (animStateInfo.IsName("NotSet")) {
                this.changeToSetState();
            }
        }
    }

    private void changeToNotSetState() {
        this._animator.SetBool("Set", false);
        this._animator.SetBool("BeingSet", false);
        this._display = false;
        this._portalPhysics.DestroyColliders();
        this._portalPhysics.ComputeColliders(false);
        this.handleLimit();
    }

    private void changeToBeingSetState() {
        this._animator.SetBool("Set", false);
        this._animator.SetBool("BeingSet", true);
        this._display = true;
        this._portalPhysics.DestroyColliders();
        this._portalPhysics.ComputeColliders(false);
    }

    private void changeToSetState() {
        this._animator.SetBool("Set", true);
        this._animator.SetBool("BeingSet", false);
        this._display = false;
        this._portalPhysics.ComputeColliders(true);
    }

    private void handlePosition() {
        Vector3 cursorPosition;
        Vector3 playerPortal;
        float x = Input.GetAxis("RightH");
        float y = Input.GetAxis("RightV");

        Vector3 playerPosition = this._player.transform.position;

        if (x != 0 || y != 0) {
            cursorPosition = new Vector3(x, y, 0) * 100f;
            playerPortal = cursorPosition;
        } else {
            cursorPosition 
                = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            cursorPosition.z = 0;
            playerPortal = cursorPosition - playerPosition;
        }     
        
        
        float distance = playerPortal.magnitude;
        distance = Mathf.Clamp(distance, 
            this.MinRadius, 
            this.MaxRadius);
        playerPortal = playerPortal.normalized*distance;
        this.transform.position = playerPosition + playerPortal;
    }

    private void displayPortalControllerSurface() {
        if (this._display) {
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
