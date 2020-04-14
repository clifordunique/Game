using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Flying : MonsterAI_03
{
    private void Start()
    {
        //MonsterInit();
    }

    // Update is called once per frame
    public override void Update()
    {
        base.Update();
        if(AI_State.Battle == ai)
        {   /* do battle */
            monster.ChangeVelocityX(monster.forward * speed, 5);
            if(monster.forward != Math.Sign(monster.playerVec.x))
            {
                StartCoroutine(ChangeSlow());
            }
        }
    }
    private void Battle()
    {

    }

    public void SlimJump()
    {
        //if (moveable) {
        //    this.StartCoroutine(JumpAttack());
        //}
        int forward = monster.forward;
        monster.velocity.y = 4; // jump force for the slim
        monster.velocity.x = 3;
    }
    IEnumerator JumpAttack()
    {
        yield return null;
    }
    IEnumerator ChangeSlow()
    {
        yield return new WaitForSeconds(1);
        monster.ForwardToPlayer();
        yield return null;
    }
    void MonsterInit()
    {
    }
}
