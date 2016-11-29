using UnityEngine;
using System.Collections;

public class EnemyScript : MonoBehaviour {
    public float speed=10;
    
    public float lifetime=0.1f;
    public float updateFrequency=0;
    public EntityDeath DeathManager;
    public  GameManager gameManager;
    private Vector2 target;
    private float startLife;
    private Vector2 direction;
    private GameObject player;
    private bool death = false;
    // Use this for initialization
    void Start () {
        gameManager.EntityManager.addEnemy(this);
        startLife = Time.time;
        player = GameObject.FindWithTag("Player");
        target = player.transform.position;
        Vector2 CurrentPosition = this.transform.position;
        direction = (CurrentPosition-target).normalized;
        if (updateFrequency != 0)
        {
            InvokeRepeating("UpdateTarget", 0, updateFrequency);
        }
    }

    void UpdateTarget()
    {
        target = player.transform.position;
        Vector2 CurrentPosition = this.transform.position;
        direction = (target- CurrentPosition).normalized;
    }
    // Update is called once per frame
    void  Update()
    {
        float timeleft = Time.time - startLife;
        if (lifetime - timeleft <= -1)
        {
            gameManager.EntityManager.removeEnemy(this);
        }
        else if(lifetime - timeleft > 0)
        {
            Vector2 CurrentPosition = this.transform.position;
            gameObject.GetComponent<Rigidbody2D>().MovePosition(CurrentPosition + direction * speed);
        }
        else
        {
            StartCoroutine(Death());
        }
    }
    public IEnumerator Death()
    {
        if (!death)
        {
            death = true;
            startLife = Time.time - lifetime;
            yield return StartCoroutine(this.DeathManager.MakeEntityDie());
        }
        
    }

    public void OnTriggerEnter2D(Collider2D collider2D)
    {
        if (collider2D.tag != "Player") return;

        this.gameManager.LevelManager.OopsPlayerIsDead();
    }
}
