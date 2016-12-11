using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class SoundManager : MonoBehaviour {
    public Dictionary<string, AudioSource> FxSources;
    public float MusicFadeDuration = 2f;
    public float FxFadeDuration = 1f;
    public float LowPitchRange = .95f;
    public float HighPitchRange = 1.05f;
    public string CurrentScene = "Root";

    [HideInInspector]
    public GameObject SoundObject;

    public static SoundManager Instance = null;  

    [SerializeField]
    public float AmbianceVolume {
        get { return this.ambianceVolume; }
        set {
            this.ambianceVolume = value;
            this.UpdateAmbianceVolume();
        }
    }

    [Range(0f, 1f)]
    public float ambianceVolume = 1f;

    [SerializeField]
    public float FxVolume {
        get { return this.fxVolume; }
        set {
            this.fxVolume = value;
            this.UpdateFxVolume();
        }
    }

    [Range(0f, 1f)]
    public float fxVolume = 1f;

    void Start() {
    }

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
            this.RemoveFxSound(this.FxSources[fxSource.name]);
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
        StartCoroutine(this.RemoveFxSound(fxSource));
    }

    public void PlayRandomizeFx(List<AudioSource> fxSources) {
        int randomIndex = fxSources.Count-1;
        float randomPitch = Random.Range(this.LowPitchRange,
            this.HighPitchRange);
        AudioSource fxSource = fxSources[randomIndex];
        fxSource.pitch = randomPitch;
        fxSource.Play();
        StartCoroutine(this.RemoveFxSound(fxSource));
    }

    public void PlayFx(AudioSource fxSource) {
        fxSource.volume *= this.FxVolume;
        fxSource.Play();
        StartCoroutine(this.RemoveFxSound(fxSource));
    }

    public void SetMusicVolume(float musicVolume) {
        musicVolume = Mathf.Clamp01(musicVolume);

        this.AmbianceVolume = musicVolume;
    }

    public void SetFxVolume(float fxVolume) {
        fxVolume = Mathf.Clamp01(fxVolume);

        this.FxVolume = fxVolume;
    }

    public IEnumerator FadeIn(AudioSource newSource, 
        AudioSource oldSource = null, float duration = 2f, 
        bool ambiance = true) {
        float startTime = Time.time;
        float endTime = startTime + duration;
        float volume = ambiance == true ? this.AmbianceVolume : this.FxVolume;


        while (Time.time < endTime) {
            float i = (Time.time - startTime) / duration;

            if (oldSource != null) {
                oldSource.volume = (1 - i) * volume;
            }

            newSource.volume = i * this.AmbianceVolume;

            yield return null;
        }
    }

    public IEnumerator FadeOut(AudioSource audioSource, float duration = 2f, 
        bool ambiance = true) {
        float startTime = Time.time;
        float endTime = startTime + duration;
        float volume = ambiance == true ? this.AmbianceVolume : this.FxVolume;

        while (Time.time < endTime) {
            float i = (Time.time - startTime) / duration;
            audioSource.volume = (1 - i) * volume;

            yield return null;
        }
    }

    public IEnumerator DeleteOldAmbiances(GameObject newAmbianceObject) {
        yield return StartCoroutine(this.DeleteAmbiances());

        if (newAmbianceObject != null) {
            this.SoundObject = newAmbianceObject;
            this.CurrentScene = newAmbianceObject.scene.name;
            DontDestroyOnLoad(this.SoundObject);
        }
    }

    public IEnumerator DeleteAmbiances() {
        if (this.SoundObject != null) {
            List<AudioSource> oldAmbiances
                = this.SoundObject.GetComponent<Ambiance>().AmbianceSources;

            int oldSize = oldAmbiances.Count;

            for (int i = 0; i < oldSize; i++) {
                yield return StartCoroutine(this.FadeOut(oldAmbiances[i]));
            }

            Destroy(this.SoundObject);

            while (this.SoundObject != null) {
                yield return null;
            }
        }
    }

    public void SwitchAmbiance(GameObject newAmbianceObject) {
        List<AudioSource> newAmbiances 
            = newAmbianceObject.GetComponent<Ambiance>().AmbianceSources;
        int newSize = newAmbiances.Count;

        for (int i = 0; i < newSize; i++) {
            newAmbiances[i].Play();
            StartCoroutine(this.FadeIn(newAmbiances[i]));
        }
        
        StartCoroutine(this.DeleteOldAmbiances(newAmbianceObject));
    }

    public void UpdateFxVolume() {
        foreach (KeyValuePair<string, AudioSource> fxSource
            in this.FxSources) {
            fxSource.Value.volume = this.FxVolume;
        }
    }

    public void UpdateAmbianceVolume() {
        Ambiance ambiance = this.SoundObject.GetComponent<Ambiance>();
        List<AudioSource> ambianceSources = ambiance.AmbianceSources;
        int size = ambianceSources.Count;

        for (int i = 0; i < size; i++) {
            ambianceSources[i].volume 
                = ambianceSources[i].volume * this.ambianceVolume;
        }
    }

    IEnumerator RemoveFxSound(AudioSource audioSource) {
        if (this.FxSources.ContainsValue(audioSource)) {
            yield return new WaitForSeconds(audioSource.clip.length);

            var keysToRemove 
                = this.FxSources.Where(kvp => kvp.Value == audioSource)
                .Select(kvp => kvp.Key)
                .ToArray();

            foreach (var key in keysToRemove) {
                this.FxSources.Remove(key);
            }
        }
    }
}