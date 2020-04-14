using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]

public class RaycastController : MonoBehaviour
{
    public enum layer
    {
        Player = 11,
        Obstacle = 13,
        Obstacle_w = 14,
        Obstacle_b = 15,
        Platform = 16,
        Platform_w = 17,
        Platform_b = 18
    }
    
    public new BoxCollider2D collider;
    public const float skinWidth = .15f;
    public LayerMask collisionMask;

    [HideInInspector]
    public float horizontalRaySpacing;
    [HideInInspector]
    public float verticalRaySpacing;
    [HideInInspector]
    public int horizontalRayCount = 4;
    public int verticalRayCount = 4;

    public virtual void Start()
    {
        collider = GetComponent<BoxCollider2D>();
        CalculateRaySpacing();
    }

    public RaycastOrigin raycastOrigins;
    public void UpdatRaycastOrigins()
    {
        Bounds bounds = collider.bounds;
        bounds.Expand(-skinWidth);
        raycastOrigins.bottomLeft = new Vector2(bounds.min.x, bounds.min.y);
        raycastOrigins.bottomRight = new Vector2(bounds.max.x, bounds.min.y);
        raycastOrigins.topLeft = new Vector2(bounds.min.x, bounds.max.y);
        raycastOrigins.topRight = new Vector2(bounds.max.x, bounds.max.y);
    }
    public void CalculateRaySpacing()
    {
        Bounds bounds = collider.bounds;
        bounds.Expand(-skinWidth);
        horizontalRayCount = Mathf.Clamp(horizontalRayCount, 2, 100);
        verticalRayCount = Mathf.Clamp(verticalRayCount, 2, 100);
        horizontalRaySpacing = bounds.size.y / (horizontalRayCount - 1);
        verticalRaySpacing = bounds.size.x / (verticalRayCount - 1);
    }

    public struct RaycastOrigin
    {
        public Vector2 topLeft, topRight;
        public Vector2 bottomLeft, bottomRight;
    }
}
