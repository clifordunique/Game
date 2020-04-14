using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class simpleAttack : MonoBehaviour
{
    [SerializeField]
    protected float atk_cd, damageLatency = 0.1f;
    float last_atk = -2f;
    Player player;
    PlayerAnim playerAnim;
    ControllerPlayer controller;
    DialoguePrinter dialoguePrinter;
    bool attack_twice;

    private void Awake()
    {
        player = Player.Instance;
        playerAnim = Player.Instance.GetComponent<PlayerAnim>();
        dialoguePrinter = FindObjectOfType<DialoguePrinter>();
        SubscribeSkill();
        SubUnpack();
    }
    private void Start()
    {
        controller = player.controller;
        if (controller == null) Debug.Log("simple Attack lose controller");
    }
    protected void SubscribeSkill()
    {   //  notice to subscribe witch event
        PlayerSkill.On_Attack_Click += skill;
        Debug.Log("Attack is listen");
    }
    protected void SubUnpack()
    {
        PlayerSkill.UnPack_Attack_Click += DestroyTheClass;
    }
    protected void DestroyTheClass()
    {
        Destroy(this);
    }
    IEnumerator skill()
    {   /* actually skill write in there */
        /*if (dialoguePrinter.talking) yield break;
        if (!dialoguePrinter.talking && dialoguePrinter.canTalk != null)
        {
            dialoguePrinter.canTalk.TriggerDialogue();
        }
        else */if (Time.time > last_atk + atk_cd)
        {
            last_atk = Time.time;
            yield return new WaitForFixedUpdate();
            player.state.canControll = false;
            if (Input.GetKey(KeyBoardInput.Instance.up))
            {
                playerAnim.SetAnim("attackUp", 2);
                yield return new WaitForSeconds(damageLatency);
                AttackUp();
            }
            else if (Input.GetKey(KeyBoardInput.Instance.down) && !player.state.isGround)
            {
                playerAnim.SetAnim("attackDown", 2);
                yield return new WaitForSeconds(damageLatency);
                AttackDown();
            }
            else
            {
                if (attack_twice) playerAnim.SetAnim("attack1", 2);
                else playerAnim.SetAnim("attack2", 2);
                yield return new WaitForSeconds(damageLatency);
                AttackForward();
            }
            yield return new WaitForSeconds(0.3f);
            player.state.canControll = true;
        }
        
    }

    void AttackForward()
    {
        attack_twice = !attack_twice;
        float forwardLength = player.attackLength;
        for (int i = 0; i < controller.horizontalRayCount; i++)
        {
            Vector2 rayOrigin = (player.forward == -1) ? controller.raycastOrigins.bottomLeft : controller.raycastOrigins.bottomRight;
            rayOrigin += Vector2.up * controller.horizontalRaySpacing * i;
            RaycastHit2D hit;
            Debug.DrawRay(rayOrigin, Vector2.right* player.forward * forwardLength, Color.white);
            hit = Physics2D.Raycast(rayOrigin, Vector2.right * player.forward, forwardLength, player.layerForMonster);
            if (hit)
            {
                /* hit and give damage to the enemy */
                HitableObj hitable = hit.collider.gameObject.GetComponent<HitableObj>();
                if (hitable)
                {
                    hitable.getHit.Invoke(Player.Instance.atk);
                }
                break;
            }
        }
    }
    void AttackUp()
    {
        float upLength = player.attackLength / 3 *2;
        for (int i = 0; i < controller.verticalRayCount; i++)
        {
            Vector2 rayOrigin = controller.raycastOrigins.topLeft;
            rayOrigin += Vector2.right * controller.verticalRaySpacing * i;
            RaycastHit2D hit;
            Debug.DrawRay(rayOrigin, Vector2.up * upLength, Color.white);
            hit = Physics2D.Raycast(rayOrigin, Vector2.up , upLength, player.layerForMonster);
            if (hit)
            {
                /* hit and give damage to the enemy */
                HitableObj hitable = hit.collider.gameObject.GetComponent<HitableObj>();
                if (hitable)
                {
                    hitable.getHit.Invoke(Player.Instance.atk);
                }
                break;
            }
        }
    }
    void AttackDown()
    {
        float downLength = player.attackLength / 2;
        for (int i = 0; i < controller.verticalRayCount; i++)
        {
            Vector2 rayOrigin = controller.raycastOrigins.bottomLeft;
            rayOrigin += Vector2.right * controller.verticalRaySpacing * i;
            RaycastHit2D hit;
            Debug.DrawRay(rayOrigin, Vector2.down * downLength, Color.white);
            hit = Physics2D.Raycast(rayOrigin, Vector2.down, downLength, player.layerForMonster);
            if (hit)
            {
                //  to simulate force when down attack
                if (Player.Instance.velocity.y <= 0) Player.Instance.velocity.y = 5;
                else Player.Instance.velocity.y += 2;

                /* hit and give damage to the enemy */
                HitableObj hitable = hit.collider.gameObject.GetComponent<HitableObj>();
                if (hitable)
                {
                    hitable.getHit.Invoke(Player.Instance.atk);
                }
            }
        }
    }
}
