using System;
using UnityEngine;
using System.Collections;
using UnityEditorInternal;

public class PortalControllerScript : MonoBehaviour {
    public Vector2 position;
    public float MaxRadius = 60f;
    public float MinRadius = 0f;
    public float PossessionState = 2;
    public GameObject PortalControllerSurface;
    public float AxisControllerTweak = 50f;
    public float AxisControllerMaxSpeed = 30f;
    public float TriggerTolerance = 0.2f;
    public MenuManager MenuManager;

    private GameObject _player;
    private Animator _animator;
    private PortalPhysics _portalPhysics;
    private bool _display = false;
    private Vector3 _oldPosition;
    private bool _isTriggerUsed = false;
    private bool _controllerMode = false;
    private SpawnEnemy _spawnEnemy;

    void Start() {
        this._player = GameObject.FindGameObjectWithTag("Player");
        this._animator = this.GetComponent<Animator>();
        this._portalPhysics = this.GetComponent<PortalPhysics>();
        this._spawnEnemy = this.GetComponent<SpawnEnemy>();
        this._portalPhysics.ComputeColliders(false);
        this.PortalControllerSurface.transform.localScale = Vector3.one * 3.4f;
    }

    void Update() {
        if (this.MenuManager.IsPause) return;
        if (PossessionState==2)
        {
            this.handleStates();
            this.displayPortalControllerSurface();
        }
        
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
    }

    public void changeToNotSetState() {
        this._animator.SetBool("Set", false);
        this._animator.SetBool("BeingSet", false);
        this._display = false;
        this._portalPhysics.DestroyColliders();
        this._portalPhysics.ComputeColliders(false);
        this.handleLimit();
    }

    public void changeToBeingSetState() {
        this._animator.SetBool("Set", false);
        this._animator.SetBool("BeingSet", true);
        this._display = true;
        this._portalPhysics.DestroyColliders();
        this._portalPhysics.ComputeColliders(false);
    }

    public void changeToSetState() {
        this._animator.SetBool("Set", true);
        this._animator.SetBool("BeingSet", false);
        this._display = false;
        this._portalPhysics.ComputeColliders(true);
        this._spawnEnemy.SpawnNewEnemy();
    }

    private Vector2 getControllerAxis() {
        return new Vector2(
             Input.GetAxis("RightH"),
             Input.GetAxis("RightV"));
    }

    private Vector2 getCenterControllerAxis() {
        return new Vector2(
             Input.GetAxis("RightHCenter"),
             Input.GetAxis("RightVCenter"));
    }

    private bool isControllerMovement(Vector2 controllerAxis) {
        return controllerAxis.x != 0 || controllerAxis.y != 0;
    }

    private bool isMouseMovement() {
        return Input.GetAxis("Mouse X") != 0 || Input.GetAxis("Mouse Y") != 0;
    }

    private Vector3 clampAxisMovement(Vector3 axisMovement) {
        float speed = axisMovement.magnitude;
        speed = Mathf.Min(speed, this.AxisControllerMaxSpeed);
        return axisMovement.normalized * speed;
    }

    private void handlePosition() {
        Vector3 mouvement;
        Vector3 playerPortal;
        Vector2 controllerAxis;
        Vector3 offset = this._player.transform.position;


        if (Input.GetButton("CenterPortal")) {
            controllerAxis = this.getCenterControllerAxis();
        } else {
            controllerAxis = this.getControllerAxis();
        }

        // we check the controller first
        if (this.isControllerMovement(controllerAxis)) {
            if (Input.GetButton("CenterPortal")) {
                mouvement = new Vector3(
                    controllerAxis.x,
                    controllerAxis.y,
                    0) * this.MaxRadius;
                playerPortal = mouvement;
            } else {
                mouvement = new Vector3(
                    controllerAxis.x,
                    controllerAxis.y,
                    0) * this.AxisControllerTweak;
                mouvement = this.clampAxisMovement(mouvement);
                playerPortal = this._oldPosition + mouvement;
            }

            this._controllerMode = true;
        } else if (this.isMouseMovement()) {
            // then, we check the mouse
            playerPortal = this.getMouseMovement(offset);
            this._controllerMode = false;
        } else {
            // default behavior when no mouvement is detected
            if (Input.GetButton("CenterPortal")) {
                playerPortal = Vector3.zero;
            } else {
                if (this._controllerMode) {
                    playerPortal = this.transform.position - offset;
                } else {
                    playerPortal = this.getMouseMovement(offset);
                }
            }
        }

        playerPortal = this.clampPortalPosition(playerPortal);
        this._oldPosition = playerPortal;
        this.transform.position = playerPortal + offset;
    }

    private Vector3 getMouseMovement(Vector3 offset) {
        Vector3 mouvement
            = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouvement.z = 0;
        return mouvement - offset;
    }

    private Vector3 clampPortalPosition(Vector3 portalPosition) {
        float distance = portalPosition.magnitude;
        distance = Mathf.Clamp(distance,
            this.MinRadius,
            this.MaxRadius);
        portalPosition = portalPosition.normalized * distance;

        return portalPosition;
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
