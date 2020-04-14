using UnityEngine;
using System.Collections;

public class CompleteCameraController : MonoBehaviour
{

    public GameObject player;       //Public variable to store a reference to the player game object

    public Vector3 offset;         //Private variable to store the offset distance between the player and camera

    public Vector3 temp;
    // Use this for initialization
    void Start()
    {
        //Calculate and store the offset value by getting the distance between the player's position and camera's position.
        offset = new Vector3(0, -3f, 0);
    }

    // LateUpdate is called after Update each frame
    void Update()
    {
        temp = player.transform.TransformPoint(Vector3.forward);
        temp = temp - player.transform.TransformPoint(Vector3.zero);
        gameObject.transform.rotation = player.transform.rotation;
        if(gameObject.transform.position != player.transform.position)
        {
            transform.position = (player.transform.position - temp*5 - offset);
        }


        // Set the position of the camera's transform to be the same as the player's, but offset by the calculated offset distance.
    }
}
