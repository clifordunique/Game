using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartEndPlat : PlatformController
{
    public float moveSpeed;
    [SerializeField]
    int indexPosition;
    [SerializeField]
    public Vector3[] positionArr;
    float recordX, recordY;
    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
        indexPosition = 0;
        moveForward = positionArr[indexPosition + 1] - positionArr[indexPosition];
        moveForward = moveForward.normalized;
        SetVel(ref velocity);
        SetmoveForward(moveForward.normalized);
    }

    // Update is called once per frame
    void Update()
    {
        UpdatRaycastOrigins();
        if (DetectToChange())
        {   // turn to next target
            if (indexPosition == positionArr.Length - 1) indexPosition = 0;
            else indexPosition++;
            moveForward = positionArr[NextIndex()] - positionArr[indexPosition];
            moveForward = moveForward.normalized;
            SetVel(ref velocity);
        }
        MovePassengers(velocity);
    }
    private void LateUpdate()
    {
        transform.Translate(velocity);
    }
    bool DetectToChange()
    {
        if (recordX < positionArr[NextIndex()].x)
        {
            if(gameObject.transform.position.x > positionArr[NextIndex()].x)
            {
                return true;
            }
        }else if (recordX > positionArr[NextIndex()].x)
        {
            if (gameObject.transform.position.x < positionArr[NextIndex()].x)
            {
                return true;
            }
        }
        else if(recordY < positionArr[NextIndex()].y)
        {
            if (gameObject.transform.position.y > positionArr[NextIndex()].y)
            {
                return true;
            }
        }
        else if (recordY > positionArr[NextIndex()].y)
        {
            if (gameObject.transform.position.y < positionArr[NextIndex()].y)
            {
                return true;
            }
        }
        return false ;
    }
    void SetVel(ref Vector3 velocity)
    {
        velocity = GetForward() * Time.deltaTime * moveSpeed;
        recordX = gameObject.transform.position.x;
        recordY = gameObject.transform.position.y;
    }
    int NextIndex()
    {
        if(indexPosition == positionArr.Length - 1)
        {
            return 0;
        }
        return indexPosition+1;
    }
}
