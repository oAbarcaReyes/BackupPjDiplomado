using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBhv : MonoBehaviour
{
    public float speed = 10.0f;
    public float fireRate = 0.25f;
    public int lives = 3;
    public int shields = 3;
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
                break;
            case ShipState.SlightlyDamaged:
                shipState = ShipState.Damaged;
                break;
            case ShipState.Damaged:
                shipState = ShipState.HeavilyDamaged;
                break;
            case ShipState.HeavilyDamaged:
                shipState = ShipState.Destroyed;
                break;
            case ShipState.Destroyed:
                break;
            
        }
    }
    void Start()
    {

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
    void Fire()
    {
        if (Input.GetKeyDown(KeyCode.Space) && Time.time > canFire)
        {
            actualAudio.Play();
            Instantiate(bulletPref, transform.position + new Vector3(0, 0.8f, 0), Quaternion.identity);
            canFire = Time.time + fireRate;
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
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            bulletPref = bullets[1].gameObject;
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            bulletPref = bullets[2].gameObject;
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
        }
    }
}

