using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformController : RaycastController
{
    public LayerMask passengerMask;
    public Vector3 moveForward;
    public Vector3 velocity;

     [HideInInspector]
    float Stimer, Etimer;
    
    public void MovePassengers(Vector3 velocity)
    {   // velocity is the platform's speed
        HashSet<Transform> movePassengers = new HashSet<Transform>();
        float directionX = Mathf.Sign(velocity.x);
        float directionY = Mathf.Sign(velocity.y);

        if (velocity.y != 0)
        {
            float rayLength = Mathf.Abs(velocity.y) + skinWidth;
            for (int i = 0; i < verticalRayCount; i++)
            {
                Vector2 rayOrigin = raycastOrigins.topLeft;
                rayOrigin += Vector2.right * (verticalRaySpacing * i);
                RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.up , rayLength, passengerMask);
                if (hit)
                {
                    if (!movePassengers.Contains(hit.transform)){
                        movePassengers.Add(hit.transform);
                    }
                    float pushY = Mathf.Abs(velocity.y);
                    hit.transform.Translate(new Vector3(0, pushY));
                }
            }
        }
        if (velocity.x != 0)
        {
            float rayLength = Mathf.Abs(velocity.x) + skinWidth;
            for (int i = 0; i < verticalRayCount; i++)
            {
                Vector2 rayOrigin = raycastOrigins.topLeft ;
                rayOrigin += Vector2.right * (verticalRaySpacing * i);
                RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.up , rayLength, passengerMask);
                if (hit)
                {
                    if (!movePassengers.Contains(hit.transform))
                    {
                        movePassengers.Add(hit.transform);
                    }
                    float pushX = velocity.x;
                    hit.transform.Translate(new Vector3(pushX, 0));
                }
            }
        }
    }
    public float GetStartTime()
    {
        return Stimer;
    }
    public float GetEndTime()
    {
        return Etimer;
    }
    public Vector3 GetForward()
    {
        return moveForward;
    }
    public void WriteStartTime(float number)
    {
        Stimer = number;
    }
    public void WriteEndTime(float number)
    {
        Etimer = number;
    }
    public void TurnAround()
    {
        moveForward *= -1;
    }
    public void SetmoveForward(Vector3 forward)
    {
        moveForward = forward;
    }

}
