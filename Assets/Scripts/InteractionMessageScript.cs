using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionMessageScript : MonoBehaviour
{
    public string interactionStyle;
    public string button;
    // Start is called before the first frame update
    public GameObject player;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        GetComponent<TMPro.TextMeshPro>().text = "Press " + button + " to " + interactionStyle;
        transform.position = player.transform.position + new Vector3(0,.5f,0);
    }

    public void DestroySelf()
    {
        Destroy(gameObject);
    }
}
