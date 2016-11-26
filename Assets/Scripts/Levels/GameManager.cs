using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {
    public LevelManager LevelManager;
    public PlayerDeath Player;

    private LevelFading _levelFading;

	void Start() {
	    this._levelFading = GameObject.FindObjectOfType<LevelFading>();
	}
	
	void Update() {
	
	}

    public IEnumerator GameOver() {
        yield return StartCoroutine(this.Player.MakePlayerDie());
        float fadeTime = this._levelFading.BeginFade(1);
        yield return new WaitForSeconds(fadeTime);
        LevelController.StaticRef.ReloadLevels(this.LevelManager);
        this.Player.MakePlayerLive();
        fadeTime = this._levelFading.BeginFade(-1);
        yield return new WaitForSeconds(fadeTime);
    }
}
