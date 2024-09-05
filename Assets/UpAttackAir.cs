using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpAttackAir : MonoBehaviour
{
    ParticleSystem dust;
    PlayerController controller;
    public float timer;
    private bool _attackSucess;
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
    private string _enemyTag;
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
    public static UpAttackAir Instance { get; set; }


    private void Awake()
    {
        Instance = this;
        attackCollider = GetComponent<Collider2D>();

        controller = GetComponentInParent<PlayerController>();
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {

        //subtract health from enemy 
        EnemyDamageable damageable = collision.GetComponent<EnemyDamageable>();

        if (damageable == null)
        {
            //EnemyTag = "";
        }
        if (damageable != null)
        {
            EnemyTag = damageable.gameObject.name;
            //Debug.Log(EnemyTag+"HELELALL");
            timer = 0;
            CinemachineShake.Instance.ShakeCamera(1.5f, 0.3f);
            damageable.Hit(10);
            AttackSucess = true;
            Rigidbody2D enemyRb = damageable.GetComponentInParent<Rigidbody2D>();

        }
        EnemyTwoDamageable damageable2 = collision.GetComponent<EnemyTwoDamageable>();

        if (damageable2 == null)
        {
            //EnemyTag = "";
        }
        if (damageable2 != null)
        {
            EnemyTag = damageable2.gameObject.name;
            //Debug.Log(EnemyTag+"HELELALL");
            timer = 0;
            CinemachineShake.Instance.ShakeCamera(1.5f, 0.3f);
            damageable2.Hit(10);
            AttackSucess = true;
            Rigidbody2D enemyRb = damageable2.GetComponentInParent<Rigidbody2D>();

        }
        SpitterDamageable damageable3 = collision.GetComponent<SpitterDamageable>();

        if (damageable3 == null)
        {
            //EnemyTag = "";
        }
        if (damageable3 != null)
        {
            EnemyTag = damageable3.gameObject.name;
            //Debug.Log(EnemyTag+"HELELALL");
            timer = 0;
            CinemachineShake.Instance.ShakeCamera(1.5f, 0.3f);
            damageable3.Hit(25);
            AttackSucess = true;
            //Rigidbody2D enemyRb = damageable2.GetComponentInParent<Rigidbody2D>();

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
