using UnityEngine;
using System.Collections;

public class ObjectMover : MonoBehaviour {
    public Vector3 ToMove = new Vector3(-1, 0, 0);
    private float _previousTime;    

    void Start() {
        this._previousTime = Time.time;
    }
	
	void Update() {
        System.Random r = new System.Random();
        int test = r.Next(1, 2);

        if (Time.time - _previousTime > 1) {
            this.ToMove = test == 1 ? new Vector3(1, 0, 0) 
                : new Vector3(-1, 0, 0);
        }

        _previousTime = Time.time;
        this.transform.position += this.ToMove;
    }
}
