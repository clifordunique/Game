using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class skill000Prefab : RaycastController
{
    public int damage = 10;
    public float distance;
    public float speed;

    protected float startTime;
    protected Player player;
    protected float directionX;
    RaycastHit2D hit;
    protected HashSet<Transform> HurtObj = new HashSet<Transform>();
    [SerializeField]
    protected float rayLength;

    protected void DetectCollisionAround()
    {
        for (int i = 0; i < 8; i++)
        {
            Vector2 rayOrigin = gameObject.transform.position;
            Vector2 rayDir = Rotate(rayOrigin,45f);
            rayOrigin += Vector2.up * horizontalRaySpacing * i;
            Debug.DrawRay(rayOrigin, rayDir * rayLength, Color.green);
            /////////
            hit = Physics2D.Raycast(rayOrigin, Vector2.right, rayLength, collisionMask);
            if (hit)
            {
                if (!HurtObj.Contains(hit.transform))
                {
                    HurtObj.Add(hit.transform);
                    Debug.Log(hit.collider.gameObject.name);
                    /* to do to minus the object's hp; */
                    HitableObj target = hit.collider.gameObject.GetComponent<HitableObj>();
                    if (target)
                    {
                        target.getHit.Invoke(damage);
                    }
                }
            }
        }
    }

    protected void DetectCollision()
    {
        rayLength = (raycastOrigins.bottomRight.x - raycastOrigins.bottomLeft.x)/1.3f;
        for (int i = 0; i < horizontalRayCount; i++)
        {
            Vector2 rayOrigin = (raycastOrigins.bottomRight + raycastOrigins.bottomLeft) / 2;
            rayOrigin += Vector2.up * horizontalRaySpacing * i;
            Debug.DrawRay(rayOrigin, (Vector2.right* directionX) * rayLength, Color.green);
            /////////
            hit = Physics2D.Raycast(rayOrigin, Vector2.right * directionX, rayLength, collisionMask);
            if (hit)
            {
                if (!HurtObj.Contains(hit.transform))
                {
                    HurtObj.Add(hit.transform);
                    Debug.Log(hit.collider.gameObject.name);
                    /* to do to minus the object's hp; */
                    HitableObj hitable = hit.collider.gameObject.GetComponent<HitableObj>();
                    if (hitable)
                    {
                        hitable.getHit.Invoke(Player.Instance.atk+1);
                    }
                }
            }
        }
    }
    Vector2 Rotate(Vector2 v, float degrees)
    {
        float sin = Mathf.Sin(degrees * Mathf.Deg2Rad);
        float cos = Mathf.Cos(degrees * Mathf.Deg2Rad);

        float tx = v.x;
        float ty = v.y;
        v.x = (cos * tx) - (sin * ty);
        v.y = (sin * tx) + (cos * ty);
        return v;
    }
}
