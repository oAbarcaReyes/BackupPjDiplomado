using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemy2 : MonoBehaviour
{
    public GameManager gameManager;
    public Transform target;
    public GameObject player;
    public float speed = 2f;
    public float health = 3;
    public float rotateSpeed = 100f;
    public Rigidbody2D rb;
    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player");
        target = player.transform;
    }

    // Update is called once per frame
    void Update()
    {
        
        
      
        
        if (target != null)
        {

            Vector2 direccion = (Vector2)target.position - rb.position;
            direccion.Normalize();
            float rotateAmount = Vector3.Cross(direccion, transform.up).z;
            rb.angularVelocity = -rotateAmount * rotateSpeed;
            rb.velocity = transform.up * speed;
        }
       
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision != null)
        {
            if (collision.gameObject.CompareTag("Bullet"))
            {
                gameManager.AddScore(30);
                
                Destroy(this.gameObject);
            }
        }
    }
   
}
