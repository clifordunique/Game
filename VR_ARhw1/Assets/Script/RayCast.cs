using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayCast : MonoBehaviour
{
    // Start is called before the first frame update
    public BoxCollider m_collider;
    public bool isGround;
    Vector3 bottomLeftBack, bottomLeftFront, bottomRightBack, bottomRightFront;
    Vector3 bottomCenter;

    public float time_f,time_end;

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        CheckState();
    }

    private void FlashPosition()
    {
        bottomLeftBack = m_collider.center - m_collider.size / 2;
        bottomRightBack = new Vector3(m_collider.size.x / 2, 0, 0) + bottomLeftBack;
        bottomLeftFront = new Vector3(0, 0, m_collider.size.z / 2) + bottomLeftBack;
        bottomRightFront = new Vector3(0, 0, m_collider.size.z / 2) + bottomRightBack;
        bottomCenter = m_collider.center - new Vector3(0, m_collider.size.y / 3, 0);
    }

    private void IsGround()
    {
        RaycastHit isHit;
        Vector3 rayorigin = transform.TransformPoint(bottomCenter);
        if(time_end <= Time.time)
        {
            if (Physics.Raycast(rayorigin, Vector3.down, out isHit,2f))
            {
                Debug.DrawRay(rayorigin, Vector3.down, Color.yellow);
                isGround = true;
                Debug.Log("Hit");
            }
            else
            {
                time_end = Time.time +0.3f;
                isGround = false;
                Debug.Log("Not Hit");
            }
        }
    }

    public void CheckState()
    {
        FlashPosition();
        IsGround();
    }
}
