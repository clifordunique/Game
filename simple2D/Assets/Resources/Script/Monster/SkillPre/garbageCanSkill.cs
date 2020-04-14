using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class garbageCanSkill : RaycastController
{
    [SerializeField]
    float speed=0, lifeTime=0;
    [SerializeField]
    Vector2 forward;
    RaycastHit2D hit;

    // Start is called before the first frame update
    new void Start()
    {
        base.Start();
        StartCoroutine(Ex());
    }

    // Update is called once per frame
    void Update()
    {
        UpdatRaycastOrigins();
        transform.Translate(forward * speed * Time.deltaTime);
        DetectCollision();
        //Debug.DrawRay(gameObject.transform.position, Vector2.right * forward.x * 20, Color.green);
    }

    public void SetInit(Vector2 Forward)
    {
        forward = Forward;
    }

    IEnumerator Ex()
    {
        yield return new WaitForSeconds(lifeTime);
        Destroy(gameObject);
    }
    private void DetectCollision()
    {
        float rayLength = skinWidth;
        for (int i = 0; i < horizontalRayCount; i++)
        {
            Vector2 rayOrigin = (forward.x <= 0) ? raycastOrigins.bottomLeft : raycastOrigins.bottomRight;
            rayOrigin += Vector2.up * horizontalRaySpacing * i;
            Debug.DrawRay(rayOrigin,forward.normalized * skinWidth, Color.green);
            /////////
            hit = Physics2D.Raycast(rayOrigin, forward, rayLength, collisionMask);
            if (hit)
            {
                /* to do to minus the object's hp; */
                Player target = hit.collider.gameObject.GetComponent<Player>();
                if (target)
                {
                    target.GetHurt(10, Player.Instance.transform.position);
                    /* hit the player */
                    //target.hp -= 10;
                }
                Destroy(gameObject);
                break;
            }
        }
    }
}
