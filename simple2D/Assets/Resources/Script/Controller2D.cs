/*
 * this script is to control all gravity object like Player or monsters
 * in this script we can see how the collider detect and stop on the platform
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller2D : RaycastController
{
    public override void Start()
    {
        base.Start();
        collisionInfo.Reset();
    }
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

        for (int i = 0; i < verticalRayCount; i++)
        {
            Vector2 rayOrigin = (directionY == -1) ? raycastOrigins.bottomLeft : raycastOrigins.topLeft;
            rayOrigin += Vector2.right * (verticalRaySpacing * i);
            Debug.DrawRay(rayOrigin, Vector2.up * directionY * rayLength, Color.red);
            hit = Physics2D.Raycast(rayOrigin, Vector2.up * directionY, rayLength, collisionMask);
            if (hit)
            {
                collisionInfo.resetFlag = collisionInfo.below = directionY == -1;
                collisionInfo.above = directionY == 1;
                if (collisionInfo.above && hit.collider.gameObject.layer == (int)layer.Platform) { }
                else if (collisionInfo.above && hit.collider.gameObject.layer == (int)layer.Obstacle)
                {
                    collisionInfo.resetFlag = true;
                }
                //else if()
                else {
                    velocity.y = (hit.distance - skinWidth * 0.5f) * directionY;
                    collisionInfo.isGround = true;
                }
                rayLength = hit.distance;
            }
        }
    }
    void HorizontalCollision(ref Vector3 velocity)
    {
        float directionX = Mathf.Sign(velocity.x);
        float rayLength = Mathf.Abs(velocity.x) + skinWidth;
        for (int i = 0; i < horizontalRayCount; i++)
        {
            Vector2 rayOrigin = (directionX == -1) ? raycastOrigins.bottomLeft : raycastOrigins.bottomRight;
            rayOrigin += Vector2.up * horizontalRaySpacing * i;
            Debug.DrawRay(rayOrigin, Vector2.right * directionX * rayLength, Color.green);
            /////////
            hit = Physics2D.Raycast(rayOrigin, Vector2.right * directionX, rayLength, collisionMask);
            if (hit)
            {
                /*if (hit.collider.gameObject.tag == "SceneChange" && this.tag == "Player")
                {
                    hit.collider.gameObject.GetComponent<SceneLeave>().LoadSceneCall();
                }*/
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
                if (hit.collider.gameObject.layer == (int)layer.Platform && velocity.y > -0.2f) { }
                else
                {
                    velocity.x = (hit.distance - skinWidth * 0.5f) * directionX;
                }
                rayLength = hit.distance;
                break;
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
