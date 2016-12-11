using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Fader : MonoBehaviour {
    public GameObject masks;
    public AudioClip[] Sounds;
    private LevelFading _levelFading;
    public bool enabled = false;
    public float frequency=10.0f;
    public float pulses = 10;
    public float pulsesLength = 10;
    private float currentpulse;
    private float currentpulses;
    private bool onPulse=false;
    private List<AudioSource> SourceSounds;
	// Use this for initialization
	void Start () {
        _levelFading = gameObject.GetComponent<LevelFading>();
        this.initializeSounds();
	}
    private void Update()
    {
        if (onPulse)
        {
            currentpulse--;
            if (currentpulse < 1) {
                masks.SetActive(!masks.activeSelf);
                currentpulses--;
                currentpulse = pulsesLength;
            }
            if (currentpulses < 1)
            {
                onPulse = false;
                currentpulses = pulsesLength;
            }
        }
    }

    public void StartChange(){
        enabled = true;
        currentpulses = pulsesLength;
        currentpulse = pulsesLength;

        InvokeRepeating("Pulse", 0, frequency);
        }
    public void Pulse() {
        SoundManager.Instance.PlayRandomizeFx(this.SourceSounds);
        _levelFading.BeginFade(1);
        _levelFading.BeginFade(-1);
        onPulse = true;
    }
    public void StopChange()
    {
        CancelInvoke();
    }

    private void initializeSounds() {
        int size = this.Sounds.Length;
        this.SourceSounds = new List<AudioSource>();

        for (int i = 0; i < size; i++) {
            AudioSource newSound = Sound.BuildFxSource(this.gameObject, 
                this.Sounds[i], false, 0f);
            this.SourceSounds.Add(newSound);
        }
    }
}
