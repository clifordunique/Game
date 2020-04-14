using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[RequireComponent(typeof(BoxCollider2D))]
public class MonsterAI_00 : MonoBehaviour
{
    protected enum AI_State
    {
        Stand = 0,
        Walk,
        Warning,
        Battle,
        Dieing,
        Return
    }

    private Coroutine state;
    protected Monster monster;  // access the monster script
    protected Vector3 position; // record monster's position;
    protected float distance;   // record the dis with spawn point
    [SerializeField]
    protected float reDistance = 10;    //  record the max battle distance
    [SerializeField]
    protected float standTime, walkTime, speed , reationTime = 1;
    [SerializeField]
    protected AI_State ai = AI_State.Stand;
    protected Controller2D controller;
    SpriteRenderer spriteRenderer;  

    public Vector3 spawnPosition;   //  record the spawn point

    private bool isFirst;
    RaycastHit2D hit;
    [SerializeField]
    float seeDistance = 0;  //  the monster's max sight distance
    [SerializeField]
    LayerMask collisionMask, collisionMaskForPLayer;
    //LayerMask collisionMaskForPLayer;
    private void Awake()
    {
        Init();
    }
    private void Start()
    {

    }
    public virtual void Update()
    {
        if (isFirst)
        {
            isFirst = false;
            switch (ai)
            {
                case (AI_State.Stand):
                    state = StartCoroutine(Stand(standTime));
                    break;
                case (AI_State.Walk):
                    state = StartCoroutine(Walk(walkTime));
                    break;
                case (AI_State.Warning):
                    state = StartCoroutine(Warning(reationTime));
                    break;
                case (AI_State.Battle):
                    break;
                case (AI_State.Dieing):
                    state = StartCoroutine(dieing());
                    break;
                case (AI_State.Return):
                    state = StartCoroutine(Return());
                    //Return();
                    break;
            }
        }
        position = gameObject.transform.position;
        if (AI_State.Battle == ai) CheckDistance(position);
        else if(AI_State.Stand == ai || AI_State.Walk == ai)
        {  //  monster will double check if player is in when monster is in warning 
            LookAround();
            if (monster.monsterState.isHurt)
            {   //  if monster is hurt, go into battle state
                StopAllCoroutines();
                TurnToState(AI_State.Battle);
            }
        }
        if (EdgeDetect()) monster.ChangeVelocityX(0, 7);
        if (MonPhysic.TouchPlayer(controller, monster))
        {   //  if monster touch the player
            if (AI_State.Battle != ai)
            {
                StopAllCoroutines();
                TurnToState(AI_State.Battle);
            }
        }
    }
    public void death()
    {
        StopAllCoroutines();
        gameObject.layer = 22;
        TurnToState(AI_State.Dieing);
    }
    protected virtual IEnumerator dieing()
    {
        while (spriteRenderer.color.a > 0)
        {
            //Debug.Log(spriteRenderer.color.a);
            spriteRenderer.color = spriteRenderer.color - new Color(0, 0, 0, 0.05f);
            //yield return new WaitForSeconds(0.05f);
            yield return null;
        }
        Debug.Log(this.name + " is going to be destroyed");
        //Destroy(gameObject);
        yield return null;
    }

    protected virtual IEnumerator Stand(float standTime)
    {
        //Debug.Log("We enter to the Stand state");
        monster.ChangeVelocityX(0,1);
        yield return new WaitForSeconds(standTime);
        //monster.forward *= (-1);
        monster.ChangeForward(monster.forward * -1);
        TurnToState(AI_State.Walk);
    }
    protected virtual IEnumerator Walk(float walkTime)
    {
        Coroutine refeee = StartCoroutine(MoveForward());
        //Debug.Log("We enter to the Walk state");
        yield return new WaitForSeconds(walkTime);
        StopCoroutine(refeee);
        monster.ChangeVelocityX(0, 1);
        TurnToState(AI_State.Stand);
    }
    protected virtual IEnumerator Return()
    {   //  in this script just check the x position
        Debug.Log("We enter to the Return state");
        float toward = spawnPosition.x - gameObject.transform.position.x;
        //monster.forward = (int)Mathf.Sign(toward);
        monster.ChangeForward((int)Mathf.Sign(toward));
        Coroutine refeee = StartCoroutine(GoToSpawnPoint(toward,spawnPosition));
        yield return null;
    }
    protected virtual IEnumerator Warning(float warningTime)
    {
        Debug.Log("We enter to the Warning state");
        float warningSeeDis = seeDistance * 1.5f;   //  increase see distance
        monster.ChangeVelocityX(0,1) ;
        yield return new WaitForSeconds(reationTime);   // wait for the monster react
        Vector2 origin = gameObject.transform.position;
        Vector2 dir = Vector2.right * monster.forward;
        hit = Physics2D.Raycast(origin, dir, warningSeeDis, collisionMaskForPLayer);
        Debug.DrawRay(origin, dir * warningSeeDis, Color.yellow);
        if (hit)
        {
            Debug.Log(hit.collider.gameObject.name);
            StopAllCoroutines();
            TurnToState(AI_State.Battle);
        }
        else
        {
            TurnToState(AI_State.Stand);
        }
        yield return null;
    }

    protected void TurnToState(AI_State ai_state)
    {
        StopCoroutine(state);
        //Debug.Log("Turn To State (" + ai_state + ")");
        isFirst = true;
        ai = ai_state;
    }

    private bool CheckDistance(Vector3 pos)
    {
        distance = Vector3.Distance(pos, spawnPosition);
        if (distance > reDistance)
        {
            TurnToState(AI_State.Return);
            return true;
        }
        return false;
    }
    private IEnumerator GoToSpawnPoint(float toward,Vector3 targetPosition)
    {   // it just dectect the x direction
        while (Mathf.Sign(toward) == monster.forward )
        {
            monster.ChangeVelocityX(Mathf.Sign(toward) * speed * 3, 9);
            yield return null;
            toward = spawnPosition.x - position.x;
            //hit = Physics2D.Raycast(origin, dir, seeDistance, collisionMaskForPLayer);
        }
        Debug.Log("monster back to the spawn point");
        TurnToState(AI_State.Stand);
        yield return null;
    }
    private IEnumerator MoveForward()
    {
        while (true)
        {
            float velocityX = monster.forward * speed;
            float tempSpeed = (EdgeDetect() == true) ? 0 : velocityX;
            monster.ChangeVelocityX(tempSpeed, 1);
            yield return null;
        }
    }
    private bool EdgeDetect()
    {
        RaycastHit2D hitF;
        RaycastHit2D hitB;
        Vector2 originForward = (monster.forward == 1) ? controller.raycastOrigins.bottomRight : controller.raycastOrigins.bottomLeft;
        Vector2 originBackward = (controller.raycastOrigins.bottomLeft + controller.raycastOrigins.bottomRight) / 2;
        Vector2 dir = Vector2.down;
        hitF = Physics2D.Raycast(originForward, dir, 10, collisionMask);    // 10 is the length to detect ground
        hitB = Physics2D.Raycast(originBackward, dir, 10, collisionMask);
        if (hitF.collider.name == hitB.collider.name)
        {
            monster.monsterState.isClosedToEdge = false;
            return false;
        }
        else
        {
            monster.monsterState.isClosedToEdge = true;
            return true;
        }
    }
    private void LookAround()
    {
        Vector2 origin = gameObject.transform.position;
        Vector2 dir = Vector2.right * monster.forward;
        hit = Physics2D.Raycast(origin, dir, seeDistance, collisionMaskForPLayer);
        Debug.DrawRay(origin, dir * seeDistance, Color.yellow);
        if (hit)
        {
            StopAllCoroutines();
            TurnToState(AI_State.Warning);
        }
    }
    void Init()
    {
        monster = gameObject.GetComponent<Monster>();
        controller = gameObject.GetComponent<Controller2D>();
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        position = gameObject.transform.position;
        spawnPosition = position;
        isFirst = true;
        reationTime = 1.0f;
        collisionMask |= (1 << LayerMask.NameToLayer("Platform"));
        collisionMask |= (1 << LayerMask.NameToLayer("Obstacle"));
        collisionMaskForPLayer |= (1 << LayerMask.NameToLayer("Player_w"));
        collisionMaskForPLayer |= (1 << LayerMask.NameToLayer("Player_b"));
        monster.ChangeVelocityX(0,0);
    }
}