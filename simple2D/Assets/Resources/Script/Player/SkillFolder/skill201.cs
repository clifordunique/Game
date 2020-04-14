using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class skill201 : MonoBehaviour
{
    private void Awake()
    {
        SubscribeSkill();
        SubUnpack();
    }
    protected void SubscribeSkill()
    {   //  notice to subscribe witch event
        PlayerSkill.On_SkillB_Click += skill;
        Debug.Log("skill 201 is listen to B");
    }
    protected void SubUnpack()
    {
        PlayerSkill.UnPack_SkillB_Click += DestroyTheClass;
    }
    protected void DestroyTheClass()
    {
        Destroy(this);
    }

    //  the main attack way
    public IEnumerator skill()
    {
        if (Player.Instance.state.isGround)
        {
            Player.Instance.state.canControll = false;
            yield return FlashForward(3.5f);
            yield return new WaitForSeconds(0.5f);  //  postpone for skill
            Player.Instance.state.canControll = true;
        }
        /* instatiate the skill001.cs
         * and but we need to cancel other listener;
         */
    }
    public IEnumerator FlashForward(float distance)
    {
        Vector3 position = Player.Instance.gameObject.transform.position;
        position.x += distance * Player.Instance.forward;
        Player.Instance.gameObject.transform.position = position;
        yield return null;
    }
    //  end

}
