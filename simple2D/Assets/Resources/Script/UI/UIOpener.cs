/* this script is used to transform the Canvas Path to the MemuManager.cs
 * this script is laod in the Canvas and control the button of the UI;
 * 
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIOpener : MonoBehaviour
{
    private void Awake()
    {
        MenuManager.Instance.m_CanvasRoot = gameObject;
        GameManager.Instance.StartTransition(false, false);
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            //MenuManager.Instance.ShowPanel("MainUI");
            //for exp ui
            MenuManager.Instance.ShowPanel("TutorialUI");
            GameManager.Instance.Pause();
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            MenuManager.Instance.CloseLastPanel();
        }
    }
}
