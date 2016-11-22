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
    private bool _firstLoad = false;
    private bool _unloadAll = false;
    private bool _loadAll = false;
    private string _toLoadNext = null;
    private string _sceneToPlacePlayer = null;

	void Start() {
	    LevelController.StaticRef = this;
        this.LoadFirstLevel(this.FirstLevelToLoad);
    }
	
	void Update() {
        if (this._loadAll) {
            this.loadAllLevels();
        }

        if (this._unloadAll) {
	        this.unloadAllLevels();
	        this._unloadAll = false;
	    }
    }

    public void LoadLevel(string levelNameToLoad, bool firstLoad = false) {
        Scene levelToLoad = SceneManager.GetSceneByName(levelNameToLoad);
        //this.LevelsLoaded.Add(levelNameToLoad);

        if (levelToLoad.isLoaded) return;
    
        SceneManager.LoadScene(levelNameToLoad, LoadSceneMode.Additive);

        this._toSetup = true;
        this._firstLoad = firstLoad;
    }

    public void LoadFirstLevel(string levelNameToLoad, 
        bool toCheckpoint = true) {
        Scene levelToLoad = SceneManager.GetSceneByName(levelNameToLoad);

        if (levelToLoad.isLoaded) return;

        SceneManager.LoadScene(levelNameToLoad, LoadSceneMode.Additive);

        if (!this.LevelsLoaded.Contains(levelNameToLoad)) {
            this.LevelsLoaded.Add(levelNameToLoad);
        }

        this._firstLoad = toCheckpoint;
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

        this._loadAll = true;
    }

    private void loadAllLevels() {
        int levelsLoadedNumber = this.LevelsLoaded.Count;

        if (levelsLoadedNumber <= 0) return;

        this.LoadFirstLevel(this.LevelsLoaded[0], levelsLoadedNumber == 1); // TO FIX YOU FUCK like you have to know which and when
        this._toLoadNext = this.LevelsLoaded[0];
    }

    public void ReloadLevels(string sceneToPlacePlayer = null) {
        int levelsLoadedNumber = this.LevelsLoaded.Count;

        for (int i = 0; i < levelsLoadedNumber; i++) {
            Destroy(LevelLoader.FindRoot
                (SceneManager.GetSceneByName(this.LevelsLoaded[i])));
        }

        this._unloadAll = true;
        this._sceneToPlacePlayer = sceneToPlacePlayer;
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

        if (this._firstLoad) {
            this.firstLoadSetup(levelLoader);
        }

        if (!this.LevelsLoaded.Contains(levelLoader.Level) 
            && !String.IsNullOrEmpty(levelLoader.Level)) {
            int indexToInsert = 0;

            if (levelLoader.IsNext) {
                this.LevelsLoaded.Add(levelLoader.Level);
            } else {
                indexToInsert = this.LevelsLoaded.IndexOf(levelLoader.NextLevel);
                this.LevelsLoaded.Insert(indexToInsert, levelLoader.Level);
            }
        }

        if (this._loadAll) {
            if (!String.IsNullOrEmpty(this._toLoadNext)) {
                int next = this.LevelsLoaded.IndexOf(this._toLoadNext);

                if (next > this.LevelsLoaded.Count-1 || next < 0 || this.LevelsLoaded.Count == 1) {
                    this._toLoadNext = null;
                    this._loadAll = false;
                    this._sceneToPlacePlayer = null;
                } else {
                    this._toLoadNext = this.LevelsLoaded[next + 1];
                    this.LoadLevel(this._toLoadNext, 
                        this._toLoadNext == this._sceneToPlacePlayer);
                }

            }
        }
    }

    private void firstLoadSetup(LevelLoader levelLoader) {
        GameObject root = LevelLoader.FindRoot(levelLoader.gameObject.scene);
        LevelManager levelManager = root.GetComponent<LevelManager>();
        levelManager.SetPlayerToLastCheckpoint();
    }
}
