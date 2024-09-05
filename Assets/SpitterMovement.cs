using System.Collections;
using System.Collections.Generic;
using Unity.Entities.UniversalDelegates;
using UnityEngine;

public class SpitterMovement : MonoBehaviour
{
    
    public GameObject projectilePrefab;
    public Transform projectileSpawnPoint;
    public GameObject coin;
    //public float attackInterval = 2f;
    public static SpitterMovement Instnace { get; private set; }
    Rigidbody2D rb;
    float timeCounter = 0;
    private bool _isAttacking;
    float attackMoveTimer;
    int counter = 0;
    Collider2D col;
    Animator animator;
    Attack attack;
    TouchingDirections touchingDirections;
    PlayerController playerController;
    CollisionDetector collisionDetector;
    EnemyDamageable spitterDamageable;
    private float walkSpeed = 4f;
    private float runSpeed = 6f;
    private bool _isFacingRight = true;
    private Transform player;
    public GameObject projectileInstance;
    private float deathTimer;
    
    [SerializeField] private float attackTimer = 0f;
    private bool _attack = false;
    public bool Attack2
    {
        get
        {
            return _attack;

        }
        set
        {
            _attack = value;
        }
    }
   
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
    private float attackCooldownTimer;

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
    private void SwitchDirections()
    {
        if ((touchingDirections.IsOnWallLeft && !HasTarget) || (rb.velocity.x > 0 && HasTarget && !_isAttacking))
        {
            IsFacingRight = true;
        }
        else if ((touchingDirections.IsOnWallRight && !HasTarget) || (rb.velocity.x < 0 && HasTarget && !_isAttacking))
        {
            IsFacingRight = false;
        }
    }
    private void Awake()
    {
        col=GetComponent<CapsuleCollider2D>();  
        Instnace = this;
        rb = GetComponent<Rigidbody2D>();
        collisionDetector = GetComponentInChildren<CollisionDetector>();
        animator = GetComponent<Animator>();
        touchingDirections = GetComponent<TouchingDirections>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        spitterDamageable = GetComponent<EnemyDamageable>();
        attack = player.GetComponent<Attack>();
        //projectilePrefab=GetComponentInChildren<Projectile>();
    }
   
    // Update is called once per frame
    void Update()
    {

    }
    public void Destory()
    {
        Destroy(gameObject);
    }
    
    public void Attacking()
    {
        if (IsFacingRight)
        {
            Instantiate(projectilePrefab, new Vector2(transform.position.x + 0.5f, transform.position.y + 1), transform.rotation);
            Instantiate(projectilePrefab, new Vector2(transform.position.x + 0.5f, transform.position.y + 0.4f), transform.rotation);
            Instantiate(projectilePrefab, new Vector2(transform.position.x + 0.5f, transform.position.y + 1.6f), transform.rotation);
        }
        else
        {
            Instantiate(projectilePrefab, new Vector2(transform.position.x - 0.5f, transform.position.y + 1), transform.rotation);
            Instantiate(projectilePrefab, new Vector2(transform.position.x - 0.5f, transform.position.y + 1.6f), transform.rotation);
            Instantiate(projectilePrefab, new Vector2(transform.position.x - 0.5f, transform.position.y + 0.4f), transform.rotation);
        }
    }
    public void UnFreezeMovement()
    {
        _isAttacking = false;
    }
    private void FixedUpdate()
    {
        //Debug.Log("HasTarget: " + HasTarget);
        //Debug.Log(Attack.Instance.)
        if (HasTarget && Mathf.Abs((transform.position.y - player.position.y)) < 10 && Mathf.Abs(transform.position.x - player.position.x) < 50)
        {
            HasTarget = true;
        }
        else
        {
            HasTarget = collisionDetector.CanSeePlayer;
        }
        /*( if (Attack2)
         {
             attackTimer += Time.deltaTime;

         }
         if (attackTimer > 0.7f)
         {
             /*if (IsFacingRight)
             {
                 Instantiate(projectilePrefab, new Vector2(transform.position.x + 0.5f, transform.position.y + 1), transform.rotation);
                 Instantiate(projectilePrefab, new Vector2(transform.position.x + 0.5f, transform.position.y + 0.4f), transform.rotation);
                 Instantiate(projectilePrefab, new Vector2(transform.position.x + 0.5f, transform.position.y + 1.6f), transform.rotation);
             }
             else
             {
                 Instantiate(projectilePrefab, new Vector2(transform.position.x - 0.5f, transform.position.y + 1), transform.rotation);
                 Instantiate(projectilePrefab, new Vector2(transform.position.x - 0.5f, transform.position.y + 1.6f), transform.rotation);
                 Instantiate(projectilePrefab, new Vector2(transform.position.x - 0.5f, transform.position.y + 0.4f), transform.rotation);
             }
             attackTimer = 0;
             Attack2 = false;
         }*/
        attackCooldownTimer += Time.fixedDeltaTime;
        //Debug.Log(attack.AttackSucess);
        //Debug.Log(transform.gameObject.name);
        float distance = Vector2.Distance(transform.position, player.position);
        float distance2 = transform.position.x - player.position.x;
        
            SwitchDirections();
        
       
        if (deathTimer > 0.25)
        {
            Instantiate(coin, transform.position, Quaternion.identity);
            attack.AttackSucess = false;
            //
            //GameObject.Destroy(gameObject);
        }
        if (!spitterDamageable.IsAlive)
        {
            Debug.Log("WTF");
            animator.SetBool("IsAlive", false);
            Physics2D.IgnoreLayerCollision(8, 7, true);
            Physics2D.IgnoreLayerCollision(8, 9, true);
            deathTimer += Time.fixedDeltaTime;
        }
        else if (attack.EnemyTag.Contains("Spitter") || (timeCounter > 0 && timeCounter < 0.2f))
        {
            Debug.Log("attacking");
            timeCounter += Time.fixedDeltaTime;
            if (counter == 0)
            {
                //spitterDamageable.Hit(25);
                rb.velocity = Vector2.zero;
                counter++;
            }
            if (distance2 < 0)
            {
                Debug.Log(rb.gameObject.name + " Left");
                rb.velocity = (new Vector2(-6, 2));
                IsFacingRight = true;
            }
            else if (distance2 > 0)
            {
                Debug.Log(rb.gameObject.name + " Right");
                rb.velocity = (new Vector2(6, 2));
                IsFacingRight = false;
            }

            attack.AttackSucess = false;
            attack.EnemyTag = "";
        }

        else if (_isAttacking)
        {
            Debug.Log("Helloooooo");
            rb.velocity = Vector2.zero;
            attackCooldownTimer = 0;
        }
       
        else if ((!attack.AttackSucess && timeCounter > 0.2f) || (!attack.AttackSucess && timeCounter == 0) )
        {
            attackMoveTimer = 0;
            timeCounter = 0;
            counter = 0;

            if ((!HasTarget && IsFacingRight))
            {
                // rb.MovePosition(new Vector2(walkSpeed,0));
                rb.velocity = new Vector2(walkSpeed, rb.velocity.y);
            }
            else if (!HasTarget && !IsFacingRight)
            {
                rb.velocity = (new Vector2(-walkSpeed, rb.velocity.y));
            }
            if ((HasTarget && Mathf.Abs(distance) > 10)||(HasTarget&&Mathf.Abs(distance)<10&&attackCooldownTimer<2))
            {
                Vector2 direction = (player.position - transform.position).normalized;
                direction.y = 0;
                //float speed = (Mathf.Abs(distance) > 5) ? runSpeed : walkSpeed;
                rb.velocity = direction * runSpeed/2;

            }
            else if (HasTarget && Mathf.Abs(distance) <= 10&&attackCooldownTimer>2)
            {
               
                _isAttacking = true;
               
                attackCooldownTimer = 0;
                Vector2 direction = (player.position - transform.position).normalized;
                rb.velocity = direction * 1;
                animator.SetTrigger("Attack");
                Attack2 = true;
               
              
                


            }
           
            if (rb.velocity.x > 0 || rb.velocity.x < 0)
            {
                animator.SetBool("IsMoving", true);
            }
        }
        

        //SetFacingDirection(new Vector2(rb.velocity.x,rb.velocity.y));
    }
}
