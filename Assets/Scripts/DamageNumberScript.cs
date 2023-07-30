using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageNumberScript : MonoBehaviour
{
    public int damage;
    public Color color;
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<TMPro.TextMeshPro>().color = color;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        GetComponent<TMPro.TextMeshPro>().text = damage.ToString();
        gameObject.transform.position = transform.position + new Vector3(0, .005f, 0);
        Color currentColor = GetComponent<TMPro.TextMeshPro>().color;
        if (currentColor.a <= 0)
        {
            Destroy(gameObject);
        }
        else
        {
            Color newColor = new Color(currentColor.r, currentColor.g, currentColor.b, currentColor.a - .005f);
            GetComponent<TMPro.TextMeshPro>().color = newColor;
        }
        
    }



}
