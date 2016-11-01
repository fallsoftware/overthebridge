using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour {
    public string PreviousLevel;
    public string NextLevel;

    [HideInInspector] public bool IsNext;

    public static string LeftAnchorName = "LeftAnchor";
    public static string RightAnchorName = "RightAnchor";
    public static string RootTag = "Root";

    void Start() {
        this.SetLevelPositionAtLoading();
        LevelController.StaticRef.SetupLevel(this);
    }

    void Update() {
	
	}

    public void OnTriggerExit() {
        LevelLoader.Destroy(gameObject);
    }

    public void SetLevelPositionAtLoading(string otherSceneName) {
        if (otherSceneName == this.PreviousLevel) {
            this.IsNext = true;
        } else if (otherSceneName == this.NextLevel) {
            this.IsNext = false;
        }
    }

    public void SetLevelPositionAtLoading() {
        Scene previousLevel = SceneManager.GetSceneByName(this.PreviousLevel);
        Scene nextLevel = SceneManager.GetSceneByName(this.NextLevel);

        if (previousLevel.isLoaded) {
            this.IsNext = true;
        } else if (nextLevel.isLoaded) {
            this.IsNext = false;
        }
    }

    public Vector3 GetOffset(Scene scene, string anchorName) {
        Transform anchor = this.getLevelAnchor(scene, anchorName);

        if (anchor == null) return Vector3.zero;

        return anchor.transform.position;
    }

    public Vector3 GetPreviousLevelOffset() {
        Vector3 levelOffset = this.GetOffset(
            this.getLevel(this.NextLevel),
            LevelLoader.LeftAnchorName);
        Vector3 previousLevelOffset 
            = this.GetLevelOffset(LevelLoader.RightAnchorName);

        return previousLevelOffset - levelOffset;
    }

    public Vector3 GetNextLevelOffset() {
        Vector3 levelOffset = this.GetOffset(
            this.getLevel(this.PreviousLevel),
            LevelLoader.RightAnchorName);
        Vector3 nextLevelOffset 
            = this.GetLevelOffset(LevelLoader.LeftAnchorName);

        return levelOffset - nextLevelOffset;
    }

    public Vector3 GetLevelOffset(string anchorName) {
        return this.GetOffset(this.gameObject.scene, anchorName);
    }

    public static GameObject FindRoot(Scene scene) {
        GameObject[] gameObjects = scene.GetRootGameObjects();

        foreach (GameObject gameObject in gameObjects) {
            if (gameObject.tag == LevelLoader.RootTag) {
                return gameObject;
            }
        }

        return null;
    }

    private Transform getLevelAnchor
        (Scene otherLevel, string anchorName) {
        GameObject root = LevelLoader.FindRoot(otherLevel);

        if (root == null) return null;

        Transform[] rightRootChildren
            = root.gameObject.GetComponentsInChildren<Transform>();

        foreach (Transform transform in rightRootChildren) {
            if (transform.gameObject.name == anchorName) {
                return transform;
            }
        }

        return null;
    }

    private Scene getLevel(string sceneName) {
        return SceneManager.GetSceneByName(sceneName);
    }
}