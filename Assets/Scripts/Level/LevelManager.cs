using UnityEngine;
using System.Collections;

public class LevelManager : MonoBehaviour {
    public Checkpoint LastCheckpoint;
    [HideInInspector] public bool SetToCheckpointAtStart = false;
    [HideInInspector] public GameObject player;

	void Start () {	    
	    this.player = GameObject.FindGameObjectWithTag("Player");

        if (this.SetToCheckpointAtStart) {
            this.SetPlayerToLastCheckpoint();
        }
    }

    void Update() {

    }

    public void SetPlayerToLastCheckpoint() {
        this.player.transform.position
            = this.LastCheckpoint.gameObject.transform.position;
    }

    public void SetPlayerToCheckpoint(Checkpoint checkpoint) {
        this.LastCheckpoint = checkpoint;
        this.SetPlayerToLastCheckpoint();
    }

    public void UpdateCheckpoint(Checkpoint checkpoint) {
        this.LastCheckpoint = checkpoint;
    }
}
