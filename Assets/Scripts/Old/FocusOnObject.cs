using UnityEngine;

public class FocusOnObject : MonoBehaviour {
    public string FocusTag = "Player";
    //private Vector3 offset;
    private GameObject _focusObject;
    private Vector3 _previousObjectPosition;

    void Start() {
        this._focusObject = GameObject.FindWithTag(this.FocusTag);

        if (this._focusObject != null) {
            this._previousObjectPosition = this._focusObject.transform.position;
            /*this.offset
                = this.previousObjectPosition - this.transform.position;*/
        }
    }

    void Update() {
        if (this._focusObject == null) return;

        Vector3 currentObjectPosition
            = this._focusObject.transform.position;
        Vector3 movement
            = currentObjectPosition - this._previousObjectPosition;
        this.transform.position += movement;
        this._previousObjectPosition = currentObjectPosition;
    }
}