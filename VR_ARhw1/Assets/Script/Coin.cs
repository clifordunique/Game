using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    public GameObject sys;

    private void Start()
    {
        sys = GameObject.FindGameObjectWithTag("GameController");
    }

    private void OnTriggerEnter(Collider other)
    {
        SystemManager script = sys.GetComponent<SystemManager>();
        if (other.tag == "Player")
        {
            script.point++;
            Destroy(gameObject);
        }
        else
        {
            Destroy(gameObject.GetComponentInParent<Transform>().gameObject);
        }
        script.coin--;
    }
}
