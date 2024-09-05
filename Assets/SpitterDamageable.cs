using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpitterDamageable : MonoBehaviour
{
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
            return Health > 0;
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

