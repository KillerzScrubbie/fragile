using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    [SerializeField] private GameObject loginUI;
    [SerializeField] private GameObject registerUI;
    [SerializeField] private GameObject menuUI;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        } else if (instance != null)
        {
            Debug.Log("Instance already exists, destroying object");
            Destroy(this);
        }
    }

    public void ClearScreen()
    {
        loginUI.SetActive(false);
        registerUI.SetActive(false);
        menuUI.SetActive(false);
    }

    public void LoginScreen()
    {
        ClearScreen();
        loginUI.SetActive(true);
    }

    public void RegisterScreen()
    {
        ClearScreen();
        registerUI.SetActive(true);
    }

    public void MenuScreen()
    {
        ClearScreen();
        menuUI.SetActive(true);
    }
}
