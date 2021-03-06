﻿using UnityEngine;
using System.Collections;

public class CrystalState : MonoBehaviour {
    public float ShineSpeed = 0.8f;
    public float ShinePeriod = 3f;
    public AudioClip CrystalSound;

    private AudioSource _audioSource;
    private Renderer _renderer;
    private float _defaultShineLocation;
    private bool _wait = false;

    void Start () {
        this._renderer = this.GetComponent<Renderer>();
        this._defaultShineLocation
            = this._renderer.material.GetFloat("_ShineLocation");
        this._renderer.material.SetFloat("_ShineWidth", this.computeShineWidth(
            this._defaultShineLocation));
        this.buildAudioSource();
    }

	void Update () {
	    if (this._wait) return;

        float oldShineLocation
            = this._renderer.material.GetFloat("_ShineLocation");
        this.setShininess(oldShineLocation);

	    if (this._renderer.material.GetFloat("_ShineWidth") <= 0.05f) {
	        StartCoroutine(this.HandleIt());
	    }
    }

    private IEnumerator HandleIt() {
        this._wait = true;

        yield return new WaitForSeconds(this.ShinePeriod);

        this._wait = false;
    }

    private void setShininess(float oldShineLocation) {
        float newShineLocation
            = oldShineLocation + (this.ShineSpeed * Time.deltaTime);
        newShineLocation %= 1;
        this._renderer.material.SetFloat("_ShineLocation", newShineLocation);
        this._renderer.material.SetFloat(
            "_ShineWidth", this.computeShineWidth(newShineLocation));
    }

    private float computeShineWidth(float shineLocation) {
        float shineWidth = 2 * shineLocation;
        const float middle = 0.5f;

        if (shineWidth > 2 * middle) {
            shineWidth = this.computeShineWidth(2 * middle - shineLocation);
        }

        return shineWidth;
    }

    private void buildAudioSource() {
        this._audioSource
            = Sound.BuildFxSource(this.gameObject, this.CrystalSound, true, 
            1f);
        this._audioSource = SoundManager.Instance.AddFxSource(
                    this._audioSource, "Crystal" + this.GetInstanceID());
        this._audioSource.minDistance = 2f;
        this._audioSource.maxDistance = 10f;
        this._audioSource.volume = 1f;
        this._audioSource.dopplerLevel = 0f;
        SoundManager.Instance.PlayFx(this._audioSource);
    }
}
