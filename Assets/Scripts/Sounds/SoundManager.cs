using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SoundManager : MonoBehaviour {
    public Dictionary<string, AudioSource> FxSources;
    public AudioSource MusicSource;
    public static SoundManager Instance = null;
    [Range(0f, 1f)] public float MusicVolume = 1f;
    [Range(0f, 1f)] public float FxVolume = 1f;

    public float LowPitchRange = .95f;
    public float HighPitchRange = 1.05f;

    void Start() {}

    void Update() {}

    void Awake() {
        if (SoundManager.Instance == null) {
            SoundManager.Instance = this;
        } else if (SoundManager.Instance != this) {
            Destroy(this.gameObject);
        }

        SoundManager.DontDestroyOnLoad(this.gameObject);
        this.FxSources = new Dictionary<string, AudioSource>();
    }

    public AudioSource AddFxSource(AudioSource fxSource) {
        if (this.FxSources.ContainsKey(fxSource.name)) {
            return this.FxSources[fxSource.name];
        }

        this.FxSources.Add(fxSource.name, fxSource);

        return fxSource;
        ;
    }

    public void PlaySingle(AudioClip audioClip) {
        AudioSource audioSource = new AudioSource();

        audioSource.clip = audioClip;
        audioSource.Play();
    }

    public void PlayRandomizeFx(params string[] fxSources) {
        int randomIndex = Random.Range(0, fxSources.Length);
        float randomPitch = Random.Range(this.LowPitchRange,
            this.HighPitchRange);
        AudioSource fxSource = this.FxSources[fxSources[randomIndex]];
        fxSource.pitch = randomPitch;
        fxSource.Play();
    }

    public void PlayFx(string fxSourceKey) {
        AudioSource fxSource = this.FxSources[fxSourceKey];
        this.PlayFx(fxSource);
    }

    public void PlayFx(AudioSource fxSource) {
        fxSource.volume *= this.FxVolume;
        fxSource.Play();
    }

    public void PlayMusic(AudioSource musicSource) {
        musicSource.Play();
        StartCoroutine(this.fadeOut(musicSource));
    }

    public void SetMusicVolume(float musicVolume) {
        musicVolume = Mathf.Clamp01(musicVolume);

        this.MusicVolume = musicVolume;
    }

    public void SetFxVolume(float fxVolume) {
        fxVolume = Mathf.Clamp01(fxVolume);

        this.FxVolume = fxVolume;
    }

    public IEnumerator fadeOut( AudioSource newMusic) {
        float duration = 2f;
        float startTime = Time.time;
        float endTime = startTime + duration;


        while (Time.time < endTime) {
            float i = (Time.time - startTime) / duration;

            if (this.MusicSource != null) {
                this.MusicSource.volume = (1 - i) * this.MusicVolume;
            }

            newMusic.volume = i * this.MusicVolume;

            yield return null;
        }

        this.MusicSource = newMusic;
    }
}