using UnityEngine;
using System.Collections;

public class ObjectFocusedScroller : MonoBehaviour {
    public Vector2 Speed = new Vector2(2, 2);
    public Vector2 Direction = new Vector2(-1, 0);
    public string ObjectTag;
    private Vector2 _previousObjectPosition;
    private GameObject _objectToScrollWith;

    void Start() {
        this._objectToScrollWith = GameObject.FindWithTag(this.ObjectTag);

        if (this._objectToScrollWith != null) {
            this._previousObjectPosition
                = this._objectToScrollWith.transform.position;
        }
    }

    void Update() {
        Vector2 movement = new Vector2(
            this.Speed.x * this.Direction.x,
            this.Speed.y * this.Direction.y);
        Vector2 currentObjectPosition 
            = this._objectToScrollWith.transform.position;

        if (this._previousObjectPosition == currentObjectPosition) return;

        Vector2 directionToMove
            = currentObjectPosition - this._previousObjectPosition;
        directionToMove = directionToMove.normalized;
        movement *= Time.deltaTime;
        movement.x *= directionToMove.x;
        movement.y *= directionToMove.y;
        this.transform.Translate(movement);
        this._previousObjectPosition = currentObjectPosition;
    }
}