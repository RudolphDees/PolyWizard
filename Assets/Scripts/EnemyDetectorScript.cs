using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDetectorScript : MonoBehaviour
{
    public Dictionary<int, GameObject> enemyDictionary = new Dictionary<int, GameObject>();
    public GameObject closestEnemy;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float shortestDistance = -1;
        GameObject clostestObject = null;
        GameObject tempObject;
        foreach (var enemyPair in enemyDictionary)
        {   
            tempObject = enemyPair.Value;
            if (clostestObject == null)
            {
                shortestDistance = Vector3.Distance(tempObject.transform.position, transform.position);
                clostestObject = tempObject;
            }
            else if (Vector3.Distance(tempObject.transform.position, transform.position) < shortestDistance && shortestDistance != -1)
            {
                shortestDistance = Vector3.Distance(tempObject.transform.position, transform.position);
                clostestObject = tempObject;
            }
        }
        closestEnemy = clostestObject;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {   
            int id = collision.gameObject.GetInstanceID();
            if (!enemyDictionary.ContainsKey(id))
            {
                enemyDictionary.Add(id, collision.gameObject);
            }
        }
    } 
    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {   
            int id = collision.gameObject.GetInstanceID();
            if (enemyDictionary.ContainsKey(id))
            {
                enemyDictionary.Remove(id);
            }
        }
    } 
}
