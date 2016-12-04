using UnityEngine;
using System.Collections;

public class Singleton : MonoBehaviour {
    public static Singleton SingletonObject;

    void Awake() {
        if (!Singleton.SingletonObject) {
            Singleton.SingletonObject = this;
            DontDestroyOnLoad(this.gameObject);
        } else {
            Destroy(this.gameObject);
        }
    }

	void Start() {
	
	}

	void Update() {
	
	}
}