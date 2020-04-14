using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController : RayCast
{
    public float moveSpeed = 0.5f;
    public float moveRL = 10;
    // Start is called before the first frame update
    new public Transform transform;
    public Rigidbody m_rb;
    private Vector3 moveDirection = Vector3.zero;
    private Vector3 previousDirection = Vector3.zero;
    private Vector3 forwardDetection = Vector3.zero;
    private bool first;
    void Start()
    {
        transform = gameObject.transform;
        moveSpeed = 0.5f;
        first = true;
    }

    // Update is called once per frame
    void Update()
    {
        CheckState();
        MoveDetect();
        ForwardDetection();
    }

    private void MoveDetect()
    {
        if (isGround)
        {
            moveDirection = new Vector3(0, 0.0f, Input.GetAxis("Vertical"));
            moveDirection = moveDirection * moveSpeed;
            previousDirection = moveDirection;
            transform.Translate(moveDirection, Space.Self);
            first = true;
        }
        else
        {
            if (first) {
                //moveDirection = transform.TransformPoint(moveDirection) - transform.localPosition;
                //Debug.Log(moveDirection);
                //moveDirection *=moveSpeed;
                //first = false;
            }
            else
            {

            }
            transform.Translate(moveDirection);

        }
        //moveDirection = transform.TransformDirection(moveDirection);

    }
    private void ForwardDetection()
    {
        if (Input.GetAxis("Horizontal") != 0 && isGround)
        {
            forwardDetection = new Vector3(0, Input.GetAxis("Horizontal"), 0) * moveRL * Input.GetAxis("Vertical");
            transform.Rotate(forwardDetection, Space.Self);
        }
        else if(!isGround)
        {
            float degree = 0;
            if (Input.GetKey(KeyCode.E))
            {
                degree = -1.5f;
            }
            else if(Input.GetKey(KeyCode.Q))
            {
                degree = 1.5f;
            }
            forwardDetection = new Vector3(Input.GetAxis("Vertical")*2, Input.GetAxis("Horizontal")*2, degree) * moveRL * 0.5f;
            transform.Rotate(forwardDetection, Space.Self);
        }
    }

}
