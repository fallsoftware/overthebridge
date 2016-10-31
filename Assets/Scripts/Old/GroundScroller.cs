using UnityEngine;
using System.Collections;

public class GroundScroller : MonoBehaviour {
    private Vector3 _previousObjectMovement;
    private GameObject _objectToScrollWith;
    private string _objectTag = "Player";

    void Start() {
        this._objectToScrollWith = GameObject.FindWithTag(this._objectTag);

        if (this._objectToScrollWith != null) {
            this._previousObjectMovement
                = this._objectToScrollWith.transform.position;
        }
    }

    void Update() {
        if (this._objectToScrollWith == null) return;

        Vector3 objectMovement
            = this._objectToScrollWith.transform.position
              - this._previousObjectMovement;
        objectMovement *= 0.02f;
        if (objectMovement != new Vector3(0, 0, 0)) {
            this.transform.Translate(-objectMovement);
        }
    }
}
