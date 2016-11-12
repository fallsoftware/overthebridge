using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SoundManager : MonoBehaviour {
    public List<AudioSource> FxSources;
    public AudioSource FxSource;
    public AudioSource MusicSource;
    public static SoundManager Instance = null;

    public float LowPitchRange = .95f;
    public float HighPitchRange = 1.05f;

	void Start () {
	
	}
	
	void Update () {
	
	}

    void Awake() {
        if (SoundManager.Instance == null) {
            SoundManager.Instance = this;
        } else if (SoundManager.Instance != this) {
            Destroy(this.gameObject);
        }

        SoundManager.DontDestroyOnLoad(this.gameObject);
    }

    public void AddSource(AudioSource audioSource, bool play = true) {
        this.FxSources.Add(audioSource);
        if (play) audioSource.Play();
    }

    public void PlaySingle(AudioClip audioClip) {
        this.FxSource.clip = audioClip;
        this.FxSource.Play();
    }

    public void DestroySingle(AudioSource audioSource) {
        if (this.FxSources.Remove(audioSource)) {
            AudioSource.Destroy(audioSource);   
        }
    }

    public void RandomizeFx(params AudioClip[] audioClips) {
        int randomIndex = Random.Range(0, audioClips.Length);
        float randomPitch = Random.Range(this.LowPitchRange, 
            this.HighPitchRange);
        this.FxSource.loop = false;
        this.FxSource.pitch = randomPitch;
        this.FxSource.clip = audioClips[randomIndex];
        this.FxSource.Play();
    }

    public void PlayFx(
        AudioClip audioClip, float volume = 1f, bool loop = false) {
        this.FxSource.clip = audioClip;
        this.FxSource.loop = loop;
        this.FxSource.volume = volume;
        this.FxSource.minDistance = 1f;
        this.FxSource.Play();
    }
}
