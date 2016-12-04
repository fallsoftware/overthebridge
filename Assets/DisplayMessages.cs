using UnityEngine;
using System.Collections;
using System.Runtime.CompilerServices;

public class DisplayMessages : MonoBehaviour {
    public float TimeDisplayed = 7f;
    public float FadingSpeed = 2f;
    public TextMesh TextDark;
    public TextMesh TextLight;

    private bool _hasDisplayed = false;
    private float _timeDisplayed;
    private Portal _portal;

    void Start() {
        this._portal = GameObject.FindGameObjectWithTag("Portal")
            .GetComponent<Portal>();
        this.setAlphaText(this.TextDark, 0);
        this.setAlphaText(this.TextLight, 0);
    }

    void Update() {
        if (!this._hasDisplayed) return;

        if (Time.time - this._timeDisplayed < this.TimeDisplayed) {
            int fadeDirection = this._portal.InDark ? 1 : -1;
            StartCoroutine(this.fadeText(this.TextDark, fadeDirection));
            StartCoroutine(this.fadeText(this.TextLight, -fadeDirection));

            return;
        }

        this.gameObject.SetActive(false);
    }

    void OnTriggerEnter2D(Collider2D collision) {
        if (this._hasDisplayed || collision.gameObject.name != "Player") {
            return;
        }

        this._hasDisplayed = true;
        this._timeDisplayed = Time.time;
    }

    private IEnumerator fadeText(TextMesh text, int direction) {
        direction = Mathf.Clamp(direction, -1, 1);
        float alphaGoal = direction == 1 ? 1 : 0;
        float alpha = text.color.a;

        while (Mathf.Abs(alpha - alphaGoal) > 0) {
            alpha += direction * this.FadingSpeed * Time.deltaTime;
            alpha = Mathf.Clamp01(alpha);
            this.setAlphaText(text, alpha);

            yield return null;
        }
    }

    private void setAlphaText(TextMesh text, float alpha) {
        text.color = new Color(
                text.color.r, text.color.g, text.color.b, alpha);
    }
}
