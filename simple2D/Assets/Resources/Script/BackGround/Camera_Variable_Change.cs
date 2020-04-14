using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera_Variable_Change : MonoBehaviour
{
    public GameObject target;
    public GameObject player;
    public float movecamera;
    float original_downrange;

    // Start is called before the first frame update
    void Start()
    {
        target = GameObject.Find("/Main Camera");
        original_downrange = target.GetComponent<Camera_Control>().down_range;
        player = GameObject.Find("/player");
    }

    // Update is called once per frame
    void Update()
    {
        if(player.transform.position.x <= this.transform.position.x + movecamera && player.transform.position.x >= this.transform.position.x - movecamera)
        {
            target.GetComponent<Camera_Control>().down_range = original_downrange - (this.transform.position.x - player.transform.position.x + movecamera) / 2;
        }
    }
}
