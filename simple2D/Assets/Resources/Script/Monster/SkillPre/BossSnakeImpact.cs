using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSnakeImpact : RaycastController
{
    [SerializeField]
    public float speed = 0, lifeTime = 0, MaxSpeed;
    [SerializeField]
    Vector2 forward;
    public float damage;
    RaycastHit2D hit;

    // Start is called before the first frame update
    new void Start()
    {
        base.Start();
        collisionMask |= (1 << LayerMask.NameToLayer("Obstacle"));
        collisionMask |= (1 << LayerMask.NameToLayer("Obstacle_w"));
        collisionMask |= (1 << LayerMask.NameToLayer("Obstacle_b"));
        StartCoroutine(Ex());
    }

    // Update is called once per frame
    void Update()
    {
        UpdatRaycastOrigins();
        transform.Translate(new Vector2(1,0) * speed * Time.deltaTime, Space.Self);
        DetectCollision();
        //Debug.DrawRay(gameObject.transform.position, Vector2.right * forward.x * 20, Color.green);
    }

    public void SetInit()
    {
        speed = MaxSpeed;
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
            Debug.DrawRay(rayOrigin, forward.normalized * skinWidth, Color.green);
            /////////
            hit = Physics2D.Raycast(rayOrigin, forward, rayLength, collisionMask);
            if (hit)
            {
                if (13 <= hit.collider.gameObject.layer && hit.collider.gameObject.layer <= 15) Destroy(gameObject);
                /* to do to minus the object's hp; */
                Player target = hit.collider.gameObject.GetComponent<Player>();
                if (target)
                {
                    target.GetHurt(10, Player.Instance.transform.position);
                }
                Destroy(gameObject);
                break;
            }
        }
        for (int i = 0; i < verticalRayCount; i++)
        {
            Vector2 rayOrigin = (forward.y <= 0) ? raycastOrigins.bottomLeft : raycastOrigins.topLeft;
            rayOrigin += Vector2.right * verticalRaySpacing * i;
            Debug.DrawRay(rayOrigin, forward.normalized * skinWidth, Color.green);
            /////////
            hit = Physics2D.Raycast(rayOrigin, forward, rayLength, collisionMask);
            if (hit)
            {
                if (13 <= hit.collider.gameObject.layer && hit.collider.gameObject.layer <= 15) Destroy(gameObject);
                /* to do to minus the object's hp; */
                Player target = hit.collider.gameObject.GetComponent<Player>();
                if (target)
                {
                    target.GetHurt(10, hit.point);
                }
                Destroy(gameObject);
                break;
            }
        }
    }
}
