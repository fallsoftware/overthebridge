using UnityEngine;
using System.Collections;

public class ActivatePortal : MonoBehaviour {

    private PortalControllerScript _portal;
    private LevelFading _levelFading;

    // Use this for initialization
    void Start()
    {
        this._portal = GameObject.FindGameObjectWithTag("Portal").GetComponent<PortalControllerScript>();
        this._levelFading = GameObject.FindObjectOfType<LevelFading>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Player" && this._portal.PossessionState == 1)
        {
            _portal.PossessionState = 2;
            _portal.changeToNotSetState();
            _levelFading.BeginFade(1);
            Invoke("EndActivation", 1.0f);
        }
    }
    private void EndActivation()
    {
        _levelFading.BeginFade(-1);
    }
}
