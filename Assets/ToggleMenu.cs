using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleMenu : MonoBehaviour
{
    public GameObject menuUI;
    private void Start()
    {
        menuUI.SetActive(false);
    }
    private void Update()
    {
        //menuUI.SetActive(true);
        if (PlayerController.Instance.MenuOpen)
        {
            menuUI.SetActive(true);
        }
        else { menuUI.SetActive(false); }
    }
}
