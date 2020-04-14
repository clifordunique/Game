using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class GarbageCan : MonsterAI_00
{
    MonSkill monskill = new MonSkill();
    [SerializeField]
    float throwCooldown, rollingCooldown;
    [SerializeField]
    GameObject garbage;


    private void Start()
    {
        monskill.SetState(MonSkill.BattleState.Walk);
        MonsterInit();
        monskill.AddSkill("Rolling");
        monskill.AddSkill("Throw");
    }
    // Update is called once per frame
    public override void Update()
    {
        base.Update();
        if (AI_State.Battle == ai)
        {
            if (MonSkill.BattleState.Skilling == monskill.battleState) { /* wait until skill done */}
            else
            {
                if (IsReadySkill())
                {   // Throw , Rolling
                    if (CondiForThrow())
                    {
                        Coroutine reff = StartCoroutine(Throw());
                    }else if (CondiForRolling())
                    {
                        Coroutine reff = StartCoroutine(Rolling());
                    }
                }
                if (MonSkill.BattleState.Walk == monskill.battleState)
                {   // walk to player
                    monster.ChangeAnim("walk",1,true);
                    monster.ChangeVelocityX(monster.forward * speed, 2);
                    if (monster.forward != Math.Sign(monster.playerVec.x))
                    {
                        StartCoroutine(ChangeSlow());
                    }
                }
                else if (MonSkill.BattleState.Stand == monskill.battleState)
                {
                    monster.ChangeAnim("stand", 0, false);
                    monster.ForwardToPlayer();
                    monster.ChangeVelocityX(0, 2);
                }
            }
        }
    }
    IEnumerator ChangeSlow()
    {
        yield return new WaitForSeconds(1);
        monster.ForwardToPlayer();
        yield return null;
    }
    IEnumerator Throw()
    {
        monster.ChangeAnim("throw", 1, true);
        monskill.StartSkill("Throw");
        /* prepare to throw the garbage */
        monskill.SetState(MonSkill.BattleState.Stand);
        yield return new WaitForSeconds(0.2f);
        Vector2 position = gameObject.transform.position;
        position.x += (monster.forward == -1) ? -0.8f : 0.8f;   /* the revision */
        GameObject temp = Instantiate(garbage, position, Quaternion.identity);
        InitForGarbage(temp,monster.playerVec);
        yield return new WaitForSeconds(1);
        monskill.SetState(MonSkill.BattleState.Walk);
        yield return new WaitForSeconds(throwCooldown - 2.5f);
        monskill.EndSkill("Throw");
    }
    IEnumerator Rolling()
    {
        monster.ChangeAnim("roll", 1, true);
        monskill.StartSkill("Rolling");
        yield return null;
        monster.ChangeVelocity(new Vector2(6 * monster.forward, 1), 5);
        yield return StartCoroutine(ChangeX(6 * monster.forward, 5, 0.7f));
        monster.ForwardToPlayer();
        monskill.SetState(MonSkill.BattleState.Walk);
        yield return new WaitForSeconds(rollingCooldown);
        monskill.EndSkill("Rolling");
    }
    IEnumerator ChangeX(float speed, float weight, float time)
    {
        float curTime = Time.time;
        float endTime = Time.time + time;
        while (Time.time < endTime)
        {
            monster.ChangeVelocityX(speed, 5);
            yield return null;
        }
        monster.ChangeVelocityX(0, 5);
    }
    IEnumerator ChangeY(float speed, float weight, float time)
    {
        float curTime = Time.time;
        float endTime = Time.time + time;
        while (Time.time < endTime)
        {
            monster.ChangeVelocityY(speed, 5);
            yield return null;
        }
    }
    bool IsReadySkill()
    {
        if (monskill.IsReadyForAll())
        {   //  skill constract we acheive
            return true;
        }
        return false;
    }
    bool CondiForThrow()
    {
        if (monskill.IsReady("Throw") && monster.disToPlayer > 1.5f)
        {
            Vector2 ForVec = (monster.forward == -1) ? Vector2.left : Vector2.right;
            if(Vector2.Angle(monster.playerVec, ForVec) < 50)return true;
            return false;
        }
        return false;
    }
    bool CondiForRolling()
    {
        if(monskill.IsReady("Rolling") && monster.disToPlayer < 4)return true;
        return false;
    }
    void InitForGarbage(GameObject temp,Vector2 forward)
    {
        forward.y -= 0.05f;  /* the revision */
        garbageCanSkill garbage = temp.GetComponent<garbageCanSkill>();
        garbage.SetInit(forward);
    }
    void MonsterInit()
    {
        throwCooldown = 4;
        rollingCooldown = 10;
    }
}
