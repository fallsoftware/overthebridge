using UnityEngine;
using System.Collections;

public class ActivatePortal : MonoBehaviour {

    private PortalControllerScript _portal;
    // Use this for initialization
    void Start()
    {
        this._portal = GameObject.FindGameObjectWithTag("Portal").GetComponent<PortalControllerScript>();

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Player" && this._portal.PossessionState == 1)
        {
            _portal.PossessionState = 2;
            _portal.changeToNotSetState();
        }
    }
}
