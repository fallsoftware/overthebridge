using UnityEngine;
using System.Collections;

public class GroundChecker : MonoBehaviour {

    public PlayerControllerScript player;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.gameObject.CompareTag("Ground"))
        {
            player.Grounded = true;
        }
    }
}
