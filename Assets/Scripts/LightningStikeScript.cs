using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static ConstantsAndObjects.Constants;

public class LightningStikeScript : MonoBehaviour
{
    public int damage;
    public GameObject target;
    private float strikeSpeed = .05f;
    private bool gettingBrighter = true;
    
    // Start is called before the first frame update
    void Start()
    {
        transform.position = target.transform.position;
        Color lightningColor = getLightningColor();
        Color startingLightningColor = new Color(lightningColor.r, lightningColor.g, lightningColor.b, 0);
        GetComponent<SpriteRenderer>().color = startingLightningColor;

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Color currentColor = GetComponent<SpriteRenderer>().color;
        float currentTransperancy = currentColor.a;
        float newTransperancy = 0;
        if (currentTransperancy + strikeSpeed > 1)
        {
            gettingBrighter = false;
            if (target != null)
            {
                target.gameObject.GetComponent<EnemyScript>().takeDamage(damage, getLightningColor());
            }
        }
        if (gettingBrighter) 
        {
            newTransperancy = currentTransperancy + strikeSpeed;
        }
        else 
        {
            newTransperancy = currentTransperancy - strikeSpeed;
        }
        if (newTransperancy <= 0)
        {
            Destroy(gameObject);
        }
        else 
        {
            GetComponent<SpriteRenderer>().color = new Color(currentColor.r, currentColor.g, currentColor.b, newTransperancy);
        }
    }
}
