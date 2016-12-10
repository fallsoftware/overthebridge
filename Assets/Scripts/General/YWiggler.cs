using UnityEngine;
using System.Collections;

public class YWiggler : MonoBehaviour {
    private float YDelta = 3f;
    private float YSpeed = 2f;

    private Vector3 _position;
    private float _minY;
    private float _maxY;
    private int _direction = 1;

    void Start () {
        this._position = this.transform.position;
        this._minY = this._position.y - this.YDelta;
        this._maxY = this._position.y + this.YDelta;
    }

	void Update () {
	    this.changePosition();
	}

    private void changePosition() {
        if (this._position.y - this._minY < 0.2f 
            || this._position.y - this._maxY > 0.2f) {
            this._direction = -this._direction;
        }

        float newY
            = this._direction * this.YSpeed * Time.deltaTime 
                + this._position.y;

        this._position = new Vector3(this._position.x, newY, this._position.z);
        this.transform.position = this._position;
    }
}
