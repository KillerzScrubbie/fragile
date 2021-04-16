using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class View : MonoBehaviour
{
    public static View instance;

    [SerializeField] private Transform upper;
    [SerializeField] private Transform center;
    [SerializeField] private Transform lower;

    [SerializeField] private GameObject loginScreen;
    [SerializeField] private GameObject registerScreen;
    [SerializeField] private GameObject optionsScreen;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != null)
        {
            Debug.Log("Instance already exists, destroying object");
            Destroy(this);
        }
    }

    public void Show(GameObject gameObject)
    {
        gameObject.transform.DOMoveY(center.position.y, 1);
    }

    public void Hide(GameObject gameObject)
    {
        gameObject.transform.DOMoveY(upper.position.y, 1);
    }

    public void HideUnder(GameObject gameObject)
    {
        gameObject.transform.DOMoveY(lower.position.y, 1);
    }

    public void OnPressOptionsButton()
    {
        Show(optionsScreen);
        Hide(registerScreen);
        Hide(loginScreen);
    }

    public void OnPressLoginButton()
    {
        Show(loginScreen);
        Hide(registerScreen);
        HideUnder(optionsScreen);
    }

    public void OnPressCloseOptionsButton()
    {
        HideUnder(optionsScreen);
    }

    public void OnPressCloseLoginButton()
    {
        Hide(loginScreen);
    }

    public void OnPressRegisterButton()
    {
        HideUnder(loginScreen);
        Show(registerScreen);
    }

    public void OnPressCloseRegisterButton()
    {
        Hide(registerScreen);
        Show(loginScreen);
    }

    public void OnSuccessfulLogin()
    {
        OnPressCloseLoginButton();
    }

    public void OnSuccessfulRegister()
    {
        OnPressCloseRegisterButton();
    }
}
