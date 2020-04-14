using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonPhysic : MonoBehaviour
{
    public static IEnumerator ChangeX(Monster monster, float speed, int weight, float time)
    {
        float curTime = Time.time;
        float endTime = Time.time + time;
        while (Time.time < endTime)
        {
            monster.ChangeVelocityX(speed, weight);
            yield return null;
        }
        monster.ChangeVelocityX(0, weight);
    }
    public static IEnumerator ChangeY(Monster monster, float speed, int weight, float time)
    {
        float curTime = Time.time;
        float endTime = Time.time + time;
        while (Time.time < endTime)
        {
            monster.ChangeVelocityY(speed, weight);
            yield return null;
        }
    }
    public static IEnumerator AccelerateX(Monster monster, float a, float v0, float weight, float time)
    {
        float speed = v0;
        float curTime = Time.time;
        float endTime = Time.time + time;
        while (Time.time < endTime)
        {
            monster.ChangeVelocityY(speed, 5);
            v0 += a * Time.deltaTime;
            yield return null;
        }
    }
    public static IEnumerator Test()
    {
        float cuT = Time.time;
        float period;
        Debug.Log(Time.time);
        while (true)
        {
            yield return null;
            period = Time.time - cuT;
            cuT = Time.time;
        }
    }
    public static bool TouchPlayer(Controller2D controller,Monster monster)
    {
        LayerMask layerForPlayer = (1 << LayerMask.NameToLayer("Player_w"));
        layerForPlayer |= (1 << LayerMask.NameToLayer("Player_b"));
        for (int i = 0; i < controller.horizontalRayCount; i++)
        {
            float forwardLength = (controller.raycastOrigins.bottomRight - controller.raycastOrigins.bottomLeft).x;
            Vector2 rayOrigin = controller.raycastOrigins.bottomLeft;
            rayOrigin += Vector2.up * controller.horizontalRaySpacing * i;
            RaycastHit2D hit;
            Debug.DrawRay(rayOrigin, Vector2.right * forwardLength, Color.white);
            hit = Physics2D.Raycast(rayOrigin, Vector2.right, forwardLength, layerForPlayer);
            if (hit) return true;
        }
        return false;
    }
    public static bool IsRayHitPlayer(RaycastHit2D hit)
    {
        if (hit)
        {
            return true;
        }
        return false;
    }
    public static Vector3 RotateClockwise(Vector3 baseVec, float degree)
    {
        float Rad = Mathf.Deg2Rad * degree;
        float newX = baseVec.x * Mathf.Cos(Rad) - baseVec.y * Mathf.Sin(Rad);
        float newY = baseVec.x * Mathf.Sin(Rad) + baseVec.y * Mathf.Cos(Rad);
        return new Vector3(newX, newY, 0);
    }
}
