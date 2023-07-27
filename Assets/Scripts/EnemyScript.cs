using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScript : MonoBehaviour
{
    public int health;
    public int movementSpeed;
    public GameObject damageNumber;
    public int maxHealth;
    public HealthBar healthBar;
    // Start is called before the first frame update
    void Start()
    {
        healthBar.SetMaxHealth(maxHealth);

        healthBar.SetHealth(health);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void takeDamage(int damage, Color damageColor)
    {
        health -= damage;
        GameObject newDamageNumber = Instantiate(damageNumber);
        newDamageNumber.GetComponent<DamageNumberScript>().damage = damage;
        newDamageNumber.GetComponent<DamageNumberScript>().color = damageColor;
        newDamageNumber.transform.position = gameObject.transform.position + new Vector3(0, 1.5f, 0);
        if (health <= 0)
        {
            Destroy(gameObject);
        }
        healthBar.SetHealth(health);
        
    }
}
