using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Ambiance : MonoBehaviour {
    public AudioClip[] AmbianceClips;
    public float Volume = 1f;

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
}
