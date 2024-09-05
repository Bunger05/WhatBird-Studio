using CameraShake;
using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public GameObject selectedProjectile;
    public static PlayerController Instance { get; private set; }
    public GameObject spitter;
    public GameObject walker;
    public GameObject bloater;
    public CameraShaker cameraShaker;
    private int dustCounter = 0;
    private float forceTimer = 0f;
    public ParticleSystem dust;
    public ParticleSystem blood;
    private int counterA = 0;
    private int counterB = 0;
    Attack attack;
    DownAttack downAttack;
    Rigidbody2D rb;
    Vector2 moveInput;
    public float cayoteTimer;
    public float walkSpeed = 20f;
    public float runSpeed = 7.5f;
    public float jumpRight = -9f;
    public float jumpLeft = 9f;
    private float maxJumpHoldTime = 1.25f;
    Animator animator;
    TouchingDirections touchingDirections;
    public float jumpImpulse = 30f;
    public float wallImpulse = 15f;
    private float jumpCounter;
    private bool doNotTransformOnce;
    public float jumpImpulseCounter = 1f;
    private bool _isJumping = false;
    private bool _isFalling = false;
    private bool _isAttacking = false;
    private bool _jumpAttempt;
    private float canJumpCounter;
    private float defaultGravityScale;
    Transform enemy;
    public LayerMask excludeLayers;
    SpriteRenderer spriteRenderer;
    Vector2 respawnpoint;
    private bool _isRespawning;
    [SerializeField] private GameObject BloaterCollider;
    public bool IsRespawning
    {
        get
        {
            return _isRespawning;
        }
        set
        {
            _isRespawning = value;
        }
    }
    public bool JumpAttempt
    {
        get
        {
            return _isJumping;
        }
        set
        {
            _isJumping = value;
        }
    }
    public bool IsAttacking
    {
        get
        {
            return _isAttacking;
        }
        set
        {
            _isAttacking = value;
        }
    }
    public bool IsFalling
    {
        get
        {
            return _isFalling;
        }
        set
        {
            _isFalling = value;
        }
    }

    public bool IsMoving
    {
        get
        {
            return _isMoving;
        }
        private set
        {
            _isMoving = value;
            // animator.SetBool(AnimationString.isMoving, value);//good practice to store strings such as "isMoving" and "isRunning" in a seperate script
        }
    }
    public bool IsJumping
    {
        get
        {
            return _isJumping;
        }
        private set
        {
            _isJumping = value;
            // animator.SetBool(AnimationString.isMoving, value);//good practice to store strings such as "isMoving" and "isRunning" in a seperate script
        }
    }
    [SerializeField]
    private bool _isRunning = false;

    private bool _isWallJumpingLeft = false;
    private float wallJumpTimer = 0f;
    public bool IsWallJumpingLeft
    {
        get
        {
            return _isWallJumpingLeft;
        }
        set
        {
            _isWallJumpingLeft = value;
        }
    }
    private bool _isWallJumpingRight = false;
    public bool IsWallJumpingRight
    {
        get
        {
            return _isWallJumpingRight;
        }
        set
        {
            _isWallJumpingRight = value;
        }
    }
    public bool IsRunning
    {
        get
        {
            return _isRunning;
        }
        set
        {
            _isRunning = value;
            // animator.SetBool(AnimationString.isRunning, value);
        }

    }
    private bool _isShadowDashing = false;
    private bool _isMoving = false;

    public float MoveSpeed
    {
        get
        {
            if (IsAlive)
            {
                if (wallJumpTimer > 0f && wallJumpTimer < 0.2f)//the end range can be changed occordingly
                {
                    if (IsWallJumpingLeft)//i might need to make it so this happens for a few frames so Ill set a bool in a large if statement and if thats false, no movement can be done.
                    {
                        //IsFacingRight = true;
                        return jumpLeft;

                    }
                    else if (IsWallJumpingRight)
                    {
                        //IsFacingRight = false;
                        return jumpRight;
                    }
                }

                else if (IsMoving && !IsRespawning&&IsRunning && !IsWallJumpingRight && !IsWallJumpingLeft)
                {
                    _wallJumping = false;
                    animator.SetBool("IsMoving", true);//Needs to be IsRunning later
                    return runSpeed;


                }

                else if (IsMoving && IsWallJumpingRight && !Input.GetKey(KeyCode.A))
                {
                    //IsFacingRight = true;
                    return walkSpeed;
                }
                else if (IsMoving && (IsWallJumpingLeft) && !Input.GetKey(KeyCode.D))
                {
                    //IsFacingRight = false;
                    return -walkSpeed;
                }
                else if (IsMoving && IsWallJumpingRight && Input.GetKey(KeyCode.A))
                {
                    //IsFacingRight = false;
                    return -walkSpeed;
                }
                else if (IsMoving && IsWallJumpingLeft && Input.GetKey(KeyCode.D))
                {
                    //IsFacingRight = true;
                    return walkSpeed;
                }


                else if (IsMoving)
                {
                    _wallJumping = false;
                    animator.SetBool("IsMoving", true);
                    return walkSpeed;
                }
            }
            animator.SetBool("IsMoving", false);
            return 0;

        }
    }
    float original;
    private bool _isAlive = true;
    private bool _wallJumping = false;

    public bool IsAlive
    {
        get
        {
            return _isAlive;
        }
        set
        {
            _isAlive = value;
        }
    }

    public bool IsShadowDashing
    {
        get
        {
            return _isShadowDashing;
        }
        set
        {
            _isShadowDashing = value;
            //animator.SetBool(AnimationString.shadowDash, value);
        }

    }

    public bool _isFacingRight = true;
    private bool _startHeavyTimer;
    private float _heavyTimer;
    private bool _menuOpen;
    [SerializeField]private LayerMask allLayers;
    [SerializeField]private float fallSpeed;

    public bool MenuOpen
    {
        get
        {
            return _menuOpen;
        }
        set
        {
            _menuOpen = value;
        }
    }

    public bool IsFacingRight
    {
        get { return _isFacingRight; }
        private set
        {
            if (_isFacingRight != value)
            {
                //Flip local scale 
                if (touchingDirections.IsGrounded)
                {
                    CreateDust();
                }
                transform.localScale *= new Vector2(-1, 1);

            }
            _isFacingRight = value;
        }
    }
    public void SetRespawnLocation()
    {
        respawnpoint = new Vector2(-49, -1.5f);
    }
    private void Update()
    {
        original = Time.timeScale;
        if (IsWallJumpingLeft || IsWallJumpingRight)
        {
            _wallJumping = true;

        }
        if (_wallJumping == true)
        {
            wallJumpTimer += Time.deltaTime;
        }
        else
        {
            wallJumpTimer = 0f;
        }
        if(_startHeavyTimer)
        {
            _heavyTimer += Time.deltaTime;
        }
        if(_heavyTimer>0.5)
        {
            _heavyTimer = 0f;
            _startHeavyTimer = false;
        }
          Debug.Log("Timer: "+_heavyTimer); 
        
        

    }
    private void Awake()//Awake loads faster than start
    {
        Instance = this;
        rb = GetComponent<Rigidbody2D>();//references the RB on player
        animator = GetComponent<Animator>();//references animator
        touchingDirections = GetComponent<TouchingDirections>();
        attack = GetComponentInChildren<Attack>();
        downAttack = GetComponentInChildren<DownAttack>();
        defaultGravityScale = rb.gravityScale;
        spriteRenderer = GetComponent<SpriteRenderer>();
        cameraShaker = FindObjectOfType<CameraShaker>();
        //Cursor.visible = false;
    }
    IEnumerator Respawn(float waitTime)
    {
        spriteRenderer.enabled = true;
        IsRespawning = true;
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        rb.constraints = RigidbodyConstraints2D.FreezePositionX;
        Physics2D.IgnoreLayerCollision(8, 7, true);
        yield return new WaitForSeconds(waitTime);
        Instantiate(walker, new Vector2(-96, -5), Quaternion.identity);
        Instantiate(spitter, new Vector2(-114, 6), Quaternion.identity);
        Instantiate(walker, new Vector2(-157, 2), Quaternion.identity);
        Damageable.Instance.Health += Damageable.Instance.MaxHealth;//make max health if that ever increases.
        transform.position = new Vector2(-170, 5);
        animator.SetBool("IsAlive", true);
        Physics2D.IgnoreLayerCollision(8, 7, false);
        rb.velocity = Vector2.zero;
        spriteRenderer.enabled = true;
        
        IsRespawning = false;
        transform.rotation = Quaternion.identity;
        rb.constraints = RigidbodyConstraints2D.None;
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;



    }
    private void FixedUpdate()
    {
        
        if (IsRespawning)
        {
            rb.velocity = Vector2.zero;
        }
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        if (Damageable.Instance.Health <= 0)
        {
            rb.constraints = RigidbodyConstraints2D.FreezeRotation;
            rb.constraints = RigidbodyConstraints2D.FreezePositionX;

        }
        if (Damageable.Instance.Health <= 0 && !IsRespawning)
        {
            StartCoroutine(Respawn(1.5f));
            //spriteRenderer.enabled = false;
            rb.velocity = Vector2.zero;
            return;
        }
        //Debug.Log(touchingDirections.IsOnEnemy);
        if (!touchingDirections.IsGrounded && !touchingDirections.IsOnWallLeft && !touchingDirections.IsOnWallRight)
        {
            cayoteTimer += Time.fixedDeltaTime;
        }
        else if (touchingDirections.IsGrounded || touchingDirections.IsOnWallLeft || touchingDirections.IsOnWallRight)
        {
            cayoteTimer = 0f;
        }

        if (IsMoving && !IsWallJumpingLeft && !IsWallJumpingRight)
        {
            rb.velocity = new Vector2(moveInput.x * MoveSpeed, rb.velocity.y);

        }
        else if ((IsWallJumpingRight && IsMoving) || (IsWallJumpingLeft && IsMoving))
        {
            rb.velocity = new Vector2(MoveSpeed, rb.velocity.y);

        }
        else if(!IsRespawning)
        {
            rb.velocity = new Vector2(MoveSpeed, rb.velocity.y);

        }
        SetFacingDirection(new Vector2(rb.velocity.x, rb.velocity.y));
        if (rb.velocity.y < 0 && !touchingDirections.IsGrounded && (touchingDirections.IsOnWallLeft))
        {
            rb.gravityScale = 2.5f;
            rb.velocity = new Vector2(rb.velocity.x, -5);
            IsFacingRight = false;
            animator.SetBool("IsWallSliding", true); //this needs to be an if 
        }
        else if (rb.velocity.y < 0 && !touchingDirections.IsGrounded && (touchingDirections.IsOnWallRight))
        {
            rb.gravityScale = 2.5f;
            rb.velocity = new Vector2(rb.velocity.x, -5);
            IsFacingRight = true;

            animator.SetBool("IsWallSliding", true); //this needs to be an if 
        }
        else if ((rb.velocity.y < 0))//this needs to be else if
        {
            rb.gravityScale = fallSpeed;
            animator.SetBool("IsWallSliding", false);
        }
        /* else if(IsFalling)
         {

             IsFalling = false;
             rb.gravityScale = 1.5f;
             animator.SetBool("IsWallSliding", false);
             CreateDust();
         }*/
        else
        {
            rb.gravityScale = 1.5f;
            animator.SetBool("IsWallSliding", false);
        }
        if (IsJumping && !IsWallJumpingRight && !IsWallJumpingLeft && jumpImpulseCounter < maxJumpHoldTime)
        {
            jumpImpulseCounter += Time.fixedDeltaTime;


            rb.AddForce(new Vector2(0, jumpImpulse * jumpImpulseCounter / 10), ForceMode2D.Impulse);
        }
        if (touchingDirections.IsGrounded)
        {
            animator.SetBool("IsGrounded", true);

        }
        else
        {
            animator.SetBool("IsGrounded", false);
            IsFalling = true;

        }
        if (touchingDirections.IsOnWall)
        {
            animator.SetBool("IsOnWall", true);
        }
        else
        {
            animator.SetBool("IsOnWall", false);
        }
        if (attack.AttackSucess)
        {
            CreateDust();
            //Debug.Log("AttackSucess");
            if (IsFacingRight)
            {
                Debug.Log("HHHH");
                rb.AddForce(new Vector2(-4, 0), ForceMode2D.Impulse);
            }
            else
            {
                Debug.Log("AAAA");
                rb.AddForce(new Vector2(4, 0), ForceMode2D.Impulse);
            }
            attack.AttackSucess = false;
            //Attack.Instance.AttackSucess = false;
        }
        if (downAttack.DownAttackSucess)
        {
            //Debug.Log("HEllo");
            rb.velocity = new Vector2(rb.velocity.x, 0);
            rb.AddForce(new Vector2(0, 10), ForceMode2D.Impulse);
            //Instantiate(blood, new Vector2(), Quaternion.identity);
            downAttack.DownAttackSucess = false;

            rb.gravityScale = 0;
        }
        if (JumpAttempt && !touchingDirections.IsGrounded)
        {
            canJumpCounter += Time.fixedDeltaTime;
        }
        else if (!JumpAttempt)
        {
            canJumpCounter = 0;
        }
        if (touchingDirections.IsOnEnemy || (forceTimer < 0.2f && forceTimer > 0))
        {
           enemy= GameObject.Find(touchingDirections.CollidedEnemyName).transform;
            //enemy = GameObject.FindGameObjectWithTag("Enemy").transform;
            Debug.Log("AAA: "+enemy.name);
            forceTimer += Time.fixedDeltaTime;

            //Debug.Log(Damageable.Instance.Health);
            //CameraShaker.Presets.ShortShake2D(forceTimer);
            if (dustCounter == 0)
            {
                // Debug.Log("DustCreated");
                CreateDust();
                Instantiate(blood, transform.position, Quaternion.identity);
                CinemachineShake.Instance.ShakeCamera(5f, 2f);
                Damageable.Instance.Hit(50);
                dustCounter++;
            }
            if (enemy.position.x < transform.position.x)
            {
                Debug.Log("right");
                rb.AddForce(new Vector2(600, 10), ForceMode2D.Force);
                //Damageable.Instance.Hit(50); 
            }
            else
            {
                Debug.Log("left");
                rb.AddForce(new Vector2(-600, 10), ForceMode2D.Force);
            }

            touchingDirections.IsOnEnemy = false;
        }
        if (forceTimer > 0.2)
        {
            forceTimer = 0;
            dustCounter = 0;
        }
    }

    private void SetFacingDirection(Vector2 moveInput)
    {

        if ((moveInput.x > 0 && !touchingDirections.IsOnWallRight)) {

            IsFacingRight = true;
        }

        else if ((moveInput.x < 0 && !touchingDirections.IsOnWallLeft))
        {
            IsFacingRight = false;
        }
    }
    public void OnMove(InputAction.CallbackContext context)
    {
        if (!Damageable.Instance.IsAlive && IsRespawning || MenuOpen)
        {
            IsMoving = false;
            moveInput = Vector2.zero;
            return;
        }
        IsRespawning = false;
        //moveInput = UserInputs.Instance.MoveInput;
        moveInput = context.ReadValue<Vector2>();
        IsMoving = moveInput != Vector2.zero;

        SetFacingDirection(moveInput);

    }
    public void OnJump(InputAction.CallbackContext context)
    {
       
        if (!Damageable.Instance.IsAlive || _menuOpen)
        {
            return;
        }
        if (context.started && touchingDirections.IsOnWallRight && !touchingDirections.IsGrounded && !IsWallJumpingLeft && !IsWallJumpingRight)
        {
            CreateDust();
            animator.SetBool("IsJumping", true);
            IsJumping = true;
            IsWallJumpingRight = true;
            counterA = 0;
            counterB++;
            rb.velocity = new Vector2(rb.velocity.x, wallImpulse);

        }
        else if (context.started && touchingDirections.IsOnWallLeft && !touchingDirections.IsGrounded && !IsWallJumpingLeft && !IsWallJumpingRight)// I think i should add a timer where while its active it overrides any other movement so that i can impulse off the wall and not have to worry about other velocities making the game finnicky.
        {
            CreateDust();
            animator.SetBool("IsJumping", true);
            IsJumping = true;
            IsWallJumpingLeft = true;
            counterA = 0;
            rb.velocity = new Vector2(rb.velocity.x, wallImpulse);

        }

        else if ((context.started) && (touchingDirections.IsGrounded ))
        {
            
            CreateDust();
            IsJumping = true;
            jumpImpulseCounter = 1f;
            animator.SetBool("IsJumping", true);
        }
        else if (context.canceled && (IsWallJumpingLeft || IsWallJumpingRight))
        {
            cayoteTimer = 0;
            animator.SetBool("IsJumping", false);
            IsWallJumpingLeft = false;
            IsWallJumpingRight = false;
            IsJumping = false;
            jumpImpulseCounter = 1f;
            counterB = 0;
            JumpAttempt = false;
        }
        else if (context.canceled)
        {
            cayoteTimer = 0;
            animator.SetBool("IsJumping", false);
            IsWallJumpingLeft = false;
            IsWallJumpingRight = false;
            if(IsJumping)
            {
                rb.velocity = new Vector2(rb.velocity.x, 0);
            }
            IsJumping = false;
            jumpImpulseCounter = 1f;
            counterB = 0;
            JumpAttempt = false;
            
           
        }

    }
    
    public void OnDownAttack(InputAction.CallbackContext context)
    {
        if (!Damageable.Instance.IsAlive || MenuOpen)
        {
            return;
        }
        if (context.started && !touchingDirections.IsGrounded)
        {
            animator.SetTrigger("DownAttack");
        }

    }
    public void OnUpAttack(InputAction.CallbackContext context)
    {
        if (!Damageable.Instance.IsAlive || MenuOpen)
        {
            return;
        }
        if (context.started)
        {
            //Debug.Log("UpAttack");
            animator.SetTrigger("UpAttack");
        }
    }
    public void OnThrow(InputAction.CallbackContext context)
    {
        if (!Damageable.Instance.IsAlive || MenuOpen)
        {
            return;
        }
        if (context.started)
        {
            Debug.Log("Hellllllo");
            if(IsFacingRight)
            {
                Instantiate(selectedProjectile, new Vector2(transform.position.x + 1, transform.position.y), Quaternion.identity);
            }
            else
            {
                Instantiate(selectedProjectile, new Vector2(transform.position.x-1,transform.position.y), Quaternion.identity);

            }

        }

    }
    public void OnRun(InputAction.CallbackContext context)
    {
        
        if (!Damageable.Instance.IsAlive || MenuOpen)
        {
            return;
        }
        if (context.started && touchingDirections.IsGrounded)
        {
            IsRunning = true;
        }
        else if (context.canceled)
        {
            IsRunning = false;
        }
    }
    public void OnAttack(InputAction.CallbackContext context)
    {
        if (!Damageable.Instance.IsAlive || MenuOpen)
        {
            return;
        }
        //maybe make a timer where you can't attack within a time frame.
        if ((context.started && !Input.GetKey(KeyCode.S) && !touchingDirections.IsGrounded && !Input.GetKey(KeyCode.W) ) || (!Input.GetKey(KeyCode.W) && context.started && touchingDirections.IsGrounded))
        {
            animator.SetTrigger("Attack");
        }




    }
    public void OnHeavyAttack(InputAction.CallbackContext context)
    {
        if (!Damageable.Instance.IsAlive || MenuOpen)
        { return; }
        if(context.started&&(_heavyTimer==0||_heavyTimer>0.5f))
        {
            animator.SetTrigger("HeavyAttack");
            _startHeavyTimer = true;
        }
        else if (context.started && _heavyTimer < 0.5f&&_heavyTimer>0)
        {
            animator.SetTrigger("HeavyAttackTwo");
            _startHeavyTimer = false;
            _heavyTimer = 0;
        }
        else
        {
            //_startHeavyTimer = false;
            //_heavyTimer = 0;
        }
    }
    public void CreateDust()
    {
        dust.Play();
    }
    public void CreateBlood()
    {
        blood.Play();
    }
    public void OnMenu(InputAction.CallbackContext context)
    {
        
        if (context.started&&!MenuOpen)
        {
            MenuOpen = true;
            Time.timeScale = 0;
        }
        else if(context.started&& MenuOpen)
        {
            Debug.Log("hello");
            MenuOpen = false;
            Time.timeScale = 1;
        }
    }
   
    public void OnChannel(InputAction.CallbackContext context)//main game mechanic
    {
       
        if (!Damageable.Instance.IsAlive|| MenuOpen)
        {
            return;
        }
        if (context.started && touchingDirections.IsGrounded && (rb.velocity.x<1&&rb.velocity.x>-1))
        {
            animator.SetBool("IsChannel", true);
            //Cursor.visible = true;
            
        }
        else if(context.canceled||!touchingDirections.IsGrounded|| rb.velocity.x > 1 || rb.velocity.x < -1)//later add conditions for if hit, the player stops channeling.
        {
            animator.SetBool("IsChannel", false);
            //Cursor.visible=false;
            //virtualCamera.Follow = transform;
        }
    }
   
}
