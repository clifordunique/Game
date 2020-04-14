using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSnake : MonoBehaviour
{
    private enum BattleState
    {
        Stand = 0,
        Walk,
        Skilling
    }
    [SerializeField]
    BattleState battleState;
    Dictionary<string, bool> skillDic = new Dictionary<string, bool>();
    SkillState skillstate;
    Monster monster;
    Animator animator;

    [SerializeField]
    float shootCd, slaveCd, jumpslaveCd,flashCd;

    string sk1; //  shoot
    string sk2; //  slave
    string sk3; //  jumpSlave
    string sk4; //  flash
    string sk5; // drop
    bool prediectFlag;  // if Hp down to 50% then set this flag
    float monsterMaxHp;
    // Start is called before the first frame update
    void Start()
    {
        skillstate = gameObject.GetComponent<SkillState>();
        monster = gameObject.GetComponent<Monster>();
        MonsterInit();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (IsSkilling()){ /* wait until skill done */ }
        else
        {   //  the monster is moving or standing
            if (skillDic[sk5])
            {
                Coroutine reff = StartCoroutine(Drop());
            }
            else if (skillDic[sk1])
            {
                Coroutine reff = StartCoroutine(Shoot());
            }
            else if (skillDic[sk2] && Mathf.Abs(monster.disToPlayer) < 4)
            {
                Coroutine reff = StartCoroutine(Slave());
            }
            else if (skillDic[sk4])
            {
                Coroutine reff = StartCoroutine(Flash());
            }
            else if (skillDic[sk3])
            {
                Coroutine reff = StartCoroutine(JumpSlave());
            }
        }
        CheckTheState();
    }
    IEnumerator Shoot()
    {   /* 開始詠唱射擊 */
        SkillStart(sk1);
        monster.ForwardToPlayer();
        if (prediectFlag) skillstate.PreSkillPar01();
        skillstate.SetSkillPar01();
        monster.ChangeAnim("Shoot", 2, true);
        animator.SetFloat("color", skillstate.ski01.color);
        yield return new WaitForSeconds(0.4f);  //  詠唱結束
        Vector3 playerVec = monster.playerVec;    //  瞄準
        yield return new WaitForSeconds(0.6f);
        //  射出來
        skillstate.Shoot(playerVec);
        battleState = BattleState.Stand;
        yield return new WaitForSeconds(shootCd - 1f);
        skillDic[sk1] = true;   //  cool over
    }
    IEnumerator Slave()
    {   /* 開始預備砍 */
        SkillStart(sk2);
        monster.ForwardToPlayer();
        monster.ChangeAnim("Attack", 2, true);
        animator.SetFloat("color",skillstate.ski02.color);
        yield return new WaitForSeconds(0.7f);
        //  砍出來
        yield return StartCoroutine(skillstate.Slave(0.3f));
        battleState = BattleState.Stand;
        yield return new WaitForSeconds(slaveCd - 1f);
        skillDic[sk2] = true;   //  cool over
    }
    IEnumerator JumpSlave()
    {   /* 開始跳出去 */
        SkillStart(sk3);
        float deltaX;
        if (prediectFlag) deltaX = skillstate.PreSkillPar03(monster.playerPosition);
        else deltaX = skillstate.SetSkillPar03();
        monster.ForwardToPlayer();
        monster.ChangeAnim("JumpAttack", 2, true);
        animator.SetFloat("color", skillstate.ski03.color);
        StartCoroutine(MonPhysic.ChangeY(monster, 12, 5, 0.1f));
        Coroutine rref = StartCoroutine(MonPhysic.ChangeX(monster, deltaX / 0.9f, 5, 1.2f));
        /* 預備砍 */
        yield return new WaitForSeconds(0.7f);
        yield return rref;
        //  砍出來
        yield return StartCoroutine(skillstate.JumpSlave(0.3f));
        battleState = BattleState.Stand;
        yield return new WaitForSeconds(slaveCd - 2f);
        skillDic[sk3] = true;   //  cool over
    }
    IEnumerator Flash()
    {   /* 顯出下一個順宜地點 */
        SkillStart(sk4);
        Vector3 targetPosition;
        if (prediectFlag) targetPosition = skillstate.SetSkillPar04(monster.playerPosition);
        else targetPosition = skillstate.SetSkillPar04(monster.playerPosition);
        yield return new WaitForSeconds(1f);
        //  跑出來
        yield return StartCoroutine(skillstate.Flash());
        yield return new WaitForSeconds(0.8f);
        battleState = BattleState.Stand;
        yield return new WaitForSeconds(flashCd - 1.5f);
        skillDic[sk4] = true;   //  cool over
        skillDic[sk1] = true;   //  cool over
    }
    IEnumerator Drop()
    {
        SkillStart(sk5);
        skillstate.SetSkillPar05();     //  出現掉落物
        yield return new WaitForSeconds(0.6f);
        //  射出來
        skillstate.Drop();
        battleState = BattleState.Stand;
        yield return new WaitForSeconds(4f);
        skillDic[sk5] = true;   //  cool over
    }
    void SkillStart(string skillname)
    {
        //Debug.Log("use " + skillname);
        battleState = BattleState.Skilling;
        skillDic[skillname] = false;  //  enter to cooldown
    }
    bool IsSkilling()
    {
        return BattleState.Skilling == battleState;
    }
    void CheckTheState()
    {   //  check if hp is down to 50%
        prediectFlag = (monster.hp <= monsterMaxHp/2)?true:false;
    }
    void MonsterInit()
    {
        sk1 = "shoot";
        sk2 = "slave";
        sk3 = "jumpSlave";
        sk4 = "flash";
        sk5 = "drop";
        monsterMaxHp = monster.hp;
        prediectFlag = false;
        skillDic.Add(sk1, true);
        skillDic.Add(sk2, true);
        skillDic.Add(sk3, true);
        skillDic.Add(sk4, true);
        skillDic.Add(sk5, true);
    }
}
