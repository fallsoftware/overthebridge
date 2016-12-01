using UnityEngine;
using System.Collections;

public class DesactivateFader : MonoBehaviour {
    private Fader _fader;
    // Use this for initialization
    void Start()
    {
        _fader = GameObject.FindGameObjectWithTag("Fader").GetComponent<Fader>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Player")
        {
            _fader.StopChange();
        }
    }
}
