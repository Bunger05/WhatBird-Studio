using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTwoDamageable : MonoBehaviour
{
    // Start is called before the first frame update
    Rigidbody2D rb;
    PlayerController controller;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        controller = GetComponent<PlayerController>();
    }
    private int _health = 100;
    public int Health
    {
        get
        {
            return _health;
        }
        set
        {
            _health = value;
        }
    }
    public bool IsAlive
    {
        get
        {
           
           if(Health>0)
            {
                return true;
            }
            else
            {
                return false;
            }
           
        }
    }


    // Update is called once per frame

    public void Hit(int damage)
    {
        if (IsAlive)
        {

            Health -= damage;
            //Debug.Log($"{gameObject.name} hit! Remaining health: {Health}");


        }
    }
}
