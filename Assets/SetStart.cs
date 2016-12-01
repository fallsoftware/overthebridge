using UnityEngine;
using System.Collections;

public class SetStart : MonoBehaviour {
    public Transform positionStartPortal;
    private PortalControllerScript _portal;
	// Use this for initialization
	void Start () {
        this._portal = GameObject.FindGameObjectWithTag("Portal").GetComponent<PortalControllerScript>();
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Player" && _portal.PossessionState==0)
        {
            _portal.transform.position = positionStartPortal.position;
            _portal.changeToSetState();
            _portal.PossessionState = 1;
        }
    }
}
