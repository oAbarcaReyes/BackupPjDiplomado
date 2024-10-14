using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bulletScript : MonoBehaviour
{
    public GameManager gameManager;
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
    public virtual void Movement()
    {
        transform.Translate(Vector3.up * 5.0f * Time.deltaTime);
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision != null)
        {
            Debug.Log("collided with : " + collision.gameObject.name);
            if (collision.gameObject.CompareTag("Enemy"))
            {
                gameManager.AddScore(10);
                Destroy(collision.gameObject);
                Destroy(this.gameObject);
            }
        }
    }
}
