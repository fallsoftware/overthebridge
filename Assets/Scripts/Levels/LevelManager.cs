using UnityEngine;
using System.Collections;

public class LevelManager : MonoBehaviour {
    public Checkpoint LastCheckpoint;
    [HideInInspector] public bool SetToCheckpointAtStart = false;
    [HideInInspector] public GameObject player;
    [HideInInspector] private GameManager _gameManager;

    void Awake() {
        this.player = GameObject.FindGameObjectWithTag("Player");
    }

	void Start () {
	    if (this.player == null) {
	        this.player = GameObject.FindGameObjectWithTag("Player");
	    }

	    this._gameManager = GameObject.FindObjectOfType<GameManager>();

        if (this.SetToCheckpointAtStart) {
            this._gameManager.LevelManager = this;
            this.SetToCheckpointAtStart = false;
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
        this._gameManager.LevelManager = this;
        this.LastCheckpoint = checkpoint;
    }

    public void SetPlayerToLocation(Vector2 location) {
        this.player.transform.position = location;
    }

    public void OopsPlayerIsDead() {
        StartCoroutine(this._gameManager.GameOver());
    }
}
