using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static ConstantsAndObjects.Constants;


public class BulletScript : MonoBehaviour
{
    public int damage;

    // Start is called before the first frame update
    void Start()
    {
        GetComponent<SpriteRenderer>().color = getFireColor();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag != "Player")
        {
            if (collision.gameObject.tag == "Enemy")
            {
                collision.gameObject.GetComponent<EnemyScript>().takeDamage(damage, getFireColor());
            }
            else if (collision.gameObject.tag == "Barrier")
            {
                Destroy(gameObject);
            }
        }
    }
    
}
