using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerBhv : MonoBehaviour
{
    public float speed = 10.0f;
    public float fireRate = 0.25f;
    public int lives = 3;
    public int shields = 3;
    public int hommingMissile = 0;
    public float canFire = 0.0f;
    public float shieldDuration = 5.0f;
    public AudioSource actualAudio;
    public AudioManager audioManager;
    // public ShipState shipState;
    //public List<Sprite> shipSprites = new List<Sprite>();
    public List<bulletScript> bullets = new List<bulletScript>();
    public GameObject bulletPref;
    public GameObject shield;
    
    //public GameObject playerExplosion;
    public ShipState shipState;
    public List<Sprite> shipSprites = new List<Sprite>();
    public int currentWeapon;
    public RawImage vidaIMG;
    public Texture2D[] vidasIMG;

    // Start is called before the first frame update
    public enum ShipState
    {
        FullHealth,
        SlightlyDamaged,
        Damaged,
        HeavilyDamaged,
        Destroyed
    }
    void ChangeShipState()
    {
        var currentState = shipState;
        Debug.Log(currentState);

        var newSprite = shipSprites.Find(x => x.name == currentState.ToString());
        var spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = newSprite;
        switch (currentState)
        {
            case ShipState.FullHealth:
                shipState = ShipState.SlightlyDamaged;
                vidaIMG.texture = vidasIMG[0];
                break;
            case ShipState.SlightlyDamaged:
                shipState = ShipState.Damaged;
                vidaIMG.texture = vidasIMG[1];
                break;
            case ShipState.Damaged:
                shipState = ShipState.HeavilyDamaged;
                vidaIMG.texture = vidasIMG[2];
                break;
            case ShipState.HeavilyDamaged:
                shipState = ShipState.Destroyed;
                vidaIMG.texture = vidasIMG[3];
                break;
            case ShipState.Destroyed:
                vidaIMG.texture = vidasIMG[4];
                break;
            
        }
    }
    void Start()
    {
        vidaIMG.texture = vidasIMG[0];
    }

    // Update is called once per frame
    void Update()
    {
        Movement();
        CheckBoundaries();
        Fire();
        ChangeWeapon();
        UseShields();
        //ChangeShipState();

    }
    void Movement()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        transform.Translate(Vector3.right * speed * horizontalInput * Time.deltaTime);
        transform.Translate(Vector3.up * speed * verticalInput * Time.deltaTime);
    }
    void CheckBoundaries()
    {
        var cam = Camera.main;
        float xMax = cam.orthographicSize * cam.aspect;
        float yMax = cam.orthographicSize;
        if (transform.position.x > xMax)
        {
            transform.position = new Vector3(-xMax, transform.position.y, 0);
        }
        else if (transform.position.x < -xMax)
        {
            transform.position = new Vector3(xMax, transform.position.y, 0);
        }
        if (transform.position.y > yMax)
        {
            transform.position = new Vector3(transform.position.x, -yMax, 0);
        }
        else if (transform.position.y < -yMax)
        {
            transform.position = new Vector3(transform.position.x, yMax, 0);
        }

    }
    //Obtener Enemigo Más cercano para bala 4
    public Transform EnemigoCercano()
    {
        GameObject[] enemigos = GameObject.FindGameObjectsWithTag("Enemy");
        GameObject masCercano= null;
        float closestDistance = Mathf.Infinity;

        Vector3 position = transform.position;

        foreach (GameObject enemy in enemigos)
        {
            float distance = Vector3.Distance(enemy.transform.position, position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                masCercano = enemy;
            }
        }

        if (masCercano != null)
        {
            return masCercano.transform;

        }

        return null; 
    }
    void Fire()
    {
        if (Input.GetKeyDown(KeyCode.Space) && Time.time > canFire)
        {
            switch (currentWeapon)
            {
                case 0:
                    actualAudio.pitch = Random.Range(4, 4.5f);
                    actualAudio.Play();
                    Instantiate(bulletPref, transform.position + new Vector3(0, 0.8f, 0), Quaternion.identity);
                    canFire = Time.time + fireRate;
                    break;
                case 1:
                    actualAudio.pitch = Random.Range(5, 5.5f);
                    actualAudio.Play();
                    Instantiate(bulletPref, transform.position + new Vector3(-1, 0.8f, 0), Quaternion.identity);
                    Instantiate(bulletPref, transform.position + new Vector3(0, 0.8f, 0), Quaternion.identity);
                    Instantiate(bulletPref, transform.position + new Vector3(-1, 0.8f, 0), Quaternion.identity);
                    canFire = Time.time + fireRate;
                    break;
                case 2:
                    actualAudio.pitch = Random.Range(1, 2f);
                    actualAudio.Play();
                    Instantiate(bulletPref, transform.position + new Vector3(0, 0.5f, 0), Quaternion.identity);
                    canFire = Time.time + fireRate;
                    break;
                case 3:
                    if (hommingMissile >0)
                    {
                        actualAudio.pitch = Random.Range(2.5f, 3f);
                        actualAudio.Play();
                        GameObject bullet = Instantiate(bulletPref, transform.position + new Vector3(0, 0.2f, 0), Quaternion.identity);
                        bullet.GetComponent<Bullet4>().target = EnemigoCercano();
                        canFire = Time.time + fireRate + 0.5f;
                        hommingMissile--;
                    }
                    
                    break;
             
            }
            
          
        }

    }
    void UseShields()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift)&& shields > 0)
        {
            shields--;
            shield.SetActive(true);
            gameObject.GetComponent<BoxCollider2D>().enabled = false;
        }
        if (shield.activeSelf)
        {
            shieldDuration -= Time.deltaTime;
            if(shieldDuration < 0)
            {
                shield.SetActive(false);
                shieldDuration = 5.0f;
                gameObject.GetComponent<BoxCollider2D>().enabled = true;
            }
        }
    }
    void ChangeWeapon()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            bulletPref = bullets[0].gameObject;
            currentWeapon = 0;
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            bulletPref = bullets[1].gameObject;
            currentWeapon = 1;
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            bulletPref = bullets[2].gameObject;
            currentWeapon = 2;
        }
        //Bullet 4
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            bulletPref = bullets[3].gameObject;
            currentWeapon = 3;

        }
    }
    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision != null)
        {
            ChangeShipState();
            if (collision.gameObject.CompareTag("Enemy"))
            {
                Destroy(collision.gameObject);
                Debug.Log("Collision");
                if (lives > 0)
                {

                    lives--;
                    
                }
                else
                {
                    Destroy(this.gameObject);
                }
            }
        }
    }
    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision != null)
        {
            if (collision.gameObject.CompareTag("Enemy"))
            {
                Destroy(collision.gameObject);
                Debug.Log("Trigger Enter");
                if (lives > 1)
                {
                    ChangeShipState();
                    lives--;
                }
                else
                {
                    lives--;
                    Destroy(this.gameObject);
                }
            }
            if (collision.gameObject.CompareTag("PowerUp"))
            {
                Destroy(collision.gameObject);
                Debug.Log("Trigger Enter");
                hommingMissile += 3;
            }
        }
    }

}

