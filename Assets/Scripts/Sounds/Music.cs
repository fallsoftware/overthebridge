using UnityEngine;
using System.Collections;

public class Music : MonoBehaviour {
    public AudioClip MusicClip;
    public float Volume = 1f;

    [HideInInspector]
    public AudioSource MusicSource;

	void Start () {
	    this.MusicSource = Sound.BuildMusicSource(
            this.gameObject, this.MusicClip);
    }

	void Update () {
	
	}

    void OnTriggerEnter2D(Collider2D collider2D) {
        if (collider2D.gameObject.tag != "Player") return;

        if (SoundManager.Instance.MusicObject != null
            && SoundManager.Instance.MusicObject.GetComponent<Music>().MusicSource.name
            == this.MusicSource.name) {
                return;
        }

        SoundManager.Instance.PlayMusic(this.gameObject);
    }
}
