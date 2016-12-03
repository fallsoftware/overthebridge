using UnityEngine;
using System.Collections;

public class Music : MonoBehaviour {
    public AudioClip MusicClip;
    public float Volume = 1f;

	void Start () {
	    AudioSource music = Sound.BuildMusicSource(
            this.gameObject, this.MusicClip);


        SoundManager.Instance.PlayMusic(music);
    }

	void Update () {
	
	}
}
