using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof (Player))]
[RequireComponent (typeof (Animator))]

public class PlayerAnim : MonoBehaviour
{
    Animator anim;
    Player player;
    PlayerSkill playerSkill;
    [SerializeField]
    int now_dir, last_dir = 1;
    [SerializeField]
    float GroundAction;//0:breathe 1:run

    ControllerPlayer controller;
    //bool attack_twice = false;
    float attack_twice;
    float last_attack;
    [SerializeField]
    private int previous_priority = 0;

    void Start()
    {
        player = Player.Instance;
        playerSkill = GetComponent<PlayerSkill>();
        player.Change += setColor;
        anim = GetComponent<Animator>();
        controller = player.controller;
        last_dir = now_dir = transform.localScale.x > 0 ? 1 : 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (previous_priority == 0) NormalAnim(player.velocity);
    }
    
    public void setColor(bool state)
    {
        if (!state)
        {
            anim.SetFloat("Color", 0f);
        }
        else
        {
            anim.SetFloat("Color", 1f);
        }
    }

    public void SetAnim(string name, int priority)
    {
        if(priority > previous_priority )
        {
            previous_priority = priority;
            StartCoroutine(PlayAnim(name));
        }
    }
    IEnumerator PlayAnim(string name)
    {
        anim.Play(name);
        yield return null;
        yield return new WaitForSeconds(anim.GetCurrentAnimatorStateInfo(0).length);
        previous_priority = 0;
    }

    void NormalAnim(Vector3 v)
    {
        if (v.x != 0) now_dir = v.x > 0 ? 1 : 0;
        if (now_dir != last_dir)
        {
            Vector3 tmp = transform.localScale;
            transform.localScale = new Vector3(-tmp.x, tmp.y, tmp.z);
            last_dir = now_dir;
        }
        if (!controller.collisionInfo.below)
        {
            if (!anim.GetCurrentAnimatorStateInfo(0).IsName("jump"))
            {
                anim.Play("jump");
            }
            if (Mathf.Abs(v.y) <= 0.12 && !anim.GetCurrentAnimatorStateInfo(0).IsName("attack1")
                                       && !anim.GetCurrentAnimatorStateInfo(0).IsName("attack2"))
            {
                anim.Play("jump_change");
            }
            anim.SetFloat("v_y", v.y);
        }
        else
        {
            if (anim.GetCurrentAnimatorStateInfo(0).IsName("jump"))
            {
                anim.Play("land");
            }
            if (v.x == 0) //breathe
            {
                GroundAction = 0;
            }
            else
            {
                GroundAction = 1;
            }
            anim.SetFloat("motion", GroundAction);
        }
    }

}

