using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet4 : bulletScript
{
    public Transform target;
    public float speed = 5f;
    public float rotateSpeed = 100f;

    public Rigidbody2D rb;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
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
        else
        {

            Destroy(gameObject, 5f); 
        }
    }



}
