using UnityEngine;
using System.Collections;

public class PortalFix : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	    Collider2D[] coll = this.gameObject.GetComponents<Collider2D>();

	    foreach (Collider2D co in coll) {
	        if (!(co is CircleCollider2D)) {
	            Destroy(co);
	        }
	    }
	}
}
