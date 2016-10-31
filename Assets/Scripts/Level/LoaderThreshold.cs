using UnityEngine;
using System.Collections;

public class LoaderThreshold : MonoBehaviour {
    [HideInInspector] public bool FromRight;

    private bool _inTrigger;

	void Start() {
	    this._inTrigger = false;
	}
	
	void Update() {
	}

    public void ProcessLevel() {
        this.SendMessageUpwards("HandleLevel");
    }

    public void OnTriggerEnter2D(Collider2D collider2D) {
        if (collider2D.tag != "Player") return;

        this._inTrigger = true;
    }

    public void OnTriggerExit2D(Collider2D collider2D) {
        if (collider2D.tag != "Player") return;

        if (!this._inTrigger) return;

        this.UpdateFromRight();
        this.ProcessLevel();
        this._inTrigger = false;
    }

    private void UpdateFromRight() {
        Collider2D collider2D = this.GetComponent<Collider2D>();
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        if (collider2D == null || player == null) return;

        if (player.transform.position.x > collider2D.bounds.max.x) {
            this.FromRight = false;
        } else {
            this.FromRight = true;
        }
    }
}
