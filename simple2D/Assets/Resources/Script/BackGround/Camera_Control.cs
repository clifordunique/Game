using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera_Control : MonoBehaviour
{
    public GameObject target;
    public float left_range;
    public float right_range;
    public float down_range;
    public float up_range;
    public Vector3 next_pos;

    private static float cameraHeight;
    private static float cameraWidth;

    // Start is called before the first frame update
    void Start()
    {
        target = Player.Instance.gameObject;
        Camera camera = GetComponent<Camera>();
        cameraHeight = 2f * camera.orthographicSize;
        cameraWidth = cameraHeight * camera.aspect;
    }

    // Update is called once per frame
    void Update()
    {
        next_pos = target.transform.position;
        next_pos.z = this.transform.position.z;
        if (next_pos.x <= left_range)
        {
            next_pos.x = left_range;
        }
        else if (next_pos.x >= right_range)
        {
            next_pos.x = right_range;
        }
        if (next_pos.y <= down_range)
        {
            next_pos.y = down_range;
        }
        else if (next_pos.y >= up_range)
        {
            next_pos.y = up_range;
        }
        this.transform.position = next_pos;
    }
    void OnDrawGizmos()
    {
        
        Gizmos.color = new Color(0, 1, 0, 0.3f);
        /*Gizmos.DrawCube(new Vector2(left_range + right_range, down_range + up_range)/2,
            new Vector2(right_range - left_range + cameraWidth, up_range - down_range + cameraHeight));*/
        Gizmos.DrawWireCube(new Vector2(left_range + right_range, down_range + up_range) / 2,
            new Vector2(right_range - left_range + cameraWidth, up_range - down_range + cameraHeight));
    }
}
