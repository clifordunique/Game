using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        MenuManager.Instance.ShowPanel("subPanel/KeyboardScroll");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
