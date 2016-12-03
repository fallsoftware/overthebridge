using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {
    public LevelManager LevelManager {
        get { return this.levelManager; }
        set {
            this.levelManager = value;
            this.levelManager.SetAmbiance();
        }
    }

    public LevelManager levelManager;
    public EntityDeath Player;
    public PortalControllerScript Portal;
    public EntityManager EntityManager;
    private LevelFading _levelFading;

    public static GameManager Instance = null;

    void Start() {
        if (GameManager.Instance == null) {
            GameManager.Instance = this;
        } else if (GameManager.Instance != this) {
            Destroy(this.gameObject);
        }

        GameObject levelController 
            = GameObject.FindGameObjectWithTag("LevelController");
	    this._levelFading = levelController.GetComponent<LevelFading>();
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
