using UnityEngine;
using System.Collections;
using DigitalRuby.RainMaker;
using UnityEngine.UI;

public class EndTrigger : MonoBehaviour {
    public Renderer BlurPanel;
    public SpriteRenderer BlackPanel;
    public Image OverTheBridgeLogo;
    public float FadingSpeed = 1f;
    public float BlackFadingSpeed = .5f;
    public float ScaleSpeed = 0.01f;
    public float BlurringSpeed = 1f;
    public float RainFadingSpeed = .1f;
    public Rigidbody2D PlayerBody;
    public float FinalLogoScale = 1f;
    public RainScript2D RainScript2D;
    public SetRain SetRain;
    public Ambiance FinalSound;
    public float FinalSoundDuration = 10f;

    private float _blurXY;

	void Start () {
	    this._blurXY = this.BlurPanel.material.GetFloat("_blurSizeXY");
        this.BlurPanel.material.SetFloat("_blurSizeXY", 0);
	    this.OverTheBridgeLogo.color 
            = this.setAlpha(this.OverTheBridgeLogo.color, 0);
	    this.BlackPanel.color = this.setAlpha(this.BlackPanel.color, 0);
	    this.PlayerBody 
            = GameObject.FindGameObjectWithTag("Player")
            .GetComponent<Rigidbody2D>();
	}
	
	void Update () {
	
	}

    private Color setAlpha(Color color, float alpha) {
        return new Color(color.r, color.b, color.g, alpha);
    }

    void OnTriggerEnter2D(Collider2D collider2D) {
        if (collider2D.gameObject.tag != "Player") return;

        StartCoroutine(this.makeTheMagicHappen());
    }

    private IEnumerator makeTheMagicHappen() {
        this.PlayerBody.constraints = RigidbodyConstraints2D.FreezeAll;
        this.RainScript2D.EnableWind = false;
        this.SetRain.IsRaining = false;
        SoundManager.Instance.SwitchAmbiance(this.FinalSound.gameObject);

        while (!this.checkIfFinished()) {
            this.shiftAlphaBlackTexture();
            this.shiftAlphaLogo();
            this.shiftBlurriness();
            this.shiftLogoScale();
            this.shiftRainIntensity();

            yield return null;
        }

        yield return new WaitForSeconds(this.FinalSoundDuration);

        StartCoroutine(SoundManager.Instance.DeleteAmbiances());
    }

    private bool checkIfFinished() {
        return this.BlurPanel.material.GetFloat("_blurSizeXY") >= this._blurXY
               && this.OverTheBridgeLogo.color.a >= 1
               && this.BlackPanel.color.a >= 1
               && this.OverTheBridgeLogo.gameObject.transform.localScale.x 
               >= this.FinalLogoScale 
               && this.RainScript2D.RainIntensity <= 0;
    }

    private void shiftRainIntensity() {
        float rainIntensity = this.RainScript2D.RainIntensity;

        if (rainIntensity <= 0) return;

        this.RainScript2D.RainIntensity = this.shiftValue(rainIntensity, 
            -this.RainFadingSpeed);
    }

    private float shiftValue(float value, float factor) {
        return value += factor * Time.deltaTime;
    }

    private void shiftAlphaLogo() {
        if (this.OverTheBridgeLogo.color.a >= 1) return;

        this.OverTheBridgeLogo.color
            = this.setAlpha(this.OverTheBridgeLogo.color, this.shiftValue(
                this.OverTheBridgeLogo.color.a, this.FadingSpeed));
    }

    private void shiftAlphaBlackTexture() {
        if (this.BlackPanel.color.a >= 1) return;

        this.BlackPanel.color
            = this.setAlpha(this.BlackPanel.color, this.shiftValue(
                this.BlackPanel.color.a, this.BlackFadingSpeed));
    }

    private void shiftLogoScale() {
        Vector3 scale = this.OverTheBridgeLogo.transform.localScale;

        if (scale.x > this.FinalLogoScale) return;

        float newScale = this.shiftValue(scale.x, this.ScaleSpeed);

        this.OverTheBridgeLogo.transform.localScale = new Vector3(newScale, 
            newScale, newScale);
    }

    private void shiftBlurriness() {
        float newBlurriness = this.BlurPanel.material.GetFloat("_blurSizeXY");

        if (newBlurriness >= this._blurXY) return;

        newBlurriness = this.shiftValue(newBlurriness, this.BlurringSpeed);

        this.BlurPanel.material.SetFloat("_blurSizeXY", newBlurriness);
    }
}
