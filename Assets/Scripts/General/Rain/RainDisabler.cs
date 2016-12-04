using UnityEngine;
using System.Collections;

public class RainDisabler : MonoBehaviour {
    public SetRain SetRain;

    void Start () {
	
	}

	void Update () {
	
	}

    void OnTriggerEnter2D(Collider2D collider2D) {
        if (collider2D.gameObject.tag != "Player") return;

        if (!SetRain.IsRaining) return;

        StartCoroutine(SetRain.NoRainAnymore());
    }
}
