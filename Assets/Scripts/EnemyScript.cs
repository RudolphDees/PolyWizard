using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScript : MonoBehaviour
{
    public int health;
    public float movementSpeed;
    public GameObject damageNumber;
    public int maxHealth;
    public HealthBar healthBar;
    public GameObject player;
    public int damage;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (player != null)
        {
            var step =  movementSpeed * Time.deltaTime; // calculate distance to move
            transform.position = Vector3.MoveTowards(transform.position, player.transform.position, step);
        }
    }

    public void takeDamage(int damage, Color damageColor)
    {
        health -= damage;
        GameObject newDamageNumber = Instantiate(damageNumber);
        newDamageNumber.GetComponent<DamageNumberScript>().damage = damage;
        newDamageNumber.GetComponent<DamageNumberScript>().color = damageColor;
        newDamageNumber.transform.position = gameObject.transform.position + new Vector3(0, .6f, 0);
        if (health <= 0)
        {
            Destroy(gameObject);
        }
        
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Player")
        {
            collision.gameObject.GetComponent<PlayerScript>().takeDamage(damage, new Color(1,1,1,1));
        }
    }
}
