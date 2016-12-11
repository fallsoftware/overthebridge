using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Fader : MonoBehaviour {
    public GameObject masks;
    public AudioClip[] Sounds;
    public LevelFading _levelFading;
    public bool enabled = false;
    public float frequency=10.0f;
    public float pulses = 10;
    public float pulsesLength = 10;
    public bool sound;
    private float currentpulse;
    private float currentpulses;
    private bool onPulse=false;
    private List<AudioSource> SourceSounds;
	// Use this for initialization
	void Start () {
        _levelFading = gameObject.GetComponent<LevelFading>();
        this.initializeSounds();
	}
    public void setActiveWorld(bool state)
    {
        masks.SetActive(state);
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
                currentpulses = pulses;
            }
        }
    }

    public void StartChange(){
        enabled = true;
        currentpulses = pulses;
        currentpulse = pulsesLength;
        if (frequency < 1)
        {
            sound = false;
            InvokeRepeating("SoundOnly", 0, 0.9f);
        }
        else
        {
            sound = true;
        }
        InvokeRepeating("Pulse", 0, frequency);
        }
    public void Pulse() {
        if (sound) 
        SoundManager.Instance.PlayRandomizeFx(this.SourceSounds);
        _levelFading.BeginFade(1);
        _levelFading.BeginFade(-1);
        onPulse = true;
    }
    public void SoundOnly()
    {
        SoundManager.Instance.PlayRandomizeFx(this.SourceSounds);
    }
    public void StopChange()
    {
        onPulse = false;
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
