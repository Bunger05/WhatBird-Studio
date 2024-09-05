using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchingDirections : MonoBehaviour
{
    public ContactFilter2D castFilter;
    CapsuleCollider2D touchingCol;
    RaycastHit2D[] groundHits = new RaycastHit2D[5];
    RaycastHit2D[] wallHits = new RaycastHit2D[5];
    RaycastHit2D[] ceilingHits = new RaycastHit2D[5];
    public float groundDistance = 0.05f;
    public float wallDistance = 0.3f;
    public float ceilingDistance = 0.05f;
    public PhysicsMaterial2D hasFriction;
    public PhysicsMaterial2D Frictionless;
    [SerializeField]
    private bool _isGround = true;
    Animator animator;
    
    public bool IsGrounded
    {
        get
        {
            return _isGround;
        }
        private set
        {
            _isGround = value;
            //animator.SetBool(AnimationString.isGrounded, value);//good practice to store strings such as "isMoving" and "isRunning" in a seperate script
        }
    }

    public bool IsOnWallRight
    {
        get
        {
            return _isOnWallRight;
        }
        private set
        {
            _isOnWallRight = value;
            // animator.SetBool(AnimationString.isOnWall, value);//good practice to store strings such as "isMoving" and "isRunning" in a seperate script
        }
    }
    [SerializeField]private bool _onEnemy = false;
    public bool IsOnEnemy
    {
        get
        {
            return _onEnemy;
        }
        set
        {
            _onEnemy= value;    
        }
    }

    private string collidedEnemyName;
    public string CollidedEnemyName
    {
        get
        {
            return collidedEnemyName;
        }
        set
        {
            collidedEnemyName = value;
        }
    }

    public bool IsOnWallLeft
    {
        get
        {
            return _isOnWallLeft;
        }
        private set
        {
            _isOnWallLeft = value;
            // animator.SetBool(AnimationString.isOnWall, value);//good practice to store strings such as "isMoving" and "isRunning" in a seperate script
        }
    }

    private bool _isOnWall = true;
    public bool IsOnWall
    {
        get
        {
            return _isOnWall;
        }
        private set
        {
            _isOnWall = value;
           // animator.SetBool(AnimationString.isOnWall, value);//good practice to store strings such as "isMoving" and "isRunning" in a seperate script
        }
    }
    private bool _isOnCeiling = true;
    private bool _isOnWallLeft=true;
    private bool _isOnWallRight=true;

    private Vector2 wallCheckDirectionLeft =>  Vector2.left;
    private Vector2 wallCheckDirectionRight => Vector2.right;
  
    public bool IsOnCeiling
    {
        get
        {
            return _isOnCeiling;
        }
        private set
        {
            _isOnCeiling = value;
           // animator.SetBool(AnimationString.IsOnCeiling, value);//good practice to store strings such as "isMoving" and "isRunning" in a seperate script
        }
    }
    public void Awake()
    {
        
        touchingCol = GetComponent<CapsuleCollider2D>();//Use capsule collider because that is what this script is referencing
        animator = GetComponent<Animator>();
       
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            IsOnEnemy = true;
            CollidedEnemyName = collision.gameObject.name;
        }
    }
    void Start()
    {

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        IsGrounded = touchingCol.Cast(Vector2.down, castFilter, groundHits, groundDistance) > 0; //checks below
        IsOnWallLeft = touchingCol.Cast(wallCheckDirectionLeft, castFilter, wallHits, wallDistance) > 0; //checks left
        IsOnWallRight = touchingCol.Cast(wallCheckDirectionRight, castFilter, wallHits, wallDistance) > 0; //checks Right
        if(IsOnWallRight||IsOnWallLeft)
        {
            touchingCol.sharedMaterial = Frictionless;
        }
        else
        {
            touchingCol.sharedMaterial = hasFriction;
        }
        IsOnCeiling = touchingCol.Cast(Vector2.up, castFilter, ceilingHits, ceilingDistance) > 0; //checks up
    }
}
