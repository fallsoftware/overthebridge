using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Ambiance : MonoBehaviour {
    public AudioClip[] AmbianceClips;

    [HideInInspector]
    public List<AudioSource> AmbianceSources;

    void Start() {
        this.AmbianceSources = new List<AudioSource>();
        int size = AmbianceClips.Length;

        for (int i = 0; i < size; i++) {
            AudioSource newFx = Sound.BuildFxSource(
                this.gameObject, this.AmbianceClips[i], 
                this.AmbianceClips[i].name, true, 0f);
            this.AmbianceSources.Add(newFx);
        }
    }

    void Update() {

    }

    void OnTriggerEnter2D(Collider2D collider2D) {
        if (collider2D.gameObject.tag != "Player") return;

        int size = this.AmbianceSources.Count;

        for (int i = 0; i < size; i++) {
            if (!SoundManager.Instance.FxSources.ContainsValue(
                this.AmbianceSources[i])) {
                AudioSource newFx = SoundManager.Instance.AddFxSource(
                    this.AmbianceSources[i]);
                SoundManager.Instance.PlayFx(newFx);
            }
        }
    }
}
