using UnityEngine;

public class LevelHandler : MonoBehaviour {
    public string Root;
    public string PreviousRoot;
    private Transform _previousAnchor;
    private Transform _anchor;

    void Start() {
        LevelController.StaticRef.SetupLevel(this);
    }

    void Update() {
	
	}

    public void OnTriggerExit() {
        LevelHandler.Destroy(gameObject);
    }

    public Vector3 GetOffset() {
        Vector3 previousLevelOffset = this.GetPreviousLevelOffset();
        Vector3 nextLevelOffset = this.GetLevelOffset();

        return previousLevelOffset - nextLevelOffset;
    }

    private void getPreviousAnchor() {
        GameObject rightRoot = GameObject.Find(this.PreviousRoot);
        Transform[] rightRootChildren
            = rightRoot.gameObject.GetComponentsInChildren<Transform>();

        foreach (Transform transform in rightRootChildren) {
            if (transform.gameObject.name == "RightAnchor") {
                this._previousAnchor = transform;
            }
        }
    }

    private void getAnchor() {
        GameObject leftRoot = GameObject.Find(this.Root);
        Transform[] leftRootChildren
            = leftRoot.gameObject.GetComponentsInChildren<Transform>();

        foreach (Transform transform in leftRootChildren) {
            if (transform.gameObject.name == "LeftAnchor") {
                this._anchor = transform;
            }
        }
    }

    private Vector3 GetPreviousLevelOffset() {
        this.getPreviousAnchor();

        return this._previousAnchor.transform.position;
    }

    private Vector3 GetLevelOffset() {
        this.getAnchor();

        return this._anchor.transform.position;
    }
}