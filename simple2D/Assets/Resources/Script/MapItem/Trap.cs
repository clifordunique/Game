using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trap : MonoBehaviour
{
    [SerializeField]
    GameObject RespawnObj;
    Transform Respawn;

    Bounds bounds;
    // Start is called before the first frame update
    void Start()
    {
        Respawn = RespawnObj.transform;
        bounds = RespawnObj.GetComponent<SpriteRenderer>().bounds;
    }

    public void Trapped()
    {
        GameManager.Instance.StartTransition(true, true);
        Player.Instance.transform.position = (Vector2)Respawn.position;
    }
    /*private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawCube((Vector2)bounds.center, (Vector2)bounds.size);
        Gizmos.DrawCube(Vector2.zero, (Vector2)bounds.size);
    }*/
}
