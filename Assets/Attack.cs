using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Attack : MonoBehaviour
{
    //ParticleSystem dust;
    //PlayerController controller;
    public float timer;
    private bool _attackSucess;
    public ParticleSystem blood;
    [SerializeField] private int damage = 10;
    [SerializeField] private float shakeMagnitude = 1.5f;
    public bool AttackSucess
    {
        get
        {
            return _attackSucess;
        }
        set
        {
            _attackSucess = value;
        }
    }
    private string _enemyTag ="";
    public string EnemyTag
    {
        get
        {
            return _enemyTag;
        }
        set
        {
            _enemyTag = value;
        }
    }
        

    Collider2D attackCollider;
    Rigidbody2D playerRb;
    public static Attack Instance { get;  set; }
    

    private void Awake()
    {
        Instance = this;
        attackCollider = GetComponent<Collider2D>();
        
       //controller= GetComponentInParent<PlayerController>();
    }

    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //collision.transform.position
        //subtract health from enemy 
       
       
        EnemyDamageable damageable = collision.GetComponent<EnemyDamageable>();
        
        if (damageable==null)
        {
            //EnemyTag = "";
        }
        if(damageable != null )
        {
            
            EnemyTag=damageable.gameObject.name;
            Debug.Log(EnemyTag);
            //Debug.Log(EnemyTag = damageable.gameObject.name);
            Instantiate(blood, new Vector2(damageable.transform.position.x,damageable.transform.position.y+0.3f),Quaternion.identity);
            //Debug.Log(EnemyTag+"HELELALL");
            timer = 0;
            CinemachineShake.Instance.ShakeCamera(shakeMagnitude, 0.3f);
            damageable.Hit(damage);
            AttackSucess = true;
            Rigidbody2D enemyRb = damageable.GetComponentInParent<Rigidbody2D>();
      
        }
       
        
        
        

    }
    private void FixedUpdate()
    {
        
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
