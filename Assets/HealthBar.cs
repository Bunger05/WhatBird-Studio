using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Slider slider;
    private float smoothSpeed = 5f;
    private void Awake()
    {
        slider.maxValue = Damageable.Instance.MaxHealth;
        slider.value = slider.maxValue;
    }
    public void SetMaxHealth()
    {
        slider.maxValue = Damageable.Instance.MaxHealth;
        
    }
    public void SetHealth()
    {
        
        slider.value = Damageable.Instance.Health;
    }
    private void Update()
    {
        slider.value = Mathf.Lerp(slider.value, Damageable.Instance.Health, Time.deltaTime * smoothSpeed);
    }
}
