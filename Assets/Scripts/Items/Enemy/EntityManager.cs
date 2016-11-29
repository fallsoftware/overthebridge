using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class EntityManager : MonoBehaviour {
    public List<EnemyScript> enemies; 
	// Use this for initialization
	void Start () {
	
	}
	public void addEnemy(EnemyScript enemy) {
        enemies.Add(enemy);
    }

    public void removeEnemy(EnemyScript enemy)
    {
        enemies.Remove(enemy);
        Destroy(enemy.gameObject);
    }
    public void DespawnAll()
    {
        while (enemies.Count != 0)
        {
            EnemyScript enemy = enemies[0];
            enemies.Remove(enemy);
            Destroy(enemy.gameObject);
        }
    }
    // Update is called once per frame
    void Update () {
	
	}
}
