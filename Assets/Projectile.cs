using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.Android;

public class Projectile : MonoBehaviour
{
    public float speed = 10f;
    public float lifetime = 5f;
    public LayerMask layerMask;
    int counter = 0;
    private Rigidbody2D rb;
    public ParticleSystem explosion;
    private void Awake()
    {
        
        rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        //Destroy(gameObject, lifetime); // Destroy the projectile after its lifetime
    }

    public void Launch()
    {
        Vector2 direction;
        if (SpitterMovement.Instnace.IsFacingRight)
        {
             direction = new Vector2(14, 1);
        }
        else
        {
            direction = new Vector2(-14, 1);
        }
        rb.velocity = direction * speed;
    }
    private void FixedUpdate()
    {
        if(counter==0)
        {
            if (rb != null && SpitterMovement.Instnace.IsFacingRight)
            {
                rb.velocity = new Vector2(9, Random.Range(3,10));
            }
            else if (rb != null)
            {
                rb.velocity = new Vector2(-9, Random.Range(3, 10));
            }
            counter++;
        }
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (((1 << collision.gameObject.layer) & layerMask) != 0)
        {
            // Handle collision logic here (e.g., apply damage to player)
            if(collision.gameObject.layer==7)
            {
                Damageable.Instance.Hit(30);
                
            }
            else
            {
                Debug.Log("here");
                explosion.transform.position = new Vector2(collision.transform.position.x, collision.transform.position.y);
                Instantiate(explosion);
            }
            explosion.transform.position = new Vector2(collision.transform.position.x, collision.transform.position.y);
            Instantiate(explosion) ;
            Destroy(gameObject); // Destroy the projectile on collision
        }
    }
}
