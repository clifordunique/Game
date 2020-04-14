/* this script is contain the equipment skill, in the Player object
 * the three skill is A, S, D
 * see the SkillSubscribe.cs for detail 
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[RequireComponent(typeof(Player))]

public class PlayerSkill : Singleton<PlayerSkill>
{
    public delegate IEnumerator theAttack();
    public delegate IEnumerator theSkillA();
    public delegate IEnumerator theSkillB();
    public delegate IEnumerator theSkillC();
    public delegate void UnPack();

    public static event theSkillA On_Attack_Click;
    public static event theSkillA On_SkillA_Click;
    public static event theSkillB On_SkillB_Click;
    public static event theSkillC On_SkillC_Click;
    public static event UnPack UnPack_Attack_Click;
    public static event UnPack UnPack_SkillA_Click;
    public static event UnPack UnPack_SkillB_Click;
    public static event UnPack UnPack_SkillC_Click;

    public Action<string, int> skill_anim;
    [SerializeField]
    protected float atk_cd;
    float last_atk = 0;
    void Update()
    {
        if (Input.GetKeyDown(KeyBoardInput.Instance.attack) && Time.time > last_atk + atk_cd)
        {
            //Debug.Log(Time.time + "v.s." + (last_atk + atk_cd) + (Time.time > last_atk + atk_cd));
            last_atk = Time.time;
            skill_anim?.Invoke("attack", 2);
            UseAttack();
        }

        if (Input.GetKeyDown(KeyBoardInput.Instance.skillA))
        {
            //use skill 1
            UseSkillA();
        }
        if (Input.GetKeyDown(KeyBoardInput.Instance.skillB))
        {
            //use skill 2
            UseSkillB();
        }
        if (Input.GetKeyDown(KeyBoardInput.Instance.skillC))
        {
            //use skill 3
            StartCoroutine(MonPhysic.Test());
        }
        Debug.Log(Input.GetKeyDown(KeyBoardInput.Instance.down));
    }
    private void UseAttack()
    {   /*  check the listener and process the skill */
        if (On_Attack_Click != null)
        {
            StartCoroutine(On_Attack_Click());
        }
        else
        {
            Debug.Log("Attack is no listener !");
        }
    }
    private void UseSkillA()
    {   /*  check the listener and process the skill */
        if (On_SkillA_Click != null)
        {
            StartCoroutine(On_SkillA_Click());
        }
        else
        {
            Debug.Log("skillA is no listener !");
        }
    }
    private void UseSkillB()
    {   /*  check the listener and process the skill */
        if (On_SkillB_Click != null)
        {
            StartCoroutine(On_SkillB_Click());
        }
        else
        {
            Debug.Log("skillB is no listener !");
        }
    }
    public void Clear_Attack_Listenter()
    {
        if (UnPack_Attack_Click != null)
        {
            UnPack_Attack_Click();
            UnPack_Attack_Click = null;
        }
        On_Attack_Click = null;
    }
    public void Clear_A_Listenter()
    {
        if (UnPack_SkillA_Click != null)
        {
            UnPack_SkillA_Click();
            UnPack_SkillA_Click = null;
        }
        On_SkillA_Click = null;
    }
    public void Clear_B_Listenter()
    {
        if (UnPack_SkillB_Click != null)
        {
            UnPack_SkillB_Click();
            UnPack_SkillB_Click = null;
        }
        On_SkillB_Click = null;
    }
    
}
