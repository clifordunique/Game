using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapPlatform : MonoBehaviour
{
    public float RayLenth = 0.5f;
    public float latency = 0.3f;
    public float RaySpacing = 0.25f;
    public bool broken;


    Transform trigger;
    Collider2D Collider;
    LayerMask collisionMask;
    Vector2 startFrom;
    
    float bound;

    void Start()
    {
        trigger = transform.GetChild(0);
        Collider = trigger.GetComponent<Collider2D>();
        collisionMask |= (1 << LayerMask.NameToLayer("Player_w"));
        collisionMask |= (1 << LayerMask.NameToLayer("Player_b"));
        bound = Collider.bounds.max.x;
    }

    IEnumerator triggered()
    {
        yield return new WaitForSeconds(latency);
        transform.localScale = Vector3.zero;
    }

    // Update is called once per frame
    void Update()
    {
        if (broken) return;
        startFrom = new Vector2(Collider.bounds.min.x, Collider.bounds.max.y);
        for (int i = 0;startFrom.x < bound; i++)
        {
            RaycastHit2D hit;
            Debug.DrawRay(startFrom, Vector2.up * RayLenth, Color.green);
            if (hit = Physics2D.Raycast(startFrom, Vector2.up, RayLenth, collisionMask))
            {
                Debug.Log("Raycast hit " + hit.collider.gameObject.name);
                if (hit.collider.gameObject.tag == "Player")
                {
                    broken = true;
                    StartCoroutine(triggered());
                }
                break;
            }
            startFrom += new Vector2(RaySpacing, 0);
        }
    }
}
