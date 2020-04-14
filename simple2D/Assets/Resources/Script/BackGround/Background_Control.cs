using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Background_Control : MonoBehaviour
{
    public GameObject target;
    public float distance;
    public Vector3 camera_offset;
    Vector3 start_pos;
    Vector3 move;

    float left_range;
    float right_range;
    float down_range;
    float up_range;

    // Start is called before the first frame update
    void Start()
    {
        start_pos = this.transform.position + camera_offset * distance;
        target = GameObject.Find("/Main Camera");

        left_range = target.GetComponent<Camera_Control>().left_range;
        right_range = target.GetComponent<Camera_Control>().right_range;
        down_range = target.GetComponent<Camera_Control>().down_range;
        up_range = target.GetComponent<Camera_Control>().up_range;
    }

    // Update is called once per frame
    void Update()
    {
        move = target.transform.position;
        move.z = this.transform.position.z;
        if (move.x <= left_range)
        {
            move.x = left_range;
        }
        else if (move.x >= right_range)
        {
            move.x = right_range;
        }
        if (move.y <= down_range)
        {
            move.y = down_range;
        }
        else if (move.y >= up_range)
        {
            move.y = up_range;
        }
        move = -move;
        move.x = start_pos.x + move.x * distance;
        move.y = start_pos.y + move.y * distance;
        this.transform.position = move;
    }
}
