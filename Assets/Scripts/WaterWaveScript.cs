using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static ConstantsAndObjects.Constants;


public class WaterWaveScript : MonoBehaviour
{
    public int damage;
    public Vector2 direction;
    private float waveSpeed = .04f;
    public float waveDistance;
    public GameObject waterWave;
    public int numberOfWaves;
    private bool gettingBrighter = true;
    private float growthRate = .08f;
    // Start is called before the first frame update
    void Start()
    {
        Color waterColor = getWaterColor();
        Color startingWaterColor = new Color(waterColor.r, waterColor.g, waterColor.b, .4f);
        GetComponent<SpriteRenderer>().color = startingWaterColor;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Color currentColor = GetComponent<SpriteRenderer>().color;
        float currentTransperancy = currentColor.a;
        float newTransperancy = 0;
        if (currentTransperancy + waveSpeed > 1)
        {
            gettingBrighter = false;
            if (numberOfWaves > 0)
            {
                GameObject newWave = Instantiate(waterWave);
                Vector3 initialPosition = transform.position;
                newWave.transform.position = new Vector3(waveDistance * direction.x + initialPosition.x, waveDistance * direction.y + initialPosition.y, initialPosition.z);
                newWave.transform.localScale += new Vector3(growthRate*(1.7f+1/numberOfWaves), growthRate*(1.7f+1/numberOfWaves), 0);
                var newWaveScript = newWave.GetComponent<WaterWaveScript>();
                newWaveScript.damage = damage;
                newWaveScript.direction = direction;
                newWaveScript.waveDistance = waveDistance;
                newWaveScript.numberOfWaves = numberOfWaves - 1;
            }
        }
        if (gettingBrighter) 
        {
            newTransperancy = currentTransperancy + waveSpeed;
        }
        else 
        {
            newTransperancy = currentTransperancy - waveSpeed;
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

    void OnTriggerStay2D(Collider2D collision)
    {
        Color currentColor = GetComponent<SpriteRenderer>().color;
        if (collision.gameObject.tag == "Enemy" && currentColor.a > .5)
        {
            collision.gameObject.GetComponent<EnemyScript>().takeDamage(damage, getWaterColor());
            collision.gameObject.transform.position += new Vector3(direction.x * .3f, direction.y * .3f, 0);
        }
        if (collision.gameObject.tag == "Barrier")
        {
            Destroy(gameObject);
        }
    }
}
