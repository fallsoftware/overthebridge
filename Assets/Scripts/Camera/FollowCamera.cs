using UnityEngine;
using UnityEngine.Networking.Match;

public class FollowCamera : MonoBehaviour {
    public Vector3 offset = new Vector3(0f, 0f, 0f);

    private void LateUpdate() {
        transform.position = new Vector3(Camera.main.transform.position.x,
            Camera.main.transform.position.y, 0) + offset;
    }
}
