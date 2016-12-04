using UnityEngine;
using System.Collections;
using DigitalRuby.RainMaker;

public class SetCameraToMakeItRain : MonoBehaviour {
    public RainScript2D RainScript2D;

	void Awake() {
	    this.RainScript2D.Camera = Camera.main;
	}

    void Start() {
        
    }

	void Update() {
	
	}
}
