using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyGameObject : MonoBehaviour
{
    public float timeOfDestruction;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(!Damageable.Instance.IsAlive)
        {
            Destroy(gameObject);
        }
        Destroy(gameObject, timeOfDestruction);
    }
}
