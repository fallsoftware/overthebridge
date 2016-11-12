using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelController : MonoBehaviour {
    public string FirstLevelToLoad;

    public static LevelController StaticRef;

    private LoaderObject _sourceLoaderObject;
    private bool _firstLoad = false;

	void Start() {
	    LevelController.StaticRef = this;
        this.LoadFirstLevel(this.FirstLevelToLoad);
    }
	
	void Update() {
	
	}

    public void LoadLevel(LoaderObject loaderObject, string levelNameToLoad) {
        Scene levelToLoad = SceneManager.GetSceneByName(levelNameToLoad);

        if (levelToLoad.isLoaded) return;
    
        SceneManager.LoadScene(levelNameToLoad, LoadSceneMode.Additive);

        this._sourceLoaderObject = loaderObject;
        this._firstLoad = false;
    }

    public void LoadFirstLevel(string levelNameToLoad) {
        Scene levelToLoad = SceneManager.GetSceneByName(levelNameToLoad);

        if (levelToLoad.isLoaded) return;

        SceneManager.LoadScene(levelNameToLoad, LoadSceneMode.Additive);

        this._firstLoad = true;
    }

    public void SetupLevel(LevelLoader levelLoader) {
        if (this._sourceLoaderObject) {
            Vector3 offset;

            if (levelLoader.IsNext) {
                offset = levelLoader.GetNextLevelOffset();
            } else {
                offset = levelLoader.GetPreviousLevelOffset();
            }

            levelLoader.gameObject.transform.Translate(offset);

            this._sourceLoaderObject = null;
        }

        if (this._firstLoad) {
            this.firstLoadSetup(levelLoader);
        }
    }

    private void firstLoadSetup(LevelLoader levelLoader) {
        GameObject root = LevelLoader.FindRoot(levelLoader.gameObject.scene);
        LevelManager levelManager = root.GetComponent<LevelManager>();
        levelManager.SetPlayerToLastCheckpoint();
    }
}
