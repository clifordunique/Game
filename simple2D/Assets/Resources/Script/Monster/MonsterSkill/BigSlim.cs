using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BigSlim : MonoBehaviour
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
    [SerializeField]
    private float skillDis; //  in this distance, monster will announce skill
    [SerializeField]
    private float speed,PlayerDis;
    [SerializeField]
    private float swordDistance, ImpactDistance;
    [SerializeField]
    float sk1Cooldown, sk2Cooldown, sk3Cooldown;
    [SerializeField]
    int maxHp;

    bool secondState;
    string sk1; //  Big sword
    string sk2; //  gogoP
    string sk3; //  rolling
    Controller2D controller;
    Monster monster;
    Player player;
    [SerializeField]
    LayerMask playerLayer, wallLayer;
    LayerMask LayerWhite, LayerBlack;
    private void Awake()
    {
        battleState = BattleState.Stand;
    }
    private void Start()
    {
        monster = gameObject.GetComponent<Monster>();
        GameObject temp = GameObject.FindGameObjectWithTag("Player");
        player = temp.GetComponent<Player>();
        controller = GetComponent<Controller2D>();
        MonsterInit();
    }
    // Update is called once per frame
    public void Update()
    {
        PlayerDis = CalPlayerDistance();
        if (BattleState.Skilling == battleState) { /* wait until skill done */}
        else
        {
            if (IsReadySkill())
            {
                //if (Input.GetKey(KeyCode.X) && skillDic[sk2])
                //{
                //    Coroutine reff = StartCoroutine(GogoPig());
                //}
                //if (Input.GetKey(KeyCode.V) && skillDic[sk1])
                //{
                //    Coroutine reff = StartCoroutine(BigSword());
                //}
                if (skillDic[sk1] && (PlayerDis < 5))
                {
                    Coroutine reff = StartCoroutine(BigSword());
                }
                else if (skillDic[sk2])
                {
                    Coroutine reff = StartCoroutine(GogoPig());
                }
                else if (skillDic[sk3])
                {
                    Coroutine reff = StartCoroutine(Rolling());
                }
            }
            else if (BattleState.Walk == battleState)
            {   // walk to player
                //if (PlayerDis > 3) 
                monster.ChangeVelocityX(monster.forward * speed, 5);
                if (monster.forward != Math.Sign(monster.playerVec.x))
                {
                    battleState = BattleState.Stand;
                    monster.ForwardToPlayer();
                }
            }
        }
        //  if hp < 50% open the second state
        if ((false == secondState) && HpIsBelowTheshold(monster.hp))
        {
            secondState = true;
            skillDic[sk3] = true;
        }
    }
    IEnumerator BigSword()
    {
        SkillStart(sk1);
        yield return null;
        /* raise the sword */
        bool black_white = (UnityEngine.Random.Range(-1, 1) >= 0)?true:false;
        Debug.Log("the color is " + black_white);
        float raiseTime = 1; // raise sword for one sec
        yield return StartCoroutine(BigSword_Raise(black_white, raiseTime));
        /* hit down */
        if (HpIsBelowTheshold(monster.hp))
        {   //Hp theshold to change the double sword
            yield return StartCoroutine(BigSword_Down_Double(black_white));
        }
        else
        {
            yield return StartCoroutine(BigSword_Down(black_white));
        }

        battleState = BattleState.Stand;
        yield return new WaitForSeconds(1.5f);
        battleState = BattleState.Walk;
        monster.ForwardToPlayer();
        yield return new WaitForSeconds(sk1Cooldown - 1.5f);
        skillDic[sk1] = true;   //  cool over
    }
    IEnumerator BigSword_Raise(bool black_white,float time)
    {   // false is white or true is black
        monster.ChangeVelocityX(0, 5);
        float curTime = Time.time;
        float endTime = Time.time + time;
        Vector2 rayOrigin = gameObject.transform.localPosition;
        Vector2 direction = Vector2.right * monster.forward + Vector2.up;
        while (Time.time < endTime)
        {
            rayOrigin = (monster.forward == -1) ? controller.raycastOrigins.topLeft : controller.raycastOrigins.topRight;
            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, direction, swordDistance, playerLayer);
            if (hit)
            {
                Debug.Log("raise and toutch the player");
            }
            Debug.DrawRay(rayOrigin, direction * swordDistance, Color.white);
            yield return null;
        }
    }
    IEnumerator BigSword_Down(bool isBlack)
    {   // false is white, true is black
        if (isBlack) {
            for (int i = 1; i < controller.horizontalRayCount; i++)
            {
                Vector2 rayOrigin = (monster.forward == -1) ? controller.raycastOrigins.bottomLeft : controller.raycastOrigins.bottomRight;
                rayOrigin += Vector2.up * controller.horizontalRaySpacing * i;
                Vector2 direction = Vector2.right * monster.forward;
                RaycastHit2D hit = Physics2D.Raycast(rayOrigin, direction, swordDistance, LayerBlack);
                Debug.DrawRay(rayOrigin, direction * swordDistance, Color.white);
                if (hit)
                {   /* black hit and give damage to the player */

                }
            }
        }
        else
        {
            for (int i = 1; i < controller.horizontalRayCount; i++)
            {
                Vector2 rayOrigin = (monster.forward == -1) ? controller.raycastOrigins.bottomLeft : controller.raycastOrigins.bottomRight;
                rayOrigin += Vector2.up * controller.horizontalRaySpacing * i;
                Vector2 direction = Vector2.right * monster.forward;
                RaycastHit2D hit = Physics2D.Raycast(rayOrigin, direction, swordDistance, LayerWhite);
                Debug.DrawRay(rayOrigin, direction * swordDistance, Color.white);
                if (hit)
                {   /* white hit and give damage to the player */

                }
            }
        }
        yield return new WaitForSeconds(1.0f);
    }
    IEnumerator BigSword_Down_Double(bool isBlack)
    {   // false is white or true is black
        if (isBlack)
        {
            for (int i = 0; i < controller.horizontalRayCount; i++)
            {
                Vector2 rayOriginL = controller.raycastOrigins.bottomLeft;
                rayOriginL += Vector2.up * controller.horizontalRaySpacing * i;
                RaycastHit2D hitL = Physics2D.Raycast(rayOriginL, Vector2.left, swordDistance, LayerBlack);
                Debug.DrawRay(rayOriginL, Vector2.left * swordDistance, Color.red);
                Vector2 rayOriginR = controller.raycastOrigins.bottomRight;
                rayOriginR += Vector2.up * controller.horizontalRaySpacing * i;
                RaycastHit2D hitR = Physics2D.Raycast(rayOriginR, Vector2.right, swordDistance, LayerBlack);
                Debug.DrawRay(rayOriginR, Vector2.right * swordDistance, Color.red);
                if (hitL || hitR)
                {   /* black hit and give damage to the player */

                }
            }
        }
        else
        {
            for (int i = 0; i < controller.horizontalRayCount; i++)
            {
                Vector2 rayOriginL = controller.raycastOrigins.bottomLeft;
                rayOriginL += Vector2.up * controller.horizontalRaySpacing * i;
                RaycastHit2D hitL = Physics2D.Raycast(rayOriginL, Vector2.left, swordDistance, LayerWhite);
                Debug.DrawRay(rayOriginL, Vector2.left * swordDistance, Color.red);
                Vector2 rayOriginR = controller.raycastOrigins.bottomRight;
                rayOriginR += Vector2.up * controller.horizontalRaySpacing * i;
                RaycastHit2D hitR = Physics2D.Raycast(rayOriginR, Vector2.right, swordDistance, LayerWhite);
                Debug.DrawRay(rayOriginR, Vector2.right * swordDistance, Color.red);
                if (hitL || hitR)
                {   /* white hit and give damage to the player */

                }
            }
        }
 
        Vector2 impactOrigin;
        if (-1 == monster.forward)
        {
            impactOrigin = controller.raycastOrigins.bottomLeft;
            impactOrigin.x -= swordDistance;
        }
        else
        {
            impactOrigin = controller.raycastOrigins.bottomRight;
            impactOrigin.x += swordDistance;
        }
        yield return new WaitForSeconds(1.0f);
    }
    IEnumerator GogoPig()
    {
        SkillStart(sk2);
        yield return null;
        monster.ChangeVelocity(new Vector2(6 * monster.forward, 9f), 5);
        yield return StartCoroutine(MonPhysic.ChangeX(monster,(5+ PlayerDis/2) * monster.forward, 5, 0.6f));
        yield return StartCoroutine(MonPhysic.ChangeY(monster,0, 5, 0.3f));
        monster.ChangeVelocityY(-10, 5);
        monster.ForwardToPlayer();
        yield return StartCoroutine(ReleaseImpact());
        battleState = BattleState.Walk;
        yield return new WaitForSeconds(sk2Cooldown);
        skillDic[sk2] = true;   //  cool over
    }
    IEnumerator Rolling()
    {
        SkillStart(sk3);
        yield return null;
        /* rolling accelerate */
        monster.ChangeVelocity(new Vector2(2 * monster.forward, 3f), 5);
        yield return new WaitForSeconds(1.0f);
        /* faster and faster */
        yield return StartCoroutine(MonPhysic.ChangeX(monster, (7 + PlayerDis / 2) * monster.forward, 5, 0.8f));
        if (IsHitTheWall())
        {   /* hit the wall, stun for few seconds */
            yield return new WaitForSeconds(5.0f);
        }
        battleState = BattleState.Walk;
        yield return new WaitForSeconds(sk2Cooldown);
        skillDic[sk3] = true;   //  cool over
    }
    bool IsReadySkill()
    {
        if (PlayerDis < skillDis && skillDic.ContainsValue(true))
        {   //  skill constract we acheive
            return true;
        }
        return false;
    }
    void SkillStart(string skillname)
    {
        //Debug.Log("use " + skillname);
        battleState = BattleState.Skilling;
        skillDic[skillname] = false;  //  enter to cooldown
    }
    void CalAttackRatio()
    {
        float ratio = gameObject.transform.localScale.x;    //  get the scale
        swordDistance *= ratio;
        ImpactDistance *= ratio;
    }
    float CalPlayerDistance()
    {
        Vector3 temp = monster.FindPlayerPostition();
        return Vector3.Distance(temp, gameObject.transform.position);
    }
    IEnumerator ReleaseImpact()
    {
        while (false == monster.monsterState.isGround)
        {
            yield return null;
        }
        Vector2 rayOriginLeft = controller.raycastOrigins.bottomLeft;
        Vector2 rayOriginRight = controller.raycastOrigins.bottomRight;
        RaycastHit2D hitL = Physics2D.Raycast(rayOriginLeft, Vector2.left, ImpactDistance, playerLayer);
        Debug.DrawRay(rayOriginLeft, Vector2.left * ImpactDistance, Color.white);
        RaycastHit2D hitR = Physics2D.Raycast(rayOriginRight, Vector2.right, ImpactDistance, playerLayer);
        Debug.DrawRay(rayOriginRight, Vector2.right * ImpactDistance, Color.white);
        if (hitL || hitR)
        {
            Debug.Log("The impact hit the player");
        }

    }
    bool HpIsBelowTheshold(int curHp)
    {
        if (curHp < maxHp / 2) return true;
        return false;
    }
    bool IsHitTheWall()
    {
        for (int i = 1; i < controller.horizontalRayCount; i++)
        {
            Vector2 rayOrigin = controller.raycastOrigins.bottomLeft;
            rayOrigin += Vector2.up * controller.horizontalRaySpacing * i;
            Vector2 direction = Vector2.right * monster.forward;
            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, direction, 0.3f, wallLayer);
            Debug.DrawRay(rayOrigin, direction * swordDistance, Color.white);
            if (hit)
            {   /* hit and give damage to the player */
                return true;
            }
        }
        return false;
    }
    void MonsterInit()
    {
        sk1Cooldown = 3;
        sk2Cooldown = 5;
        sk1 = "BigSword";
        sk2 = "BigJump";
        sk3 = "Rolling";
        skillDic.Add(sk1, true);
        skillDic.Add(sk2, true);
        skillDic.Add(sk3, false);
        CalAttackRatio();
        maxHp = monster.hp;
        secondState = false;
        playerLayer |= 1 << LayerMask.NameToLayer("Player_w");
        playerLayer |= 1 << LayerMask.NameToLayer("Player_b");
        wallLayer = 1 << LayerMask.NameToLayer("Obstacle");
        LayerWhite |= 1 << LayerMask.NameToLayer("Player_w");
        LayerBlack |= 1 << LayerMask.NameToLayer("Player_b");
    }
}