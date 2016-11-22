using UnityEngine;
using System.Collections;

public class DeathManager : MonoBehaviour {
    public LevelManager LevelManager;

    void Start() {
        if (LevelManager == null) return;
    }

    void Update() {
        if (LevelManager == null) return;
    }

    public void OnTriggerEnter2D(Collider2D collider2D) {
        if (collider2D.tag != "Player") return;

        this.LevelManager.OopsPlayerIsDead();
    }
}
