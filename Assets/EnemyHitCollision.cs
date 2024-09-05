using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class EnemyHitCollision : MonoBehaviour
{
    ParticleSystem dust;
    PlayerController controller;
    public float timer;
    private bool _attackSucess;
    public ParticleSystem blood;
    public Transform player;
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
    public static EnemyHitCollision Instance { get; set; }


    private void Awake()
    {
        Instance = this;
        attackCollider = GetComponent<Collider2D>();

        controller = GetComponentInParent<PlayerController>();
        player= GameObject.FindGameObjectWithTag("Player").transform;
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {

        //subtract health from enemy 
        Damageable damageable = collision.GetComponent<Damageable>();

        if (damageable == null)
        {
            //EnemyTag = "";
        }
        if (damageable != null)
        {
            
            CinemachineShake.Instance.ShakeCamera(1.5f, 0.3f);
            blood.transform.position = new Vector2(player.position.x,player.position.y+1f);
            blood.Play();
            Damageable.Instance.Hit(20);
            Debug.Log("Health Player: " + Damageable.Instance.Health);
            AttackSucess = true;
            //Rigidbody2D enemyRb = damageable.GetComponentInParent<Rigidbody2D>();

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
