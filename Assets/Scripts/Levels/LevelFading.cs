using UnityEngine;
using System.Collections;

public class LevelFading : MonoBehaviour {
    public Texture2D FadeOutTexture;
    public float FadingSpeed = .8f;

    private int _drawDepth = -1000; // to be drawn last
    private float _alpha = 1.0f; // default alpha
    private int _fadeDirection = -1; // -1 to be fadeIn (-1) or fadeOut (1)

    void Start () {
	
	}
		
	void Update () {
	
	}

    void OnGUI() {
        this._alpha += this._fadeDirection * this.FadingSpeed * Time.deltaTime;
        this._alpha = Mathf.Clamp01(this._fadeDirection);
        GUI.color 
            = new Color(GUI.color.r, GUI.color.g, GUI.color.b, this._alpha);
        GUI.depth = this._drawDepth;

        GUI.DrawTexture(
            new Rect(0, 0, Screen.width, 
            Screen.width), this.FadeOutTexture);
    }

    public float BeginFade(int fadeDirection) {
        this._fadeDirection = fadeDirection;

        return (this.FadingSpeed);
    }
}
