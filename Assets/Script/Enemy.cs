using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public GameManager gameManager;
    public GameObject explosion;
    public float speed  = 1f;
    public float health = 1;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        Movement();
    }
    void Movement()
    {
        transform.Translate(new Vector3(Mathf.Sin(Time.time*1.5f),-1,0)*speed*Time.deltaTime);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision != null)
        {
            if (collision.gameObject.CompareTag("Bullet"))
            {
                gameManager.AddScore(10);
                Destroy(collision.gameObject);
                Destroy(this.gameObject);
            }
        } 
    }
}
