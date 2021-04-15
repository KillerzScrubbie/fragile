using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class View : MonoBehaviour
{
    public Transform upper;
    public Transform center;

    public void Show()
    {
        transform.DOMoveY(center.position.y, 1);
    }

    public void Hide()
    {
        transform.DOMoveY(upper.position.y, 1);
    }
}
