using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Timers;
using static ConstantsAndObjects.Constants;

public class PlayerScript : MonoBehaviour
{
    Controls controls;
    private Rigidbody2D rb;
    private int currentCount;

    // Game Object variables
    public GameObject interactionMessage;
    private GameObject newInteractionMessage;
    public GameObject enemyDetector;
    public GameObject lightningStrikeObject;
    public GameObject waterWaveObject;
    public GameObject fireBallObject;
    public GameObject earthPoundObject;
    public GameObject lightningAttackArea;
    public GameObject currentLightningAttackArea;
    public GameObject currentTarget;
    public HealthBar healthBar;

    // Player Stats
    public int currentHealth;
    public int maxHealth;
    public float movementSpeed;
    public int lightningStikeCount;
    public int lightningStrikeDamage;
    public float lightningStrikeSpeed;
    public int lightningStrikeCooldown;
    public float fireRate;
    public int burstNumber;
    public float burstFireRate;
    public int fireBallDamage;
    public float dashDistance;
    public int dashCount;
    public float dashCooldown;
    public float dashRefreshRate;
    public int waterWaveDamage;
    public float waterWaveDistance;
    public int waterWaveCooldown;
    public int numberOfWaves;
    public int lightningAttackAreaSize;




    // Player Status
    private bool canMove = true;
    public Vector2 aimDirection;
    public bool isMovingLeft;
    public bool isMovingRight;
    public bool canMoveLeft = true;
    public bool canMoveRight = true;
    public bool canMoveDown = true;
    public bool canMoveUp = true;
    public bool canFire = true;
    public bool canDash = true;
    public int dashesAvailable;
    public bool canLightningStrike = true;
    public bool canWaterWave = true;
    private bool isShootingFireballs = false;
    private bool isAimingLightningStike = false;
    Vector2 move;
    Vector2 aim;




    // Start is called before the first frame update
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();

        controls = new Controls();

        controls.Gameplay.Fire.performed += context => FireballStart();

        controls.Gameplay.Dash.performed += context => Dash();

        controls.Gameplay.Water.performed += context => WaterWave();

        controls.Gameplay.Earth.performed += context => EarthStomp();

        controls.Gameplay.Lightning.started += context => LightningStrikeStart();

        controls.Gameplay.Lightning.canceled += context => LightningStrikeEnd();

        controls.Gameplay.Fire.canceled += context => FireballEnd();

        controls.Gameplay.Move.performed += context => move = context.ReadValue<Vector2>();

        controls.Gameplay.Aim.performed += context => aim = context.ReadValue<Vector2>();

        controls.Gameplay.Move.canceled += context => move = Vector2.zero;
    }

    void Start()
    {
        healthBar.SetMaxHealth(maxHealth);

        healthBar.SetHealth(currentHealth);

        dashesAvailable = dashCount;

    }

    void Update()
    {
        if (canMove && !isAimingLightningStike)
        {
            if ((!canMoveLeft && move.x < 0) || (!canMoveRight && move.x > 0))
            {
                Vector2 movement = new Vector2(0, move.y) * Time.deltaTime * movementSpeed;
                transform.Translate(movement, Space.World); 
            }
            else if ((!canMoveDown && move.y < 0)  || (!canMoveUp && move.y > 0) )
            {
                Vector2 movement = new Vector2(move.x, 0) * Time.deltaTime * movementSpeed;
                transform.Translate(movement, Space.World); 
            }
            else 
            {
                Vector2 movement = new Vector2(move.x, move.y) * Time.deltaTime * movementSpeed;
                transform.Translate(movement, Space.World); 
            }
        }
        if (isAimingLightningStike)
        {
            {
                Vector2 movement = new Vector2(move.x, move.y) * Time.deltaTime * movementSpeed;
                currentLightningAttackArea.transform.Translate(movement, Space.World); 
            }
        }
        
        if (Mathf.Abs(move.x) > .5 || Mathf.Abs(move.y) > .5 )
        {
            aimDirection = new Vector2(move.x, move.y);
            aimDirection.Normalize();
        }
        else if (Mathf.Abs(move.x) < .5 && Mathf.Abs(move.y) < .5 && isShootingFireballs)
        {
            GameObject closestEnemy = getCurrentTarget();
            if (closestEnemy != null)
            {
                Vector2 enemyDirection = new Vector2(closestEnemy.transform.position.x-transform.position.x, closestEnemy.transform.position.y-transform.position.y);
                enemyDirection.Normalize();
                aimDirection = enemyDirection;
            }
        }
        if (Mathf.Abs(aim.x) > .5 || Mathf.Abs(aim.y) > .5 )
        {
            aimDirection = new Vector2(aim.x, aim.y);
            aimDirection.Normalize();
        }



        Vector2 verticlVector = new Vector2(0,1);
        float rotationAngle = (180/Mathf.PI) * Mathf.Acos(Vector2.Dot(verticlVector, aimDirection));  
        if (aimDirection.x > 0)
        {
            rotationAngle = 360 - rotationAngle;
        }        
        transform.rotation = Quaternion.Euler(0, 0, rotationAngle);


        if (isShootingFireballs && canFire)
        {
            canFire = false;
            gameObject.GetComponent<SpriteRenderer>().color = getFireColor();
            StartCoroutine(FireballCoroutine());
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Barrier")
        {
            for (int i = 0; i < collision.contactCount; i++)
            {
                print(collision.GetContact(i).normal);
                if (collision.GetContact(i).normal.x > 0)
                {
                    canMoveLeft = false;
                }
                if (collision.GetContact(i).normal.x <  0)
                {
                    canMoveRight = false;
                }
                if (collision.GetContact(i).normal.y > 0)
                {
                    canMoveDown = false;
                }
                if (collision.GetContact(i).normal.y <  0)
                {
                    canMoveUp = false;
                }
            }
        }
        
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Barrier")
        {
            canMoveRight = true;
            canMoveLeft = true;
            canMoveUp = true;
            canMoveDown = true;
        }
        
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "InteractableObject")
        {
            var intractionObjectScript = collision.gameObject.GetComponent<InteractableObjectScript>();
            if (intractionObjectScript.isEnabled)
            {
                newInteractionMessage = Instantiate(interactionMessage);
                var newInteractionMessageScript = newInteractionMessage.GetComponent<InteractionMessageScript>();
                newInteractionMessageScript.button = intractionObjectScript.button;
                newInteractionMessageScript.interactionStyle = intractionObjectScript.action;
                newInteractionMessageScript.player = gameObject;
            }
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "InteractableObject")
        {
            var intractionObjectScript = collision.gameObject.GetComponent<InteractableObjectScript>();
            if (intractionObjectScript.isEnabled)
            {
                var newInteractionMessageScript = newInteractionMessage.GetComponent<InteractionMessageScript>();
                newInteractionMessageScript.DestroySelf();
            }
        }
    }
    void LightningStrikeStart()
    {
        if (canLightningStrike)
        {

            gameObject.GetComponent<SpriteRenderer>().color = getLightningColor();
            currentLightningAttackArea = Instantiate(lightningAttackArea);
            currentLightningAttackArea.transform.localScale += new Vector3(lightningAttackAreaSize, lightningAttackAreaSize, 0);
            currentLightningAttackArea.GetComponent<LightningDetectorScript>().transform.position = getCurrentTarget().transform.position;
            
            isAimingLightningStike = true;
            canFire = false;
            canWaterWave = false;

        }
    }

    void LightningStrikeEnd()
    {
        if (canLightningStrike && currentLightningAttackArea != null)
        {
            canLightningStrike = false;
            canFire = true;
            canWaterWave = true;
            isAimingLightningStike = false;
            StartCoroutine(LightningStrikeRoutine());
        }
    }

    void WaterWave()
    {
        if (canWaterWave)
        {
            gameObject.GetComponent<SpriteRenderer>().color = getWaterColor();        
            canWaterWave = false;
            StartCoroutine(WaterWaveCoroutine());
        }
    }

    void EarthStomp()
    {
        gameObject.GetComponent<SpriteRenderer>().color = getEarthColor();

    }

    void FireballStart()
    {
        isShootingFireballs = true;
        canMove = false;
    }

    void FireballEnd()
    {
        isShootingFireballs = false;
        canMove = true;
    }

    IEnumerator LightningStrikeRoutine()
    {
        currentCount = 0;
        List<GameObject> listOfTargets = new List<GameObject>(lightningStikeCount);
        Dictionary<int, GameObject> selectedEnemies = currentLightningAttackArea.GetComponent<LightningDetectorScript>().enemyDictionary;
        foreach (var enemyPair in selectedEnemies)
        {   
            GameObject enemy = enemyPair.Value;
            if (currentCount != lightningStikeCount)
            {
                listOfTargets.Add(enemy);
                currentCount += 1;

            }
        }
        foreach (GameObject enemy in listOfTargets)
        {
            print("THis is working!");
            yield return new WaitForSeconds(lightningStrikeSpeed);
            GameObject newLightningStrike = Instantiate(lightningStrikeObject);
            newLightningStrike.GetComponent<LightningStikeScript>().damage = lightningStrikeDamage;
            newLightningStrike.GetComponent<LightningStikeScript>().target = enemy;
        }
        Destroy(currentLightningAttackArea);
        yield return new WaitForSeconds(lightningStrikeCooldown);
        canLightningStrike = true;
    }

    IEnumerator FireballCoroutine()
    {
        if (move.x < 0.5 && move.y < 0.5)
        {

        }
        for (int i = 0; i < burstNumber; i++)
        {
            GameObject newFireball = Instantiate(fireBallObject);
            newFireball.GetComponent<BulletScript>().damage = 1;
            Rigidbody2D newFireballRB = newFireball.GetComponent<Rigidbody2D>();
            newFireball.transform.position = gameObject.transform.position;
            newFireballRB.velocity = aimDirection * 15;
            Vector2 verticlVector = new Vector2(0,1);
            float rotationAngle = (180/Mathf.PI) * Mathf.Acos(Vector2.Dot(verticlVector, aimDirection));  
            if (aimDirection.x > 0)
            {
                rotationAngle = 360 - rotationAngle;
            }        
            newFireball.transform.rotation = Quaternion.Euler(0, 0, rotationAngle); 
            if (burstNumber > 1)
            {
                yield return new WaitForSeconds(burstFireRate);
            }
        }
        yield return new WaitForSeconds(fireRate);
        canFire = true;

    }

    void Dash()
    {
        if (canDash && dashesAvailable > 0) 
        {
            canDash = false;
            StartCoroutine(DashCoroutine());
        }
    }

    IEnumerator DashCoroutine()
    {
        Vector3 initialPosition = gameObject.transform.position;
        transform.position = new Vector3(dashDistance * move.x + initialPosition.x, dashDistance * move.y + initialPosition.y, initialPosition.z);
        dashesAvailable -= 1;
        if (dashCooldown < dashRefreshRate)
        {
            yield return new WaitForSeconds(dashCooldown);
            canDash = true;
            yield return new WaitForSeconds(dashRefreshRate - dashCooldown);
            dashesAvailable += 1;
        }
        if (dashRefreshRate < dashCooldown)
        {
            yield return new WaitForSeconds(dashRefreshRate - dashCooldown);
            dashesAvailable += 1;
            yield return new WaitForSeconds(dashCooldown);
            canDash = true;
            
        }
    }

    IEnumerator WaterWaveCoroutine()
    {
        Vector3 axis = Vector3.Cross(aim, Vector3.up);
        GameObject newWave = Instantiate(waterWaveObject);
        newWave.transform.position = transform.position + new Vector3(aimDirection.x * 1f, aimDirection.y * 1f, 0);
        var newWaveScript = newWave.GetComponent<WaterWaveScript>();
        newWaveScript.damage = waterWaveDamage;
        newWaveScript.direction = aimDirection;
        newWaveScript.waveDistance = waterWaveDistance;
        newWaveScript.numberOfWaves = numberOfWaves - 1;

        newWave = Instantiate(waterWaveObject);
        newWave.transform.position = transform.position + new Vector3(aimDirection.x * 1f, aimDirection.y * 1f, 0);
        newWaveScript = newWave.GetComponent<WaterWaveScript>();
        newWaveScript.damage = waterWaveDamage;
        newWaveScript.direction = Quaternion.AngleAxis(25, axis) * aimDirection;
        newWaveScript.waveDistance = waterWaveDistance;
        newWaveScript.numberOfWaves = numberOfWaves - 1;

        newWave = Instantiate(waterWaveObject);
        newWave.transform.position = transform.position + new Vector3(aimDirection.x * 1f, aimDirection.y * 1f, 0);
        newWaveScript = newWave.GetComponent<WaterWaveScript>();
        newWaveScript.damage = waterWaveDamage;
        newWaveScript.direction = Quaternion.AngleAxis(-25, axis) * aimDirection;
        newWaveScript.waveDistance = waterWaveDistance;
        newWaveScript.numberOfWaves = numberOfWaves - 1;
        yield return new WaitForSeconds(waterWaveCooldown);
        canWaterWave = true;

    }

    void OnEnable()
    {
        controls.Gameplay.Enable();
    }

    void OnDisable()
    {
        controls.Gameplay.Disable();
    }

    GameObject getCurrentTarget()
    {
        return enemyDetector.GetComponent<EnemyDetectorScript>().closestEnemy;
    }
}