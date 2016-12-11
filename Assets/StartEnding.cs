using UnityEngine;
using System.Collections;

public class StartEnding : MonoBehaviour {
    private Collider2D portalCollider;
    public LevelManager levelManager;
    public Fader fader;
    public EndTrigger endTrigger;
    private bool activated = false;
    // Use this for initialization
    void Start () {
        fader = GameObject.FindGameObjectWithTag("Fader").GetComponent<Fader>();
    }

    // Update is called once per frame
    void Update () {
        if (!activated)
        {
            if (portalCollider == null)
            {
                portalCollider = GameObject.FindWithTag("Portal").GetComponent<CircleCollider2D>();
            }
            if (portalCollider.OverlapPoint(new Vector2(this.transform.position.x, this.transform.position.y)))
            {
                activated = true;
                InvokeRepeating("Ending", 0, 1f);
                ((Rigidbody2D)levelManager.player.GetComponent<Rigidbody2D>()).constraints = RigidbodyConstraints2D.FreezeAll;
                ((PortalControllerScript)GameObject.FindWithTag("Portal").GetComponent<PortalControllerScript>()).PossessionState = 0;
                fader.StartChange();
            }
        }
    }

    void Ending()
    {
        
        fader.StopChange();
        fader.StartChange();
        if (fader.frequency < 2)
        {
            fader.pulsesLength -= 1;
            if (fader.pulsesLength == 0)
            {
                ((PortalControllerScript)GameObject.FindWithTag("Portal").GetComponent<PortalControllerScript>()).changeToNotSetState();
                fader.pulses = 1;
                fader.frequency = 10;
                fader.pulsesLength = 1000;

                this.CancelInvoke();
                fader.StopChange();
                fader.setActiveWorld(true);
                fader._levelFading.BeginFade(1);
                Invoke("EndFade", 1.0f);
                
                
            }
        }
        else
        {
            fader.frequency -= 1f;
        }

        
    }
    private void EndFade ()
    {
        
        fader._levelFading.BeginFade(-1);
        Invoke("End", 5.0f);
    }

    private void End()
    {
        endTrigger.StartCoroutine(endTrigger.makeTheMagicHappen());
    }
}
