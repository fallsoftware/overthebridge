using System;
using UnityEngine;
using System.Collections;
using UnityEditor;
using UnityEngine.SceneManagement;

public class LoaderObject : MonoBehaviour {
    public LevelLoader LevelLoader;
    [HideInInspector] public bool FromRight = false;
    private string _levelToUnload = null;

    void Start() {
    }

    void Update() {
        if (!String.IsNullOrEmpty(this._levelToUnload)) {
            // should check if any physics interactions before...
            LevelController.StaticRef.UnloadLevel(this._levelToUnload);

            this._levelToUnload = null;
        }
    }

    public void HandleLevel() {
        Scene previousLevel
            = SceneManager.GetSceneByName(this.LevelLoader.PreviousLevel);
        Scene nextLevel
            = SceneManager.GetSceneByName(this.LevelLoader.NextLevel);
        string levelToAdd = null;
        string levelToRemove = null;

        if (previousLevel.isLoaded) { // the player goes to the right
            levelToAdd = this.LevelLoader.NextLevel;
            levelToRemove = this.getLevelBeforePreviousLevel(previousLevel);
        } else if (nextLevel.isLoaded) { // the player goes to the left
            levelToAdd = this.LevelLoader.PreviousLevel;
            levelToRemove = this.getLevelAfterNextLevel(nextLevel);
        } else { // first level loaded
            levelToAdd = this.LevelLoader.NextLevel;
        }

        if (!String.IsNullOrEmpty(levelToAdd)) {
            this.addLevel(levelToAdd);
        }

        if (!String.IsNullOrEmpty(levelToRemove)) {
            this.removeLevel(levelToRemove);
        }
    }

    private void removeLevel(string level) {
        if (!SceneManager.GetSceneByName(level).isLoaded) return;

        Destroy(LevelLoader.FindRoot(SceneManager.GetSceneByName(level)));
        this._levelToUnload = level;
    }

    private void addLevel(string level) {
        LevelController.StaticRef.LoadLevel(level);
    }

    private string getLevelBeforePreviousLevel(Scene level) {
        GameObject root = LevelLoader.FindRoot(level);

        if (root == null) return null;

        LevelLoader levelLoader = root.GetComponent<LevelLoader>();

        return levelLoader.PreviousLevel;
    }

    private string getLevelAfterNextLevel(Scene level) {
        GameObject root = LevelLoader.FindRoot(level);

        if (root == null) return null;

        LevelLoader levelLoader = root.GetComponent<LevelLoader>();

        return levelLoader.NextLevel;
    }
}