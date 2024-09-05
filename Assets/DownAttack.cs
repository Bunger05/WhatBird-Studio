using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DownAttack : MonoBehaviour
{
    public ParticleSystem blood;
    GameObject gameobject;
    public float timer;
    private bool _downAttackSucess;
    public bool DownAttackSucess
    {
        get
        {
            return _downAttackSucess;
        }
        set
        {
            _downAttackSucess = value;
        }
    }
    Collider2D attackCollider;
    Rigidbody2D playerRb;



    private void Awake()
    {
        attackCollider = GetComponent<Collider2D>();
        
        
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        //subtract health from enemy 
        EnemyDamageable damageable = collision.GetComponent<EnemyDamageable>();

        if (damageable != null)
        {
            timer = 0;
            damageable.Hit(10);
            CinemachineShake.Instance.ShakeCamera(1.5f, 0.3f);
            DownAttackSucess = true;
            Instantiate(blood, new Vector2(damageable.transform.position.x, damageable.transform.position.y + 0.3f), Quaternion.identity);



        }
       


    }

    private void OnTriggerExit2D(Collider2D collision)
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
