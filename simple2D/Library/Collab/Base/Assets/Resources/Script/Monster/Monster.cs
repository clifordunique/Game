/*
 * this is the base Monster script
 * this script indicate monster's base Info like Hp, attack power
 * question: how to acess skill data from diffrent C#
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

//[RequireComponent(typeof(MonsterInfo))]
[RequireComponent(typeof(Controller2D))]
public class Monster : MonoBehaviour
{
    public struct MonsterState
    {
        public bool isAttack;
        public bool isClosedToEdge;
        public bool isGround;
        public bool isHurt;
        public bool isWall;
    };

    public float moveSpeed;
    public int hp, mp, damage;
    public float gravity = -10;
    public Vector3 velocity;
    public Vector3 playerPosition;  /*  it return the pointer to player */
    public Vector3 bornPosition;
    public int forward; //  1 is Right,up
    public MonsterState monsterState;

    Controller2D controller;
    private GameObject player;
    private int velWeight;

    public void death()
    {
        Debug.Log(this.name + "died (Monster.cs");
        Type t = MonsterDictionary.MonsterDic["Slim"];
        Component c = gameObject.GetComponent(t);
        t.GetMethod("death").Invoke(c, null);
        //tmp.death();
    }

    void Awake()
    {
        controller = GetComponent<Controller2D>();
        player = GameObject.FindWithTag("Player");
        //velocity.x = 0;
        Init();
    }
    // Update is called once per frame
    private void Update()
    {
        UpState();
        velocity.y += gravity * Time.deltaTime;
        playerPosition = FindPlayerVec();
        if (controller.collisionInfo.resetFlag) { velocity.y = 0; }
        if (monsterState.isHurt) ForwardToPlayer();
        Debug.DrawRay(gameObject.transform.position, Vector3.right * forward, Color.blue);
    }
    private void UpState()
    {
        monsterState.isGround = controller.collisionInfo.isGround;
        velWeight = 0;  //reset the velocity weight
    }
    void LateUpdate()
    {   // move when other script change the velocity
        controller.Move(velocity * Time.deltaTime);
    }
    private Vector3 FindPlayerVec()
    {
        /*  it return the pointer to player */
        Vector3 temp = player.transform.position - gameObject.transform.position;
        temp = temp.normalized;
        Debug.DrawRay(gameObject.transform.position, temp, Color.red);
        return temp;
    }
    public Vector3 FindPlayerPostition()
    {   // be cal in Boss.cs
        return player.transform.position;
    }
    public int ForwardToPlayer()
    {
        forward = Math.Sign(playerPosition.normalized.x);
        return forward;
    }

    public void ChangeVelocity(Vector2 vec,int weight)
    {
        if (weight > velWeight)
        {
            velocity = vec;
            velWeight = weight;
        }
    }
    public void ChangeVelocityX(float velX, int weight)
    {
        if (weight > velWeight)
        {
            velocity.x = velX;
            velWeight = weight;
        }
    }
    public void ChangeVelocityY(float velY, int weight)
    {
        if (weight > velWeight)
        {
            velocity.y = velY;
            velWeight = weight;
        }
    }
    public void GetHurt()
    {
        hp -= 10;
        if (false== monsterState.isHurt)StartCoroutine(ChangeHurtState());

    }
    IEnumerator ChangeHurtState()
    {
        int KbTime = 20;
        monsterState.isHurt = true;
        for (int i = 0; i < KbTime; i++)
        {   // KBTime controll the interative time
            ChangeVelocityX(0, 3);
            yield return null;
        }
        monsterState.isHurt = false;
        yield return null;
    }

    private void Init()
    {
        monsterState.isClosedToEdge = false;
        monsterState.isAttack = false;
        monsterState.isHurt = false;
        forward = 1;
        velWeight = 0;
    }
}
