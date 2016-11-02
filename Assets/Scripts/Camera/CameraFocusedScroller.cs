using UnityEngine;

public class CameraFocusedScroller : MonoBehaviour {
    public Vector2 Speed = new Vector2(2, 2);
    public Vector2 Direction = new Vector2(-1, 0);
    private Vector2 _previousCameraMovement;

    void Start() {
        this._previousCameraMovement 
            = this.getIntVector(Camera.main.transform.position);
    }

    void Update() {
        Vector2 movement = new Vector2(
            this.Speed.x * this.Direction.x,
            this.Speed.y * this.Direction.y);
        Vector2 currentCameraMovement
            = this.getIntVector(Camera.main.transform.position);

        if (this._previousCameraMovement == currentCameraMovement) return;

        Vector2 directionToMove 
            = currentCameraMovement - this._previousCameraMovement;
        directionToMove = directionToMove.normalized;
        movement *= Time.deltaTime;
        movement.x *= directionToMove.x;
        movement.y *= directionToMove.y;
        this.transform.Translate(movement);
        this._previousCameraMovement = currentCameraMovement;
    }

    private Vector2 getIntVector(Vector2 vector) {
        return new Vector2((int) vector.x, (int) vector.y);
    }
}