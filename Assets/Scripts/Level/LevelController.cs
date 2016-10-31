using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelController : MonoBehaviour {
    public GameObject LoaderThreshold;
    public static LevelController StaticRef;

    private LoaderObject _sourceLoaderObject;

	void Start() {
	    LevelController.StaticRef = this;
	    LoaderObject loaderObject = 
            (LoaderObject) GameObject.FindObjectOfType(typeof(LoaderObject));
        this.LoadLevel(loaderObject);
    }
	
	void Update() {
	
	}

    public void LoadLevel(LoaderObject loaderObject) {
        if (loaderObject.tag == "Processed") return;

        loaderObject.tag = "Processed";

        string levelToLoad = this.GetLevelToLoad(loaderObject);

        if (levelToLoad == null) return;

        Scene sceneToLoad = SceneManager.GetSceneByName(levelToLoad);

        if (sceneToLoad.isLoaded) return;
    
        SceneManager.LoadScene(levelToLoad, LoadSceneMode.Additive);
        this._sourceLoaderObject = loaderObject;
    }

    public void SetupLevel(LevelHandler levelHandler) {
        if (levelHandler.tag == "Processed") return;

        levelHandler.tag = "Processed";

        if (this._sourceLoaderObject) {
            Vector3 offset = levelHandler.GetOffset();
            levelHandler.gameObject.transform.Translate(offset);

            this._sourceLoaderObject = null;
        }
    }

    private string GetLevelToLoad(LoaderObject loaderObject) {
        string levelToLoad = null;

        levelToLoad = loaderObject.LevelName;

        if (String.IsNullOrEmpty(levelToLoad)) {
            return null;
        }

        return levelToLoad;
    }
}
