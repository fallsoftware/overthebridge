using UnityEngine;
using System.Collections;

public class FadeDistance : MonoBehaviour {
    public float MinDistance = 10f;
    public float MaxDistance = 100f;
    public float Tolerance = .2f;

    private TextMesh _textMesh;
    private Color _color;
    private float _alpha;
    private Transform _player;

	// Use this for initialization
	void Start () {
	    this._player 
            = GameObject.FindGameObjectWithTag("Player")
            .GetComponent<Transform>();
	    this._textMesh = this.GetComponent<TextMesh>();
	    this._color = this._textMesh.color;
	}
	
	// Update is called once per frame
	void Update () {
        this.setAlpha();
	    this._textMesh.color = new Color(this._color.r, this._color.g, 
            this._color.b, this._alpha);
    }

    private float computeDistance() {
        return (this.transform.position - this._player.position).magnitude;
    }

    private void setAlpha() {
        float distance = this.computeDistance();

        if (distance - this.MinDistance < this.Tolerance) {
            this._alpha = 1f;
            return;
        }

        if (this.MaxDistance - distance < this.Tolerance) {
            this._alpha = 0f;
            return;
        }

        this._alpha = (this.MaxDistance - distance)
            / (this.MaxDistance - this.MinDistance);
    }
}