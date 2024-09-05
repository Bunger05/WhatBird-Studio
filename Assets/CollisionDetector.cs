using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionDetector : MonoBehaviour
{
    public LayerMask targetLayer;
    private bool _canSeePlayer = false;
    private Transform player;
    public LayerMask wallLayer; // Set this to the "Wall" layer in the Inspector
    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }
    public bool CanSeePlayer
    {
        get
        {
            return _canSeePlayer;
        }
        set
        {
            
                _canSeePlayer = value;
            
            
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the other object's layer matches the target layer
        if ((targetLayer.value & (1 << other.gameObject.layer)) != 0)
        {
            CheckLineOfSight();
        }

    }
    void OnTriggerStay2D(Collider2D other)
    {
        // Check if the other object's layer matches the target layer
        if ((targetLayer.value & (1 << other.gameObject.layer)) != 0)
        {
            CheckLineOfSight();
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        // Check if the other object's layer matches the target layer
        if ((targetLayer.value & (1 << other.gameObject.layer)) != 0)
        {
            CanSeePlayer = false;
        }
    }
    private void CheckLineOfSight()
    {
        // Calculate direction to the player
        Vector2 directionToPlayer = (player.position - transform.position).normalized;

        // Calculate distance to the player
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        // Perform the raycast
        RaycastHit2D hit = Physics2D.Raycast(transform.position, directionToPlayer, distanceToPlayer, wallLayer);

        if (hit.collider == null)
        {
            CanSeePlayer = true;
            // No wall in the way, enemy can see the player
            //Debug.Log("Player in sight!");
            // Implement logic for when the player is in sight
        }
        else
        {
            CanSeePlayer = false;
            // Wall in the way, enemy can't see the player
            //Debug.Log("Player out of sight.");
            // Implement logic for when the player is out of sight
        }

        // Debugging visualization
        Debug.DrawRay(transform.position, directionToPlayer * distanceToPlayer, hit.collider == null ? Color.green : Color.red);
    }

}
