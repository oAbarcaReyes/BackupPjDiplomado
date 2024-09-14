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
    //public GameObject shield;
    //public GameObject playerExplosion;

    // Start is called before the first frame update
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

