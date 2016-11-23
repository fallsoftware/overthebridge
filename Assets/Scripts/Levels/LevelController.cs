using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelController : MonoBehaviour {
    public string FirstLevelToLoad;

    [HideInInspector]
    public List<string> LevelsLoaded;

    public static LevelController StaticRef;

    private bool _toSetup = false;
    private bool _placeToCheckpoint = false;
    private string _toLoadNext = null;
    private string _sceneToPlacePlayer = null;
    private Vector2 _locationToPlacePlayer;
    private bool _isLocation = false;


    void Start() {
	    LevelController.StaticRef = this;
        this.LoadFirstLevel(this.FirstLevelToLoad);
    }
	
	void Update() {
    }

    public void LoadLevel(string levelNameToLoad, bool setup = true, 
        bool placeToCheckpoint = false) {
        Scene levelToLoad = SceneManager.GetSceneByName(levelNameToLoad);

        if (levelToLoad.isLoaded) return;
    
        SceneManager.LoadScene(levelNameToLoad, LoadSceneMode.Additive);

        this._toSetup = setup;
        this._placeToCheckpoint = placeToCheckpoint;
    }

    public void LoadFirstLevel(string levelNameToLoad, 
        bool placeToCheckpoint = true) {
        this.LoadLevel(levelNameToLoad, false, placeToCheckpoint);

        if (!this.LevelsLoaded.Contains(levelNameToLoad)) {
            this.LevelsLoaded.Add(levelNameToLoad);
        }
        
    }

    public void UnloadLevel(string levelNameToUnload) {
        this.LevelsLoaded.Remove(levelNameToUnload);
        SceneManager.UnloadScene(levelNameToUnload);
    }

    private void unloadAllLevels() {
        int levelsLoadedNumber = this.LevelsLoaded.Count;

        for (int i = 0; i < levelsLoadedNumber; i++) {
            SceneManager.UnloadScene(this.LevelsLoaded[i]);
        }
    }

    private void loadAllLevels() {
        int levelsLoadedNumber = this.LevelsLoaded.Count;

        if (levelsLoadedNumber <= 0) return;

        this._toLoadNext = this.LevelsLoaded[0];

        this.LoadFirstLevel(
            this.LevelsLoaded[0], 
            this.LevelsLoaded[0] 
                == this._sceneToPlacePlayer);
    }

    public void ReloadLevels(LevelManager levelManager = null) {
        int levelsLoadedNumber = this.LevelsLoaded.Count;

        for (int i = 0; i < levelsLoadedNumber; i++) {
            Destroy(LevelLoader.FindRoot
                (SceneManager.GetSceneByName(this.LevelsLoaded[i])));
        }

        if (levelManager != null) {
            this._sceneToPlacePlayer = levelManager.gameObject.scene.name;
            this._locationToPlacePlayer 
                = levelManager.LastCheckpoint.transform.position;
            this._isLocation = true;
        }

        this.unloadAllLevels();
        this.loadAllLevels();
    }

    public void SetupLevel(LevelLoader levelLoader) {
        if (this._toSetup) {
            Vector3 offset;

            if (levelLoader.IsNext) {
                offset = levelLoader.GetNextLevelOffset();
            } else {
                offset = levelLoader.GetPreviousLevelOffset();
            }

            levelLoader.gameObject.transform.Translate(offset);

            this._toSetup = false;
        }

        if (this._placeToCheckpoint) {
            this.placeToCheckpoint(levelLoader);
        }

        this.addLevelToList(levelLoader);
        this.loadNextLevel();
    }

    private void addLevelToList(LevelLoader levelLoader) {
        if (!this.LevelsLoaded.Contains(levelLoader.Level)
            && !String.IsNullOrEmpty(levelLoader.Level)) {
            if (levelLoader.IsNext) {
                this.LevelsLoaded.Add(levelLoader.Level);
            } else {
                int indexToInsert 
                    = this.LevelsLoaded.IndexOf(levelLoader.NextLevel);
                this.LevelsLoaded.Insert(indexToInsert, levelLoader.Level);
            }
        }
    }

    private void loadNextLevel() {
        if (!String.IsNullOrEmpty(this._toLoadNext)) {
            int next = this.LevelsLoaded.IndexOf(this._toLoadNext);

            if (next >= this.LevelsLoaded.Count - 1 
                || next < 0 
                || this.LevelsLoaded.Count == 1) {
                this._toLoadNext = null;
                this._sceneToPlacePlayer = null;
                this._isLocation = false;
            } else {
                this._toLoadNext = this.LevelsLoaded[next + 1];
                this.LoadLevel(this._toLoadNext, true,
                    this._toLoadNext 
                        == this._sceneToPlacePlayer);
            }

        }
    }

    private void placeToCheckpoint(LevelLoader levelLoader) {
        GameObject root = LevelLoader.FindRoot(levelLoader.gameObject.scene);
        LevelManager levelManager = root.GetComponent<LevelManager>();

        if (!this._isLocation) {
            levelManager.SetPlayerToLastCheckpoint();
        } else {
            levelManager.SetPlayerToLocation(this._locationToPlacePlayer);
        }
    }
}
