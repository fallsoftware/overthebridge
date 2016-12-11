using UnityEngine;
using System.Collections;

public class BlackHoleSound : MonoBehaviour {
    public AudioClip BlackHoleClip;

    private AudioSource _audioSource;

    void Start() {
        this.buildAudioSource();
    }

    void Update() {}

    private void buildAudioSource() {
        this._audioSource
            = Sound.BuildFxSource(this.gameObject, this.BlackHoleClip, true, 1f);
        this._audioSource = SoundManager.Instance.AddFxSource(
            this._audioSource, "BlackHole" + this.GetInstanceID());
        this._audioSource.minDistance = 2f;
        this._audioSource.maxDistance = 3f;
        this._audioSource.volume = 1f;
        this._audioSource.dopplerLevel = 0f;
        SoundManager.Instance.PlayFx(this._audioSource);
    }
}
