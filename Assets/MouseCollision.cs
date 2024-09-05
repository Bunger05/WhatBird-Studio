using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseCollision : MonoBehaviour
{
    private Camera mainCamera;
    public Rigidbody2D rb;
    private bool _hit = false;
    private Vector2 pos;
    //Transform colliderTransform;
    public static MouseCollision Instance { get; private set; }  
    void Start()
    {
        Instance = this;
        mainCamera = Camera.main;
        rb = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate() // Use FixedUpdate for physics-related updates
    {
        Vector3 mousePosition = Input.mousePosition;
        mousePosition.z = 0f; // Keep z at 0 for 2D
        Vector3 worldPosition = mainCamera.ScreenToWorldPoint(mousePosition);
       
        if(!_hit)
        {
            rb.MovePosition(worldPosition);
        }
       
    }
   
    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("Hello?");
        _hit = true;
        rb.MovePosition(collision.transform.position);
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        _hit = false;

    }
}
