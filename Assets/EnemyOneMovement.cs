using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class EnemyOneMovement : MonoBehaviour
{
    public GameObject coin;
    Rigidbody2D rb;
    private bool willDie = false;
    private float deathTimer = 0f;
    private float deathTimer2 = 0f;
    float timeCounter = 0;
    int counter = 0;
    Collider2D col;
    Animator animator;
    Attack attack;
    TouchingDirections touchingDirections;
    PlayerController playerController;
    CollisionDetector collisionDetector;
    EnemyDamageable enemyDamageable;
    private float walkSpeed = 4f;
    private float runSpeed = 6f;
    private bool _isFacingRight=true;
    private Transform player;
    private Collider2D collider;
    public float originalSpeed;
    public bool IsFacingRight
    {
        get
        {
            return _isFacingRight;
        }
        set
        {
            if (_isFacingRight != value)
            {
                transform.localScale *= new Vector2(-1, 1);

            }
            _isFacingRight = value;
        }
    }
    private void SetFacingDirection()
    {

        if ((rb.velocity.x > 0 && !touchingDirections.IsOnWallRight))
        {

            IsFacingRight = true;
        }

        else if ((rb.velocity.x < 0 && !touchingDirections.IsOnWallLeft))
        {
            IsFacingRight = false;
        }
    }
    private bool _hasTarget;
    public bool HasTarget
    {
        get
        {
            return _hasTarget;
        }
        set
        {
            _hasTarget = value;
        }
       
    }

    private float attackMoveTimer = 0f;

    private void SwitchDirections()
    {
        if (touchingDirections.IsOnWallLeft&&!collisionDetector.CanSeePlayer)
        {
            IsFacingRight = true;
        }
        else if (touchingDirections.IsOnWallRight && !collisionDetector.CanSeePlayer)
        {
            IsFacingRight = false;
        }
    }
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        collisionDetector = GetComponentInChildren<CollisionDetector>();
        animator = GetComponent<Animator>();
        originalSpeed = animator.speed;
        collider=GetComponent<CapsuleCollider2D>();
        touchingDirections = GetComponent<TouchingDirections>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        enemyDamageable = GetComponent<EnemyDamageable>();
        attack = player.GetComponent<Attack>();
    }
    public void SizeUp()
    {
        transform.localScale = new Vector2(3, 3);
        rb.velocity = Vector2.zero;
        //collider.isTrigger = true;
        
    }
    public void Destroy()
    {
        Destroy(gameObject);
    }
    public void Shake()
    {
        CinemachineShake.Instance.ShakeCamera(5f, 0.5f);
    }
    
    // Update is called once per frame
    void Update()
    {
        
    }
    private void FixedUpdate()
    {
        if(rb.velocity.x<originalSpeed)
        {
            animator.speed = 2;
        }
        else
        {
            animator.speed = rb.velocity.x;
        }
       
        //Debug.Log(Attack.Instance.EnemyTag);
        //Debug.Log(attack.AttackSucess);
        float distance = Vector2.Distance(transform.position, player.position);
        float distance2 = transform.position.x-player.position.x; ;
        SwitchDirections();
        if(deathTimer2>0.25f)
        {
            Instantiate(coin, transform.position, Quaternion.identity);
            attack.AttackSucess = false;
            GameObject.Destroy(gameObject);
            
        }
        if (!enemyDamageable.IsAlive)
        {
            animator.SetBool("IsAlive", false);
            Physics2D.IgnoreLayerCollision(8, 7, true);
            Physics2D.IgnoreLayerCollision(8, 9, true);
            deathTimer2 += Time.fixedDeltaTime;
        }
        else if (attackMoveTimer < 1 && attackMoveTimer > 0)
        {
            rb.velocity = Vector2.zero;
            attackMoveTimer += Time.fixedDeltaTime;
            if (attackMoveTimer > 0.9)
            {
                Instantiate(coin, transform.position, Quaternion.identity);
                attack.AttackSucess = false;
                //GameObject.Destroy(gameObject);
            }
        }
        else if ((!attack.AttackSucess && timeCounter > 0.3) || (!attack.AttackSucess && timeCounter == 0) || ((attack.EnemyTag != "Bloater" && timeCounter > 0.3)))
        {
            
            attackMoveTimer = 0;
            timeCounter = 0;
            counter = 0;
            
            if (!HasTarget && IsFacingRight)
            {
                // rb.MovePosition(new Vector2(walkSpeed,0));
                rb.velocity = new Vector2(walkSpeed, rb.velocity.y);
            }
            else if (!HasTarget && !IsFacingRight)
            {
                rb.velocity = (new Vector2(-walkSpeed, 0));
            }
            if (collisionDetector.CanSeePlayer&&distance>5)
            {
                
                Vector2 direction = (player.position - transform.position).normalized;
                direction.y = 0;
                //float speed = (Mathf.Abs(distance));
                rb.velocity = new Vector2(direction.x * runSpeed, rb.velocity.y);
                deathTimer = 0;
            }
            else if(collisionDetector.CanSeePlayer&&distance<=5)
            {
                
                deathTimer += Time.fixedDeltaTime;
                animator.SetTrigger("Tick");


            }
            if(deathTimer>0.8f)
            {
                willDie = true;
               
                


            }
             if(willDie)
            {
                attackMoveTimer = 0.001f;
                animator.SetTrigger("Explode");
                deathTimer2 += Time.fixedDeltaTime;
                
            }

            
            
            if (rb.velocity.x > 0 || rb.velocity.x < 0)
            {
                animator.SetBool("IsMoving", true);
            }
        }
        else if(attack.EnemyTag.Contains("Bloater") || (timeCounter > 0 && timeCounter < 0.1f))
        {
            Debug.Log("Fuck");
            
            timeCounter += Time.fixedDeltaTime;
            if (counter == 0)
            {
                //enemyDamageable.Hit(25);
                rb.velocity = Vector2.zero;
                counter++;
            }
            if(distance2<0)
            {
                Debug.Log(rb.gameObject.name+" Left");
                rb.velocity =(new Vector2(-6, 2));
            }
            else if(distance2>0)
            {
                Debug.Log(rb.gameObject.name + " Right");
                rb.velocity = (new Vector2(6, 2));
            }
            
            attack.AttackSucess = false;
            //Attack.Instance.EnemyTag = "";
        }
        //SetFacingDirection(new Vector2(rb.velocity.x,rb.velocity.y));
    }
}
