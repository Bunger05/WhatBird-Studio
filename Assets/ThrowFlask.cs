using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowFlask : MonoBehaviour
{
    public float speed = 10f;
    public float lifetime = 5f;
    public LayerMask layerMask;
    int counter = 0;
    private Rigidbody2D rb;
    public ParticleSystem explosion;
    Animator animator;
    private void Awake()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        Physics2D.IgnoreLayerCollision(8, 7, false);
    }

    private void Start()
    {
        //Destroy(gameObject, lifetime); // Destroy the projectile after its lifetime
    }

    public void Launch()
    {
        Vector2 direction;
        if (PlayerController.Instance.IsFacingRight)
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
        if (counter == 0)
        {
            if (rb != null && PlayerController.Instance.IsFacingRight)
            {
                rb.velocity = new Vector2(speed, Random.Range(3, 10));
            }
            else if (rb != null)
            {
                rb.velocity = new Vector2(-speed, Random.Range(3, 10));
            }
            rb.AddTorque(25, ForceMode2D.Force);
            counter++;
        }
      
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (((1 << collision.gameObject.layer) & layerMask) != 0)
        {
            Debug.Log("HELELLALLALALLA");
            // Handle collision logic here (e.g., apply damage to player)
            
            
            explosion.transform.position = collision.transform.position;
            //Instantiate(explosion);
            // Destroy the projectile on collision
        }
        if (collision.gameObject.layer != 7)
        {
           

        }
        animator.SetTrigger("Explode");
        rb.gravityScale = 0;
        rb.velocity = Vector2.zero;
        rb.freezeRotation = true;
        Physics2D.IgnoreLayerCollision(8, 7, true);
        
        rb.constraints = RigidbodyConstraints2D.FreezeAll;

    }
    public void OnDestroy()
    {
        Destroy(gameObject);
    }
}
