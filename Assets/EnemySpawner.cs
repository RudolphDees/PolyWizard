using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemy;
    public int spawnRate;
    public GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SpawnCoRoutine());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator SpawnCoRoutine()
    {
        GameObject newEnemy = Instantiate(enemy);
        var newEnemyScript = newEnemy.GetComponent<EnemyScript>();
        newEnemyScript.player = player;
        newEnemy.transform.position = transform.position;
        yield return new WaitForSeconds(spawnRate);
        StartCoroutine(SpawnCoRoutine());
    }
}
