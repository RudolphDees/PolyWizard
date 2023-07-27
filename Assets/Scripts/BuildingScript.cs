using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingScript : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject roof;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        print("test");

        if (collision.gameObject.tag == "Player"){
            var roofSpriteRenderer = roof.GetComponent<SpriteRenderer>();
            Color roofColor = roofSpriteRenderer.color;
            Color newColor = new Color(roofColor.r, roofColor.g,roofColor.b, .1f);
            roofSpriteRenderer.color = newColor; 
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        print("test");
        if (collision.gameObject.tag == "Player"){
            var roofSpriteRenderer = roof.GetComponent<SpriteRenderer>();
            Color roofColor = roofSpriteRenderer.color;
            Color newColor = new Color(roofColor.r, roofColor.g,roofColor.b, 1);
            roofSpriteRenderer.color = newColor; 
        }
    }
}
