using System.Collections;
using System.Collections.Generic;
using Unity.Physics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MouseCollisionCheck : MonoBehaviour
{
    public static MouseCollisionCheck Instance { get; private set; }
    public Collider2D collider;
    private bool _isInCorridor;
    RaycastHit2D hit;

    private void Awake()
    {
        collider = GetComponent<TilemapCollider2D>();
        Instance = this;
    }

    public bool IsInCorridor
    {
        get { return _isInCorridor; }
        set { _isInCorridor = value; }
    }

    void Update()
    {
    
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
      
        //Debug.Log(mousePosition);
        if (collider.OverlapPoint(mousePosition)) {
            Debug.Log("Jeeee");
        }
    }
    private void OnMouseOver()
    {
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        //Debug.Log(mousePosition);
        if (collider.OverlapPoint(mousePosition))
        {
            Debug.Log("Jeeee");
        }

    }
    private void OnMouseEnter()
    {
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        //Debug.Log(mousePosition);
        if (collider.OverlapPoint(mousePosition))
        {
            Debug.Log("Jeeee");
        }
    }
}
