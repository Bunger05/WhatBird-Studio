using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDamageable : MonoBehaviour
{
    // Start is called before the first frame update
    Rigidbody2D rb;
    PlayerController controller;
   public static EnemyDamageable Instance { get;  set; }
    private SpriteRenderer spriteRenderer;
    [SerializeField] private Material tempMaterial;
    private Material originalMaterial;
    private Coroutine flashRoutine;
    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        originalMaterial =spriteRenderer.material;
        rb = GetComponent<Rigidbody2D>();
        controller =GetComponent<PlayerController>();
    }
    [SerializeField]private int _health = 100;
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
            if(!Damageable.Instance.IsAlive)
            {
                return false;
            }
            return Health > 0;
        }
    }
   

    // Update is called once per frame
    
    public void Hit(int damage)
    {
        if (IsAlive)
        {
           
            Health -= damage;
            if (flashRoutine != null)
            {
                // In this case, we should stop it first.
                // Multiple FlashRoutines the same time would cause bugs.
                StopCoroutine(flashRoutine);
            }

            // Start the Coroutine, and store the reference for it.
            flashRoutine = StartCoroutine(FlashRoutine());
            //Debug.Log($"{gameObject.name} hit! Remaining health: {Health}");


        }
    }
    private IEnumerator FlashRoutine()
    {
        // Swap to the flashMaterial.
        spriteRenderer.material = tempMaterial;

        // Pause the execution of this function for "duration" seconds.
        yield return new WaitForSeconds(0.15f);

        // After the pause, swap back to the original material.
        spriteRenderer.material = originalMaterial;

        // Set the routine to null, signaling that it's finished.
        flashRoutine = null;
    }
}
