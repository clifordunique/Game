using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleUIOpener : MonoBehaviour
{
    private void Awake()
    {
        MenuManager.Instance.m_CanvasRoot = gameObject;
        MenuManager.Instance.ShowPanel("TitleMainMenu");
    }

    
}
