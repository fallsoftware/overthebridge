using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour {
    public string RootScene;
    public GameObject PauseMenu;

    void Start () {
	
	}
	
	void Update () {
        if (this.PauseMenu != null && Input.GetButtonDown("Pause")) {
            this.LaunchPause();
    
        }
    }

    public void LaunchRootScene() {
        SceneManager.LoadScene(this.RootScene);
    }

    public void QuitGame() {
        Application.Quit();
    }

    public void LaunchPause() {
        if (this.PauseMenu == null) return;

        bool isPause = this.PauseMenu.activeSelf;

        if (isPause) {
            Time.timeScale = 1f;
        } else {
            Time.timeScale = 0f;
        }

        this.PauseMenu.SetActive(!isPause);
    }
}
