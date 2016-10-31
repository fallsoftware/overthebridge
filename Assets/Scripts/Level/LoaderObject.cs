using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public enum Location {
    LEFT,
    RIGHT
}

public class LoaderObject : MonoBehaviour {
    public string LevelName = "LevelName";
    public string LevelRoot = "LevelRoot";
    public Location Location;
    [HideInInspector] public bool FromRight = false;
    private bool toUnload = false;

    void Start() {
    }

    void Update() {
        if (this.toUnload) {
            // should check if any physics interactions before...
            SceneManager.UnloadScene(this.LevelName);
            this.toUnload = false;
        }
    }

    public void HandleLevel() {
        if (this.Location == Location.RIGHT) {
            this.HandleRight();
        } else if (this.Location == Location.LEFT) {
            this.HandleLeft();
        }
    }

    public void HandleLeft() {
        LoaderThreshold loaderThreshold = (LoaderThreshold)
            GameObject.FindObjectOfType(typeof(LoaderThreshold));

        if (loaderThreshold.FromRight) {
            this.AddScene();
        } else {
            this.RemoveScene();
        }
    }

    public void HandleRight() {
        LoaderThreshold loaderThreshold = (LoaderThreshold) 
            GameObject.FindObjectOfType(typeof(LoaderThreshold));

        if (!loaderThreshold.FromRight) {
            this.AddScene();
        } else {
            this.RemoveScene();
        }
    }

    public void RemoveScene() {
        GameObject levelToDestroy = GameObject.Find(this.LevelRoot);
        Destroy(levelToDestroy);
        this.toUnload = true;
    }

    public void AddScene() {
        LevelController.StaticRef.LoadLevel(this);
    }
}