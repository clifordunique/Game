using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Slim : MonsterAI_00
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

    public void death()
    {
        StartCoroutine(dieing());
    }
    protected virtual IEnumerator dieing()
    {
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        Color fade = new Color(0, 0, 0, 0.05f);
        while (spriteRenderer.color.a > 0)
        {
            spriteRenderer.color -= fade;
            yield return new WaitForSeconds(0.05f);
        }
        Debug.Log(this.name + " is going to be destroyed");
        Destroy(gameObject);
        yield return null;
    }
    void MonsterInit()
    {
    }
}
