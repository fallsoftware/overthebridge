using UnityEngine;
using System.Collections;

public class Checkpoint : MonoBehaviour {
    public LevelManager levelManager;
    public int Index;

	void Start () {
	}
	
	void Update () {	
	}

    public void OnTriggerEnter2D(Collider2D collider2D) {
        if (this.levelManager.LastCheckpoint.Index <= this.Index) {
            this.levelManager.UpdateCheckpoint(this);
        }
    }
}
