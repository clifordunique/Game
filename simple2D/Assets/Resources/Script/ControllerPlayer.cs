/*
 * this script is to control all gravity object like Player or monsters
 * in this script we can see how the collider detect and stop on the platform
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerPlayer : RaycastController
{
    public override void Start()
    {
        base.Start();
        collisionInfo.Reset();
        Init();
    }
    [SerializeField]
    LayerMask inWhite,inBlack;

    RaycastHit2D hit;
    public CollisionInfo collisionInfo;
    float maxSlopeAngle = 45;
    float previousY = 0;
    //  Move is the main function
    public void Move(Vector3 velocity)
    {
        UpdatRaycastOrigins();
        collisionInfo.Reset();
        YReverseJudge(velocity);
        if (velocity.y != 0) VerticalCollision(ref velocity);
        if (velocity.x != 0) HorizontalCollision(ref velocity);
        transform.Translate(velocity);
    }

    void VerticalCollision(ref Vector3 velocity)
    {
        float directionY = Mathf.Sign(velocity.y);
        float rayLength = Mathf.Abs(velocity.y) + skinWidth;
        if (Player.Instance.colorState)
        {   //  this is in true
            for (int i = 0; i < verticalRayCount; i++)
            {
                Vector2 rayOrigin = (directionY == -1) ? raycastOrigins.bottomLeft : raycastOrigins.topLeft;
                rayOrigin += Vector2.right * (verticalRaySpacing * i);
                Debug.DrawRay(rayOrigin, Vector2.up * directionY * rayLength, Color.red);
                hit = Physics2D.Raycast(rayOrigin, Vector2.up * directionY, rayLength, inBlack);
                if (hit)
                {
                    collisionInfo.resetFlag = collisionInfo.below = directionY == -1;
                    collisionInfo.above = directionY == 1;
                    if (collisionInfo.above && (hit.collider.gameObject.layer == (int)layer.Platform || hit.collider.gameObject.layer == (int)layer.Platform_w)) { }
                    else if (collisionInfo.above && (hit.collider.gameObject.layer == (int)layer.Obstacle || hit.collider.gameObject.layer == (int)layer.Obstacle_w))
                    {
                        collisionInfo.resetFlag = true;
                    }
                    //else if()
                    else
                    {
                        velocity.y = (hit.distance - skinWidth * 0.5f) * directionY;
                        collisionInfo.isGround = true;
                    }
                    rayLength = hit.distance;
                }
            }
        }
        else
        {
            for (int i = 0; i < verticalRayCount; i++)
            {
                Vector2 rayOrigin = (directionY == -1) ? raycastOrigins.bottomLeft : raycastOrigins.topLeft;
                rayOrigin += Vector2.right * (verticalRaySpacing * i);
                Debug.DrawRay(rayOrigin, Vector2.up * directionY * rayLength, Color.red);
                hit = Physics2D.Raycast(rayOrigin, Vector2.up * directionY, rayLength, inWhite);
                if (hit)
                {
                    collisionInfo.resetFlag = collisionInfo.below = directionY == -1;
                    collisionInfo.above = directionY == 1;
                    if (collisionInfo.above && (hit.collider.gameObject.layer == (int)layer.Platform || hit.collider.gameObject.layer == (int)layer.Platform_b)) { }
                    else if (collisionInfo.above && (hit.collider.gameObject.layer == (int)layer.Obstacle || hit.collider.gameObject.layer == (int)layer.Obstacle_b))
                    {
                        collisionInfo.resetFlag = true;
                    }
                    //else if()
                    else
                    {
                        velocity.y = (hit.distance - skinWidth * 0.5f) * directionY;
                        collisionInfo.isGround = true;
                    }
                    rayLength = hit.distance;
                }
            }
        }

    }
    void HorizontalCollision(ref Vector3 velocity)
    {
        float directionX = Mathf.Sign(velocity.x);
        float rayLength = Mathf.Abs(velocity.x) + skinWidth;

        if (Player.Instance.colorState)
        {   //  this is in black
            for (int i = 0; i < horizontalRayCount; i++)
            {
                Vector2 rayOrigin = (directionX == -1) ? raycastOrigins.bottomLeft : raycastOrigins.bottomRight;
                rayOrigin += Vector2.up * horizontalRaySpacing * i;
                Debug.DrawRay(rayOrigin, Vector2.right * directionX * rayLength, Color.green);
                /////////
                hit = Physics2D.Raycast(rayOrigin, Vector2.right * directionX, rayLength, inBlack);
                if (hit)
                {
                    float hitAngle = Vector2.Angle(hit.normal, Vector2.up);
                    if (i == 0 && hitAngle < maxSlopeAngle)
                    {
                        ClimpSlope(ref velocity, hitAngle);
                    }
                    else
                    {

                    }
                    collisionInfo.left = directionX == -1;
                    collisionInfo.right = directionX == 1;
                    if ((hit.collider.gameObject.layer == (int)layer.Platform || hit.collider.gameObject.layer == (int)layer.Platform_w) && velocity.y > -0.2f) { }
                    else
                    {
                        velocity.x = (hit.distance - skinWidth * 0.5f) * directionX;
                    }
                    rayLength = hit.distance;
                    break;
                }
            }
        }
        else
        {
            for (int i = 0; i < horizontalRayCount; i++)
            {
                Vector2 rayOrigin = (directionX == -1) ? raycastOrigins.bottomLeft : raycastOrigins.bottomRight;
                rayOrigin += Vector2.up * horizontalRaySpacing * i;
                Debug.DrawRay(rayOrigin, Vector2.right * directionX * rayLength, Color.green);
                /////////
                hit = Physics2D.Raycast(rayOrigin, Vector2.right * directionX, rayLength, inWhite);
                if (hit)
                {
                    float hitAngle = Vector2.Angle(hit.normal, Vector2.up);
                    if (i == 0 && hitAngle < maxSlopeAngle)
                    {
                        ClimpSlope(ref velocity, hitAngle);
                    }
                    else
                    {

                    }
                    collisionInfo.left = directionX == -1;
                    collisionInfo.right = directionX == 1;
                    if ((hit.collider.gameObject.layer == (int)layer.Platform || hit.collider.gameObject.layer == (int)layer.Platform_b) && velocity.y > -0.2f) { }
                    else
                    {
                        velocity.x = (hit.distance - skinWidth * 0.5f) * directionX;
                    }
                    rayLength = hit.distance;
                    break;
                }
            }
        }

    }
    void ClimpSlope(ref Vector3 velocity, float slopeAngle)
    {
        float moveDistance = Mathf.Abs(velocity.x);
        velocity.y = Mathf.Sin(slopeAngle * Mathf.Deg2Rad) * moveDistance;
        velocity.x = Mathf.Cos(slopeAngle * Mathf.Deg2Rad) * moveDistance * Mathf.Sign(velocity.x);
    }
    void YReverseJudge(Vector3 velocity)
    {
        previousY = velocity.y;
    }
    void Init()
    {
        inWhite |= (1 << LayerMask.NameToLayer("Obstacle"));
        inWhite |= (1 << LayerMask.NameToLayer("Obstacle_b"));
        inWhite |= (1 << LayerMask.NameToLayer("Platform"));
        inWhite |= (1 << LayerMask.NameToLayer("Platform_b"));
        inBlack |= (1 << LayerMask.NameToLayer("Obstacle"));
        inBlack |= (1 << LayerMask.NameToLayer("Obstacle_w"));
        inBlack |= (1 << LayerMask.NameToLayer("Platform"));
        inBlack |= (1 << LayerMask.NameToLayer("Platform_w"));

    }
    public struct CollisionInfo
    {
        public bool above, below;
        public bool left, right;
        public bool resetFlag;
        public bool isGround;
        public void Reset()
        {
            isGround = false;
            above = below = false;
            left = right = false;
            resetFlag = false;
        }
    }
}
