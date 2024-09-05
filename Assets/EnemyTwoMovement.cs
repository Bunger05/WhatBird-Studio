using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTwoMovement : MonoBehaviour
{
    public GameObject coin;
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
    EnemyDamageable enemyTwoDamageable;
    private float walkSpeed = 4f;
    private float runSpeed = 6f;
    private bool _isFacingRight = true;
    private Transform player;
    private float deathTimer;
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
  
    private bool _hasTarget=false;
    private float attackTimer;

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
        if ((touchingDirections.IsOnWallLeft && !collisionDetector.CanSeePlayer)||(rb.velocity.x>0&&HasTarget&&!_isAttacking&& transform.position.x - player.position.x < 0) ||(rb.velocity.x < 0 && HasTarget && !_isAttacking&& transform.position.x - player.position.x<0))
        {
           
            IsFacingRight = true;
        }
        else if ((touchingDirections.IsOnWallRight && !collisionDetector.CanSeePlayer)||(rb.velocity.x<0&&HasTarget&&!_isAttacking&& transform.position.x - player.position.x > 0) || (rb.velocity.x > 0 && HasTarget && !_isAttacking && transform.position.x - player.position.x > 0))
        {
            IsFacingRight = false;
        }
    }
    public void UnFreezeMovement()
    {
        _isAttacking = false;
    }
    private void Awake()
    {
        col=GetComponent<CapsuleCollider2D>();  
        rb = GetComponent<Rigidbody2D>();
        collisionDetector = GetComponentInChildren<CollisionDetector>();
        animator = GetComponent<Animator>();
        touchingDirections = GetComponent<TouchingDirections>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        enemyTwoDamageable = GetComponent<EnemyDamageable>();
        attack = player.GetComponent<Attack>();
    }
    public void Destroy()
    {
        Destroy(gameObject);
    }

    // Update is called once per frame
    void Update()
    {

    }
    private void FixedUpdate()
    {

        if (HasTarget && Mathf.Abs((transform.position.y - player.position.y)) < 10 && Mathf.Abs(transform.position.x - player.position.x) < 30)
        {
            HasTarget = true;
        }
        else
        {
            HasTarget = collisionDetector.CanSeePlayer;
        }
        //Debug.Log(attack.AttackSucess);
        //Debug.Log(transform.gameObject.name);
        float distance = Vector2.Distance(transform.position, player.position);
        float distance2 = transform.position.x - player.position.x;
        SwitchDirections();
        if(deathTimer>0.25)
        {
            Instantiate(coin, transform.position, Quaternion.identity);
            attack.AttackSucess = false;
            //GameObject.Destroy(gameObject);
        }
        if (!enemyTwoDamageable.IsAlive)
        {
            animator.SetBool("IsAlive", false);
            Physics2D.IgnoreLayerCollision(8, 7, true);
            //Physics2D.IgnoreLayerCollision(8, 9, true);
            deathTimer += Time.fixedDeltaTime;
        }
        else if (attackMoveTimer<1&&attackMoveTimer>0)
        {
            rb.velocity = Vector2.zero;
            attackMoveTimer += Time.fixedDeltaTime;
            attack.AttackSucess = false;
            attack.EnemyTag = "";
        }
        else if (attack.EnemyTag.Contains("SkinnyBloater") || (timeCounter > 0 && timeCounter < 0.15f)&&(attackMoveTimer==0||attackMoveTimer>1))
        {
            Debug.Log("distance " + distance);
            timeCounter += Time.fixedDeltaTime;
            if (counter == 0)
            {
                //enemyTwoDamageable.Hit(25);
                rb.velocity = Vector2.zero;
                counter++;
            }
            if (distance2 < 0)
            {
                IsFacingRight = true;
                Debug.Log(rb.gameObject.name + " Left");
                rb.velocity = (new Vector2(-6, 2));
            }
            else if (distance2 > 0)
            {
                IsFacingRight = false;
                Debug.Log(rb.gameObject.name + " Right");
                rb.velocity = (new Vector2(6, 2));
            }

            attack.AttackSucess = false;
            attack.EnemyTag = "";
        }
        else if ((!attack.AttackSucess && timeCounter > 0.15 ) || (!attack.AttackSucess && timeCounter == 0 )|| ((attack.EnemyTag != "SkinnyBloater" && timeCounter > 0.3)))
        {
            Debug.Log("H: " +HasTarget);
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
                rb.velocity = (new Vector2(-walkSpeed, rb.velocity.y));
            }
            if (HasTarget&&Mathf.Abs(distance)>2)
            {
                Vector2 direction = (player.position - transform.position).normalized;
                direction.y = 0;
                //float speed = (Mathf.Abs(distance) > 5) ? runSpeed : walkSpeed;
                rb.velocity = direction * runSpeed;

            }
            else if(HasTarget && Mathf.Abs(distance) <= 2)
            {
               
                _isAttacking = true;
                attackMoveTimer = 0.001f;
                Vector2 direction = (player.position - transform.position).normalized;
                rb.velocity = direction * 1;
                animator.SetTrigger("Attack");
            }
           
            if (rb.velocity.x > 0 || rb.velocity.x < 0)
            {
                animator.SetBool("IsMoving", true);
            }
        }
       

        //SetFacingDirection(new Vector2(rb.velocity.x,rb.velocity.y));
    }
}
