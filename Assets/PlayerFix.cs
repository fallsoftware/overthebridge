using UnityEngine;
using System.Collections;

public class PlayerFix : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnCollisionEnter2D(Collision2D c) {

        if((c.collider is EdgeCollider2D)&& c.collider.gameObject.name=="Portal Collider")
        {
            if (((EdgeCollider2D) c.collider).points[0].ToString() == "(-0.5, 0.0)" && ((EdgeCollider2D) c.collider).points[1].ToString() == "(0.5, 0.0)") {
                Destroy(c.collider);
            }
        }

    }
}
