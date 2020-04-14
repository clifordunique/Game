using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MyIntEvent : UnityEvent<int>
{
}

public class HitableObj : MonoBehaviour
{
    [SerializeField]
    public MyIntEvent getHit;
    private void Awake()
    {
        if (getHit == null)
        {
            getHit = new MyIntEvent();
        }
    }
}
