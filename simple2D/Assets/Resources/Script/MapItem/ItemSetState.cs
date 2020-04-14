using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSetState : MonoBehaviour
{
    //bool state;

    
    void SetState(bool target)
    {
        if (!target)
        {
            transform.GetChild(0).localScale = Vector2.one;
            transform.GetChild(1).localScale = Vector2.zero;
        }
        else
        {
            transform.GetChild(0).localScale = Vector2.zero;
            transform.GetChild(1).localScale = Vector2.one;
        }
        //state = target;
    }
    void Start()
    {
        Player.Instance.Change += SetState;
        SetState(Player.Instance.colorState);
    }
    
    // Update is called once per frame
    void Update()
    {
        
    }
}
