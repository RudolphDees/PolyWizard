using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static ConstantsAndObjects.Constants;




public class EarthStompScript : MonoBehaviour
{
    public int damage;
    private float slamSpeed = .2f;
    public int stompCount;
    public float radius;
    private int currentStomps;
   
    // Start is called before the first frame update
    void Start()
    {
        Color earthColor = getEarthColor();
        GetComponent<SpriteRenderer>().color = earthColor;
    }


    // Update is called once per frame
    void FixedUpdate()
    {
        print(transform.localScale.x);
        if (transform.localScale.x > radius)
        {
            currentStomps += 1;
            transform.localScale -= new Vector3(radius, radius, 0);
        }
        transform.localScale += new Vector3(slamSpeed, slamSpeed, 0);
        if (currentStomps >= stompCount)
        {
            Destroy(gameObject);
        }
    }
}




