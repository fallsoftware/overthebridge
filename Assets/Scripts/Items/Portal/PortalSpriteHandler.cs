using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PortalSpriteHandler : MonoBehaviour {
    public GameObject[] GameObjects;

    private List<SpriteRenderer> _spriteRenderers;
    private float[] _oldAlphas;

    void Start() {
        this.setSpriteRenderers();
        this.setOldAlphas();
    }
	
	void Update() {
	
	}

    private void setOldAlphas() {
        int size = this._spriteRenderers.Count;

        this._oldAlphas = new float[size];

        for (int i = 0; i < size; i++) {
            this._oldAlphas[i] = this._spriteRenderers[i].color.a;
        }
    }

    public void SetAlpha(float alpha) {
        int size = this._spriteRenderers.Count;
        Color newColor;

        for (int i = 0; i < size; i++) {
            newColor = this._spriteRenderers[i].color;
            this._spriteRenderers[i].color = new Color(
                newColor.r, newColor.g, newColor.b, alpha);
        }
    }

    public void RevertAlpha() {
        int size = this._spriteRenderers.Count;
        Color newColor;

        for (int i = 0; i < size; i++) {
            newColor = this._spriteRenderers[i].color;
            this._spriteRenderers[i].color = new Color(
                newColor.r, newColor.g, newColor.b, this._oldAlphas[i]);
        }
    }

    private void setSpriteRenderers() {
        this._spriteRenderers = new List<SpriteRenderer>();

        GameObject parent = this.gameObject;
        SpriteRenderer spriteRenderer = parent.GetComponent<SpriteRenderer>();

        if (spriteRenderer != null) {
            this._spriteRenderers.Add(spriteRenderer);
        }

        foreach (Transform child in parent.transform) {
            this.setSpriteRenderersRecursively(child);
        }
    }

    private void setSpriteRenderersRecursively(Transform parentTransform) {
        GameObject parent = parentTransform.gameObject;
        SpriteRenderer spriteRenderer = parent.GetComponent<SpriteRenderer>();

        if (spriteRenderer != null) {
            this._spriteRenderers.Add(spriteRenderer);
        }

        foreach (Transform child in parent.transform) {
            this.setSpriteRenderersRecursively(child);
        }
    }
}
