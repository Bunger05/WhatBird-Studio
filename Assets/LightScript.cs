using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering.Universal;

public class LightScript : MonoBehaviour
{
    public CinemachineVirtualCamera virtualCamera;
   
    public GameObject lightPrefab; // Reference to the light prefab
    [SerializeField] public int maxLights = 10; // Maximum number of lights to spawn
    int counter = 0;
    private Queue<GameObject> lightsQueue = new Queue<GameObject>();
    //Animator animator;
    GameObject otherObject; 
    Animator animator;
    private bool _canSpawn;
    Transform playerTransform;
    public float minSize, maxSize, zoomStep, cameraDistance;
    private void Awake()
    {
        otherObject = GameObject.Find("Player");
        animator = otherObject.GetComponent<Animator>();
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Start()
    {
        
    }

    void Update()
    {
        //Debug.Log(Mathf.Abs(playerTransform.position.x - Input.mousePosition.x) / 100);


        // Convert screen space position to world space
        //worldPos = .ScreenToWorldPoint(Mouse.current.position);
       Vector2 mousePosition = Mouse.current.position.ReadValue();
        Vector3 mousePos = Mouse.current.position.ReadValue();
        Vector3 screenPosition = Camera.main.WorldToScreenPoint(playerTransform.position);
        //Vector3 worldPoint = Camera.main.ScreenToWorldPoint(mousePos);
        //Debug.Log("X: " + worldPoint.x + "y: " + worldPoint.y + "z: " + worldPoint.z)
        // Get the X position in world space

        Debug.Log(lightsQueue.Count);
        if (animator.GetBool("IsChannel"))
        {
            SpawnLightAtMousePosition();

            virtualCamera.m_Lens.OrthographicSize = Mathf.Max(10f, Mathf.Abs(screenPosition.x - mousePosition.x) / 50);
        }
        else
        {
            virtualCamera.m_Lens.OrthographicSize = 10f;
        }
        




    }
    /*void SpawnLightAtMousePosition()
    {
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        

        // Check if there's any light in the queue
        if (lightsQueue.Count > 0)
        {
            bool validPosition = false;
            Vector2 newPosition = mousePosition;

            // Ensure the new light is at least 3 units away from all existing lights
            while (!validPosition)
            {
                validPosition = true;

                foreach (GameObject existingLight in lightsQueue)
                {
                    Vector2 existingPosition = existingLight.transform.position;

                    if (Vector2.Distance(newPosition, existingPosition) < 3f)
                    {
                        validPosition = false;
                        // If too close, adjust position (e.g., move the new position by a small offset)
                        newPosition = existingPosition + (newPosition - existingPosition).normalized * 3f;
                        break;
                    }
                }
            }

            // Instantiate the light at the valid position
            GameObject newLight = Instantiate(lightPrefab, newPosition, Quaternion.identity);
            Light2D light2D = newLight.GetComponent<Light2D>();
            light2D.intensity = 0f; // Start at 0 intensity

            StartCoroutine(LerpLightIntensity(light2D, 1f, 1f));
            // Add the new light to the queue
            lightsQueue.Enqueue(newLight);

            // If the number of lights exceeds the maximum, destroy the oldest one
            if (lightsQueue.Count > maxLights)
            {
                GameObject oldLight = lightsQueue.Dequeue();
                Destroy(oldLight);
            }
        }
        else
        {
            // If no lights are in the queue, instantiate at the mouse position
            GameObject newLight = Instantiate(lightPrefab, mousePosition, Quaternion.identity);
            Light2D light2D = newLight.GetComponent<Light2D>();
            light2D.intensity = 0f; // Start at 0 intensity

            StartCoroutine(LerpLightIntensity(light2D, 1f, 1f));
            // Add the new light to the queue
            lightsQueue.Enqueue(newLight);
        }
    }*/
        void SpawnLightAtMousePosition()
         {
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        if (mousePosition.y - playerTransform.position.y > 5)
        {
            mousePosition = new Vector2(mousePosition.x, playerTransform.position.y + 5);
        }
        else if(playerTransform.position.y-mousePosition.y>5)
        {
            mousePosition = new Vector2(mousePosition.x, playerTransform.position.y -5);
        }
        // Only spawn a new light if there are no lights in the queue or if it is far enough from existing lights
        bool canSpawn = true;

        foreach (GameObject light in lightsQueue)
        {
            Vector2 lightPos = light.transform.position;
           
            // Check if the distance between the mouse position and the light is less than 5 units
            if (Vector2.Distance(lightPos, mousePosition) < 3)
            {
                canSpawn = false;
                break; // If a close light is found, no need to check further
            }
        }

        // Instantiate the light at the mouse position if the condition is met
        if (canSpawn)
        {
            Debug.Log("helllll");
            GameObject newLight = Instantiate(lightPrefab, mousePosition, Quaternion.identity);
            Light2D light2D = newLight.GetComponent<Light2D>();
            light2D.intensity = 0f; // Start at 0 intensity

            StartCoroutine(LerpLightIntensity(light2D, 0.7f, 5f));

            // Add the new light to the queue
            lightsQueue.Enqueue(newLight);
        }

        // If the number of lights exceeds the maximum, destroy the oldest one
        if (lightsQueue.Count > maxLights)
        {
            GameObject oldLight = lightsQueue.Dequeue();
            Destroy(oldLight);
        }
    }
    
    private IEnumerator LerpLightIntensity(Light2D light2D, float targetIntensity, float duration)
    { 
    
        float startIntensity = light2D.intensity;
        float timeElapsed = 0f;

        while (timeElapsed < duration)
        {
            light2D.intensity = Mathf.Lerp(startIntensity, targetIntensity, timeElapsed / duration);
            timeElapsed += Time.deltaTime;
            yield return null;
        }

        light2D.intensity = targetIntensity; // Ensure it ends exactly at target intensity
    }

}
