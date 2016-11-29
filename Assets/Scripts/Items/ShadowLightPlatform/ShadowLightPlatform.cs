using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ShadowLightPlatform : MonoBehaviour {

    public bool gravity = false;
    public float AerialTime=0.1f;
    public float acceleration=0.01f;
    public float speed = 0;
    public float maxspeed = 2;
    private float rayspeed = 0;
    private Vector2 InitialPosition;
    private float startTime=0;
    private bool falling = false;
    private List<GameObject> collisionDown= new List<GameObject>();
    private List<GameObject> collisionUp = new List<GameObject>();
    // Use this for initialization
    void Start () {
        InitialPosition = this.transform.position;
	}

    void OnTriggerEnter2D(Collider2D collider) {
        if (collider.CompareTag("Ground")) {
            if (falling)
            {
                collisionDown.Add(collider.gameObject);
            }
            else
            {
                collisionUp.Add(collider.gameObject);
            }
            
        }
    }
    void OnTriggerExit2D(Collider2D collider) {
        if (collider.CompareTag("Ground")) {
            if (collisionUp.Exists(item => item==collider.gameObject))
            {
                collisionUp.Remove(collider.gameObject);
            }
            else
            {
                collisionDown.Remove(collider.gameObject);
            }
        }
    }

    public void ApllyForce(Vector2 Velocity)
    {
        falling = false;
        if (gravity)
        {
            startTime = Time.time;
        }
        if (collisionUp.Count == 0) {
            if (rayspeed < maxspeed) {
                rayspeed += acceleration;
            }
            gameObject.GetComponent<Rigidbody2D>().velocity = Velocity.normalized * rayspeed;
            
        }
        else
        {
            gameObject.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        }

    }
    // Update is called once per frame
    void Update() {
        if (gravity && collisionDown.Count == 0) { 
            float timeleft = Time.time - startTime;
            if(AerialTime - timeleft<=0)
            {
                rayspeed = 0;
                if (speed < maxspeed)
                {
                    speed += acceleration;
                }
                Vector2 CurrentPosition = this.transform.position;
                gameObject.GetComponent<Rigidbody2D>().MovePosition(CurrentPosition + Vector2.down * speed);
                falling = true;

            }
            else
            {
                speed = 0;
            }
        }
        else if(collisionDown.Count != 0)
        {
            gameObject.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        }
    }
}
