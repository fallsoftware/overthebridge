using UnityEngine;
using System.Collections;

public class DeathManager : MonoBehaviour {
    public LevelManager levelManager;

    void Start() {
        if (levelManager == null) return;
    }

    void Update() {
        if (levelManager == null) return;
    }

    public void OnTriggerEnter2D(Collider2D collider2D) {
        this.levelManager.SetPlayerToLastCheckpoint();
    }
}
