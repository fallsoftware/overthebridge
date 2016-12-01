using UnityEngine;
using System.Collections;

public class StartChange : MonoBehaviour {
    private Fader _fader;
    private static bool _Enable=false;
	// Use this for initialization
	void Start () {
        _fader = GameObject.FindGameObjectWithTag("Fader").GetComponent<Fader>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Player" && _fader.enabled ==false)
        {
            _fader.StartChange();
        }
    }
}
