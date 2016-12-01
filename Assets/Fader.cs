using UnityEngine;
using System.Collections;

public class Fader : MonoBehaviour {
    public GameObject masks;
    private LevelFading _levelFading;
    public bool enabled = false;
    public float frequency=10.0f;
    public float pulses = 10;
    public float pulsesLength = 10;
    private float currentpulse;
    private float currentpulses;
    private bool onPulse=false;
	// Use this for initialization
	void Start () {
        _levelFading = gameObject.GetComponent<LevelFading>(); 
	}
    private void Update()
    {
        if (onPulse)
        {
            currentpulse--;
            if (currentpulse < 1)
            {
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
    public void Pulse()
    {
        _levelFading.BeginFade(1);
        _levelFading.BeginFade(-1);
        onPulse = true;
    }
    public void StopChange()
    {
        CancelInvoke();
    }
}
