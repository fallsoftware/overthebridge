using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour {
    public string RootScene;
    public GameObject PauseMenu;
    public bool IsPause = false;

    void Start () {
	
	}
	
	void Update () {
        if (this.PauseMenu != null && Input.GetButtonDown("Pause")) {
            this.LaunchPause();
    
        }
    }

    public void LaunchRootScene() {
        SceneManager.LoadScene(this.RootScene);
        Time.timeScale = 1f;
    }

    public void QuitGame() {
        Application.Quit();
    }

    public void LaunchPause() {
        this.IsPause = !this.IsPause;

        if (this.IsPause) {
            Time.timeScale = 0f;
        } else {
            Time.timeScale = 1f;
        }

        this.PauseMenu.SetActive(this.IsPause);
    }
}
