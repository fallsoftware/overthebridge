using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

public class Portal : MonoBehaviour {
    public GameObject Player;
    public bool InDark;
    public float ShineSpeed = 0.8f;
    public float Tolerance = 0.1f;
    public AudioClip OutSound;
    public AudioClip InSound;
    public float SoundFactor = 1f;

    private AudioSource _audioSource;
    private Renderer _renderer;
    private float _defaultShineLocation;

    void Start() {
        this._renderer = this.GetComponent<Renderer>();
        this._defaultShineLocation 
            = this._renderer.material.GetFloat("_ShineLocation");
        this._renderer.material.SetFloat("_ShineWidth", this.computeShineWidth(
            this._defaultShineLocation));

        if (this.InDark) {
            this.buildAudioSource(this.OutSound);
        } else {
            this.buildAudioSource(this.InSound);
        }
    }

    void Update() {
        float oldShineLocation 
            = this._renderer.material.GetFloat("_ShineLocation");

        // you have to reset the animation to its default state if the player
        // is in the dark world
        if (!this.InDark 
            || Math.Abs(oldShineLocation - this._defaultShineLocation) 
            > this.Tolerance) {
            this.setShininess(oldShineLocation);
        }

        this.CheckIfPlayerStillInside();
        this.updateAudioVolume();
    }

    void OnTriggerEnter2D(Collider2D collider2D) {
        if (!this.InDark || !this.checkIfPlayer(collider2D)) return;
        this.InDark = false;
        Portal.SetLayerRecursively(
            this.Player, LayerMask.NameToLayer("Light"));
        Portal.SetSortingLayerRecursively(
            this.Player, "MiddlegroundLight");

        this.updateAudioClip(this.InSound);
    }

    void OnTriggerExit2D(Collider2D collider2D) {
        if (this.InDark || !this.checkIfPlayer(collider2D)) return;
        
        this.SetPlayerToDark();
    }

    public void SetPlayerToDark() {
        this.InDark = true;
        Portal.SetLayerRecursively(this.Player, LayerMask.NameToLayer("Dark"));
        Portal.SetSortingLayerRecursively(
            this.Player, "MiddlegroundDark");

        this.updateAudioClip(this.OutSound);
    }

    public void CheckIfPlayerStillInside() {
        CircleCollider2D circle = this.GetComponent<CircleCollider2D>();

        if (!circle.OverlapPoint(this.Player.transform.position)) {
            this.SetPlayerToDark();
        }
    }

    private bool checkIfPlayer(Collider2D collider2D) {
        return (collider2D.tag == "Player" && collider2D is CircleCollider2D);
    }

    public static void SetLayerRecursively(
        GameObject gameObject, int newLayer) {
        gameObject.layer = newLayer;

        foreach (Transform child 
            in gameObject.GetComponentInChildren<Transform>()) {
            SetLayerRecursively(child.gameObject, newLayer);
        }
    }

    public static void SetSortingLayerRecursively(
        GameObject gameObject, string newSortingLayer) {
        SpriteRenderer spriteRenderer 
            = gameObject.GetComponent<SpriteRenderer>();

        if (spriteRenderer != null) {
            spriteRenderer.sortingLayerName = newSortingLayer;
        }

        foreach (Transform child
            in gameObject.GetComponentInChildren<Transform>()) {
            SetSortingLayerRecursively(child.gameObject, newSortingLayer);
        }
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
        float shineWidth = 2*shineLocation;
        const float middle = 0.5f;

        if (shineWidth > 2* middle) {
            shineWidth = this.computeShineWidth(2*middle - shineLocation);
        }

        return shineWidth;
    }

    private void updateAudioVolume() {
        if (!this.InDark) return;

        Vector2 distance 
            = this.Player.transform.position - this.transform.position;
        this._audioSource.volume = 1/distance.sqrMagnitude;
        this._audioSource.volume *= this.SoundFactor;
    }

    private void updateAudioClip(AudioClip audioClip) {
        this._audioSource.clip = audioClip;
        this._audioSource.Play();
    }

    private void buildAudioSource(AudioClip audioClip) {
        this._audioSource 
            = SoundManager.Instance.gameObject.AddComponent<AudioSource>();
        this._audioSource.loop = true;
        this._audioSource.clip = audioClip;
        this._audioSource.Play();
    }
}