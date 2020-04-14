using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class change : MonoBehaviour
{
    Player player;
    ControllerPlayer controller;
    PlayerAnim playerAnim;

    private void Awake()
    {
        player = Player.Instance;
        playerAnim = Player.Instance.GetComponent<PlayerAnim>();
        SubscribeSkill();
        SubUnpack();
    }
    private void Start()
    {
        controller = player.controller;
        if (controller == null) Debug.Log("simple Change lose controller");
    }
    protected void SubscribeSkill()
    {   //  notice to subscribe witch event
        PlayerSkill.On_Change_Click += skill;
        Debug.Log("Change is listen");
    }
    protected void SubUnpack()
    {
        PlayerSkill.UnPack_Change_Click += DestroyTheClass;
    }
    protected void DestroyTheClass()
    {
        Destroy(this);
    }
    IEnumerator skill()
    {   /* actually skill write in there */
        if (CouldChange())
        {
            player.SetState();
        }
        yield return null;
    }
    bool CouldChange()
    {
        RaycastHit2D hit;
        Vector2 rayOrigin = (controller.raycastOrigins.topLeft+ controller.raycastOrigins.bottomLeft)/2;
        float rayLength = (controller.raycastOrigins.topRight.x - controller.raycastOrigins.topLeft.x);
        hit = Physics2D.Raycast(rayOrigin, Vector2.right, rayLength, controller.collisionMask);
        if (hit) return false;
        return true;
    }

}
