using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {
    public  LevelManager LevelManager;

    private LevelFading _levelFading;

	void Start() {
	    this._levelFading = GameObject.FindObjectOfType<LevelFading>();
	}
	
	void Update() {
	
	}

    public IEnumerator GameOver() {
        float fadeTime = this._levelFading.BeginFade(1);
        yield return new WaitForSeconds(fadeTime);
        LevelController.StaticRef.ReloadLevels(this.LevelManager);
        fadeTime = this._levelFading.BeginFade(-1);
        yield return new WaitForSeconds(fadeTime);
    }
}
