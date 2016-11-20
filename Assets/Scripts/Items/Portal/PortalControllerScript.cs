using UnityEngine;
using System.Collections;
using UnityEditorInternal;

public class PortalControllerScript : MonoBehaviour {
    public Vector2 position;
    public float MaxRadius = 60f;
    public float MinRadius = 0f;
    public GameObject PortalControllerSurface;
    public float AxisControllerTweak = 50f;
    public float AxisControllerMaxSpeed = 30f;
    public float TriggerTolerance = 0.2f;

    private GameObject _player;
    private Animator _animator;
    private PortalPhysics _portalPhysics;
    private bool _display = false;
    private Vector3 _oldPosition;
    private bool _isTriggerUsed = false;

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

    private bool isTriggerPressed() {
        if (Input.GetAxisRaw("SetPortal") > this.TriggerTolerance) {
            if (!this._isTriggerUsed) this._isTriggerUsed = true;
        } else {
            this._isTriggerUsed = false;
        }

        return this._isTriggerUsed;
    }

    private void handleStates() {
        AnimatorStateInfo animStateInfo 
            = this._animator.GetCurrentAnimatorStateInfo(0);

        if (animStateInfo.IsName("Set")) {
            if (Input.GetButtonDown("SetPortal")
                || this.isTriggerPressed()) {
                this.changeToBeingSetState();
            }

            if (Input.GetButtonUp("RemovePortal")) {
                this.changeToNotSetState();
            }
        } else if (animStateInfo.IsName("BeingSet")) {
            if (!Input.GetButton("SetPortal")
                && !this.isTriggerPressed()) {
                this.changeToSetState();
            }

            this.handlePosition();
        } else if (animStateInfo.IsName("NotSet")) {
            if (Input.GetButtonDown("SetPortal")
                || this.isTriggerPressed()) {
                this.changeToBeingSetState();
            }

            if (Input.GetButtonUp("RemovePortal")) {
                this.changeToSetState();
            }
        }

        Debug.Log("BUTTON = " + Input.GetButton("SetPortal"));
        Debug.Log("AXIS = " + this.isTriggerPressed());
        Debug.Log("TRIGGER = " + Input.GetAxisRaw("SetPortal"));
        Debug.Log("TOL = " + this.TriggerTolerance);
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

    private Vector2 getControllerAxis() {
        return new Vector2(
             Input.GetAxis("RightH"),
             Input.GetAxis("RightV"));
    }

    private bool isControllerMovement(Vector2 controllerAxis) {
        return controllerAxis.x != 0 || controllerAxis.y != 0;
    }

    private bool isMouseMovement() {
        return Input.GetAxis("Mouse X") != 0 || Input.GetAxis("Mouse Y") != 0;
    }

    private Vector3 clampAxisMovement(Vector3 axisMovement) {
        float x = axisMovement.x;
        float y = axisMovement.y;
        float z = axisMovement.z;

        x = Mathf.Sign(x) 
            * Mathf.Min(Mathf.Abs(x), this.AxisControllerMaxSpeed);
        y = Mathf.Sign(y)
            * Mathf.Min(Mathf.Abs(y), this.AxisControllerMaxSpeed);
        z = Mathf.Sign(z)
            * Mathf.Min(Mathf.Abs(z), this.AxisControllerMaxSpeed);

        return new Vector3(x, y, z);
    }

    private void handlePosition() {
        Vector3 cursorPosition;
        Vector3 playerPortal;
        Vector2 controllerAxis = this.getControllerAxis();
        Vector3 offset = this._player.transform.position;

        // we check the controller first
        if (this.isControllerMovement(controllerAxis)) {
            cursorPosition = new Vector3(
                controllerAxis.x, 
                controllerAxis.y, 
                0) * this.AxisControllerTweak;
            cursorPosition = this.clampAxisMovement(cursorPosition);
            playerPortal = this._oldPosition + cursorPosition;
        } else if (this.isMouseMovement()) {
            // then, we check the mouse
            cursorPosition
                = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            cursorPosition.z = 0;
            playerPortal = cursorPosition - offset;
        } else {
            // default behavior when no mouvement is detected
            playerPortal = this.transform.position - offset;
        }


        this._oldPosition = playerPortal;
        this.transform.position 
            = this.clampPortalPosition(playerPortal, offset);
    }

    private Vector3 clampPortalPosition(Vector3 portalPosition, 
        Vector3 offset) {
        float distance = portalPosition.magnitude;
        distance = Mathf.Clamp(distance,
            this.MinRadius,
            this.MaxRadius);
        portalPosition = portalPosition.normalized * distance;

        return offset + portalPosition;
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
