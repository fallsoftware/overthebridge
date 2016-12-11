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
        this.BlackPanel.transform.position
            = Camera.main.ScreenToWorldPoint(
                new Vector3(Screen.width / 2, Screen.height / 2,
                Camera.main.nearClipPlane));
        this.BlackPanel.transform.position = new Vector3(this.BlackPanel.transform.position.x, this.BlackPanel.transform.position.y, 50);

	    this.BlurPanel.transform.position = this.BlackPanel.transform.position;
	}

    private Color setAlpha(Color color, float alpha) {
        return new Color(color.r, color.b, color.g, alpha);
    }

    void OnTriggerEnter2D(Collider2D collider2D) {
        if (collider2D.gameObject.tag != "Player") return;

        StartCoroutine(this.makeTheMagicHappen());
    }

    public IEnumerator makeTheMagicHappen() {
        this.PlayerBody.constraints = RigidbodyConstraints2D.FreezeAll;
        if (RainScript2D != null)
        {
            this.RainScript2D.EnableWind = false;
        }
        if (SetRain != null)
        {
            this.SetRain.IsRaining = false;
        }
        if (FinalSound != null)
        {
            SoundManager.Instance.SwitchAmbiance(this.FinalSound.gameObject);
        }
        this.resizeSpriteToScreen(this.BlackPanel);
        this.resizRendererToScreen(this.BlurPanel);

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
               >= this.FinalLogoScale;
    }

    private void shiftRainIntensity() {
        if (RainScript2D != null)
        {
            this.RainScript2D.EnableWind = false;
            float rainIntensity = this.RainScript2D.RainIntensity;

            if (rainIntensity <= 0) return;

            this.RainScript2D.RainIntensity = this.shiftValue(rainIntensity,
                -this.RainFadingSpeed);
        }
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

    private void resizeSpriteToScreen(SpriteRenderer spriteRenderer) {
        spriteRenderer.gameObject.transform.localScale
            = Vector3.one;

        float width = spriteRenderer.sprite.bounds.size.x;
        float height = spriteRenderer.sprite.bounds.size.y;

        double worldScreenHeight = Camera.main.orthographicSize * 2.0;
        double worldScreenWidth 
            = worldScreenHeight / Screen.height * Screen.width;

        float newX = (float) worldScreenWidth / width;
        float newY = (float)worldScreenHeight / height;

        spriteRenderer.gameObject.transform.localScale 
            = new Vector2(newX, newY);
    }

    private void resizRendererToScreen(Renderer renderer) {
        renderer.gameObject.transform.localScale
            = Vector3.one;

        float width = renderer.bounds.size.x;
        float height = renderer.bounds.size.y;

        double worldScreenHeight = Camera.main.orthographicSize * 2.0;
        double worldScreenWidth
            = worldScreenHeight / Screen.height * Screen.width;

        float newX = (float)worldScreenWidth / width;
        float newY = (float)worldScreenHeight / height;

        renderer.gameObject.transform.localScale
            = new Vector2(newX, newY);
    }
}
