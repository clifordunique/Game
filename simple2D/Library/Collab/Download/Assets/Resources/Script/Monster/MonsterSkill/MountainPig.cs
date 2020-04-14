using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MountainPig : MonsterAI_01
{
    public enum BattleState
    {
        Stand = 0,
        Walk,
        Skilling
    }
    [SerializeField]
    public BattleState battleState;
    Dictionary<string, bool> skillDic = new Dictionary<string, bool>();
    [SerializeField]
    private float skillDis; //  in this distance, monster will announce skill
    [SerializeField]
    float dodgeCooldown;
    [SerializeField]
    float GogoCooldown;
    private void Awake()
    {
        battleState = BattleState.Walk;
        skillDic.Add("GogoPig", true);
        skillDic.Add("Dodge", true);
    }

    // Update is called once per frame
    public override void Update()
    {
        base.Update();
        if (AI_State.Battle == ai)
        {
            if (BattleState.Skilling == battleState) { /* wait until skill done */}
            else
            {
                if (IsReadySkill())
                {
                    //if (Input.GetKey(KeyCode.X) && skillDic["GogoPig"])
                    //{
                    //    Coroutine reff = StartCoroutine(GogoPig());
                    //}
                    if (skillDic["Dodge"] && monster.monsterState.isHurt)
                    {
                        Coroutine reff = StartCoroutine(Dodge());
                    }
                    else if (skillDic["GogoPig"])
                    {
                        Coroutine reff = StartCoroutine(GogoPig());
                    }
                }
                if(BattleState.Walk == battleState)
                {   // walk to player
                    Move();
                    if (monster.forward != Math.Sign(monster.playerPosition.x))
                    {
                        StartCoroutine(ChangeSlow());
                    }
                }else if (BattleState.Stand == battleState)
                {
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
    IEnumerator Dodge()
    {
        Debug.Log("we dodge");
        battleState = BattleState.Skilling;
        skillDic["Dodge"] = false;  //  enter to cooldown
        yield return null;
        monster.ChangeVelocity(new Vector2((-6)* monster.forward, 2.5f), 5);
        yield return StartCoroutine(MonPhysic.ChangeX(monster,(-6) * monster.forward, 5, 0.3f));
        battleState = BattleState.Stand;
        yield return new WaitForSeconds(1.5f);
        battleState = BattleState.Walk;
        yield return new WaitForSeconds(dodgeCooldown - 1.5f);
        skillDic["Dodge"] = true;   //  cool over
    }
    IEnumerator GogoPig()
    {
        Debug.Log("we Gogo");
        battleState = BattleState.Skilling;
        skillDic["GogoPig"] = false;  //  enter to cooldown
        yield return null;
        monster.ChangeVelocity(new Vector2(6 * monster.forward, 4f), 5);
        yield return StartCoroutine(MonPhysic.ChangeX(monster,6 * monster.forward, 5, 0.7f));
        monster.ForwardToPlayer();
        battleState = BattleState.Walk;
        yield return new WaitForSeconds(GogoCooldown);
        skillDic["GogoPig"] = true;   //  cool over
    }
    private void Move()
    {
        float velocityX = monster.forward * speed;
        if (EdgeDetect() == true)
        {
            Vector3 vecToSpawm = spawnPosition - position;
            if (Mathf.Sign(vecToSpawm.x) == monster.forward)
            {
                monster.ChangeVelocityX(velocityX, 8);
            }
        }
        else monster.ChangeVelocityX(velocityX, 2);
    }
    bool IsReadySkill()
    {
        if (monster.disToPlayer < skillDis && skillDic.ContainsValue(true))
        {   //  skill constract we acheive
            return true;
        }
        return false;
    }
    void MonsterInit()
    {
        skillDis = 5;
        GogoCooldown = 3;
        dodgeCooldown = 5;
    }
}