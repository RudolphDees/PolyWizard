using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectionCirlceScript : MonoBehaviour
{
    public GameObject interactionMessage;
    public GameObject player;
    private GameObject newInteractionMessage;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "InteractableObject" && newInteractionMessage == null)
        {
            var intractionObjectScript = collision.GetComponent<InteractableObjectScript>();
            if (intractionObjectScript.isEnabled)
            {
                newInteractionMessage = Instantiate(interactionMessage);
                var newInteractionMessageScript = newInteractionMessage.GetComponent<InteractionMessageScript>();
                newInteractionMessageScript.button = intractionObjectScript.button;
                newInteractionMessageScript.interactionStyle = intractionObjectScript.action;
                newInteractionMessageScript.player = player;
            }
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "InteractableObject")
        {
            var intractionObjectScript = collision.GetComponent<InteractableObjectScript>();
            if (intractionObjectScript.isEnabled)
            {
                var newInteractionMessageScript = newInteractionMessage.GetComponent<InteractionMessageScript>();
                newInteractionMessageScript.DestroySelf();
            }
        }
    }
}
