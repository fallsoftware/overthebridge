using UnityEngine;
using System.Collections;
using DigitalRuby.RainMaker;

public class SetRain : MonoBehaviour {
    public ParticleSystem[] ParticleSystems;
    public RainScript2D RainScript2D;
    public string SortingLayer = "UI";
    public float FadeSpeed = 0.01f;
    public bool IsRaining = false;

    private float _rainIntensity;

	void Start () {
	    int size = this.ParticleSystems.Length;

	    for (int i = 0; i < size; i++) {
	        this.ParticleSystems[i].GetComponent<Renderer>().sortingLayerName
	            = this.SortingLayer;
	    }

	    this._rainIntensity = RainScript2D.RainIntensity;
	    RainScript2D.RainIntensity = 0;
        DontDestroyOnLoad(this.gameObject);
	}

	void Update () {
	
	}

    public IEnumerator MakeItRain() {
        while (this.RainScript2D.RainIntensity < this._rainIntensity) {
            float oldIntensity = this.RainScript2D.RainIntensity;
            this.RainScript2D.RainIntensity 
                = Mathf.Min(oldIntensity + this.FadeSpeed * Time.time, 
                this._rainIntensity);

            yield return null;
        }

        this.IsRaining = true;
    }

    public IEnumerator NoRainAnymore() {
        while (this.RainScript2D.RainIntensity > 0) {
            float oldIntensity = this.RainScript2D.RainIntensity;
            this.RainScript2D.RainIntensity
                = Mathf.Min(oldIntensity - this.FadeSpeed * Time.time,
                this._rainIntensity);

            yield return null;
        }

        this.IsRaining = false;
    }
}
