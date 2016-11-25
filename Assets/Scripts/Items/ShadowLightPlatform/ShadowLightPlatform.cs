using UnityEngine;
using System.Collections;

public class ShadowLightPlatform : MonoBehaviour {

    public bool gravity = false;
    public float AerialTime=0.1f;
    public float acceleration=0.01f;
    public float speed = 0;
    public float maxspeed = 2;
    private float rayspeed = 0;
    private Vector2 InitialPosition;
    private float startTime=0;
	// Use this for initialization
	void Start () {
        InitialPosition = this.transform.position;
	}
	public void ApllyForce(Vector2 Velocity)
    {
        if (rayspeed < maxspeed)
        {
            rayspeed += acceleration;
        }
        gameObject.GetComponent<Rigidbody2D>().velocity = Velocity.normalized*rayspeed;
        if (gravity)
        {
            startTime = Time.time;
        }
    }
    // Update is called once per frame
    void Update() {
        gameObject.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        if (gravity && startTime != 0) { 
            float timeleft = Time.time - startTime;
            if(AerialTime - timeleft<=0)
            {
                rayspeed = 0;
                if (speed < maxspeed)
                {
                    speed += acceleration;
                }
                Vector2 CurrentPosition = this.transform.position;
                if ((InitialPosition - CurrentPosition).magnitude < speed)
                {
                    gameObject.GetComponent<Rigidbody2D>().MovePosition(InitialPosition);
                }
                else
                {
                    gameObject.GetComponent<Rigidbody2D>().MovePosition(CurrentPosition + (InitialPosition - CurrentPosition).normalized * speed);
                }
                

            }
            else
            {
                speed = 0;
            }
        }
    }
}
