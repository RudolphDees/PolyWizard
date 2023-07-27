using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightningDetectorScript : MonoBehaviour
{
    public Dictionary<int, GameObject> enemyDictionary = new Dictionary<int, GameObject>();
    public GameObject target;
    // Start is called before the first frame update
    void Start()
    {
        transform.position = target.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // void OnTriggerStay2D(Collider2D collision)
    // {
    //     if (newlyCreated) 
    //     {
    //         if (collision.gameObject.tag == "Enemy")
    //             {   
    //                 print("test");
    //                 int id = collision.gameObject.GetInstanceID();
    //                 if (!enemyDictionary.ContainsKey(id))
    //                 {
    //                     enemyDictionary.Add(id, collision.gameObject);
    //                 }
    //             }
    //         newlyCreated = false;

    //     }
    // } 

    void OnTriggerEnter2D(Collider2D collision)
    {
        print(collision.gameObject.name);
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
