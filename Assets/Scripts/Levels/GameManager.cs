using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {
    public LevelManager LevelManager;
    public EntityDeath Player;
    public PortalControllerScript Portal;
    public EntityManager EntityManager;
    private LevelFading _levelFading;

	void Start() {
	    this._levelFading = GameObject.FindObjectOfType<LevelFading>();
        this.EntityManager = GameObject.FindObjectOfType<EntityManager>();
    }
	
	void Update() {
	
	}

    public IEnumerator GameOver() {
        yield return StartCoroutine(this.Player.MakeEntityDie());
        float fadeTime = this._levelFading.BeginFade(1);
        
        yield return new WaitForSeconds(fadeTime);
        EntityManager.DespawnAll();
        Portal.changeToNotSetState();
        
        LevelController.StaticRef.ReloadLevels(this.LevelManager);
        
        fadeTime = this._levelFading.BeginFade(-1);
        this.Player.MakeEntityLive();
        yield return new WaitForSeconds(fadeTime);
        
    }
}
