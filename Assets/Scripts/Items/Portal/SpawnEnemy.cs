using UnityEngine;
using System.Collections;

public class SpawnEnemy : MonoBehaviour {
    public float speed=0.5f;
    public float updatetime=0;
    public float lifetime=15;
    public EnemyScript Enemy;
    public GameManager gameManager;
    public bool active=false;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
    public void SpawnNewEnemy()
    {
        if (active)
        {
            EnemyScript enemy = Instantiate(Enemy);
            enemy.transform.position = this.transform.position;
            enemy.speed = this.speed;
            enemy.updateFrequency = this.updatetime;
            enemy.lifetime = this.lifetime;
            enemy.gameManager = this.gameManager;
        }
    }
}
