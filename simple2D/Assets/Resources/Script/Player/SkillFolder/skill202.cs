using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class skill202 : MonoBehaviour
{
    private void Awake()
    {
        SubscribeSkill();
        SubUnpack();
    }
    protected void SubscribeSkill()
    {   //  notice to subscribe witch event
        PlayerSkill.On_SkillB_Click += skill;
        Debug.Log("skill 202 is listen to B");
    }
    protected void SubUnpack()
    {
        PlayerSkill.UnPack_SkillB_Click += DestroyTheClass;
    }
    protected void DestroyTheClass()
    {
        Destroy(this);
    }
    public IEnumerator skill()
    {
        Player.Instance.state.canControll = false;
        yield return FlyUp(5);
        yield return new WaitForSeconds(0.3f);  //  postpone for skill
        Player.Instance.state.canControll = true;
    }
    public IEnumerator FlyUp(float power)
    {
        Player.Instance.velocity.y = power;
        Player.Instance.SetToBaseGravity();
        yield return null;
    }
}
