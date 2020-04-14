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
[RequireComponent(typeof(HitableObj))]
public class Monster : MonoBehaviour
{
    public float moveSpeed;
    public int hp, mp, damage, def;
    public float gravity = -10;
    public Vector3 velocity;
    public Vector3 playerVec;  /*  it return the pointer to player */
    public Vector3 playerPosition;  /*  it return the pointer to player */
    [SerializeField]
    public float disToPlayer;
    public Vector3 bornPosition;
    public int forward; //  1 is Right,up
    public MonsterState monsterState;

    Controller2D controller;
    private GameObject player;
    private int velWeight;
    private Animator animator;
    private int animWeight;

    public HealthSystem healthSystem;
    public Transform pfHealthBar;
    public Boolean showHP = false;
    public Transform healthBarTransform;

    private TmpAttackCnt attackCnt;

    void Start()
    {
        controller = GetComponent<Controller2D>();
        animator = GetComponent<Animator>();
        player = GameObject.FindWithTag("Player");
        GetComponent<HitableObj>().getHit.AddListener(GetHurt);
        attackCnt = GetComponent<TmpAttackCnt>();
        
        //velocity.x = 0;
        Init();
    }
    // Update is called once per frame
    private void Update()
    {
        UpState();
        velocity.y += gravity * Time.deltaTime;
        playerVec = FindPlayerVec();
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
    public void death()
    {
        Debug.Log(this.name + "died (Monster");
        Destroy(transform.GetChild(0).gameObject);
        StopAllCoroutines();
        gameObject.layer = LayerMask.NameToLayer("Enemy_dying");
        if (MonsterDictionary.MonsterDic.ContainsKey(this.name))
        {
            Type t = MonsterDictionary.MonsterDic[this.name];
            Component c = gameObject.GetComponent(t);
            if (c == null)
            {
                Debug.Log("gameObject.GetComponent(t) is null");
                StartCoroutine(NormalDieing());
                return;
            }
            if (t.GetMethod("death") != null) StartCoroutine(NormalDieing());
            else t.GetMethod("death").Invoke(c, null);
        }
        else
        {
            StartCoroutine(NormalDieing());
        }
    }
    private Vector3 FindPlayerVec()
    {
        /*  it return the pointer to player */
        playerPosition = player.transform.position;
        Vector3 temp = player.transform.position - gameObject.transform.position;
        disToPlayer = temp.magnitude;
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
        //forward = Math.Sign(playerPosition.normalized.x);
        ChangeForward(Math.Sign(playerVec.normalized.x));
        return forward;
    }
    public void ChangeAnim(string name, int weight, bool wait)
    {
        if (weight >= animWeight)
        {
            if (wait)
            {
                StartCoroutine(WaitForAnim(name, animWeight));
            }
            animWeight = weight;
            animator.Play(name);
            if (!animator.HasState(0, Animator.StringToHash(name)))
            {
                Debug.Log(this.name + " cant find animation: " + name);
            } 
        }
    }
    private IEnumerator WaitForAnim(string name, int OriginWeight)
    {
        yield return null;
        while (animator.GetCurrentAnimatorStateInfo(0).IsName(name))
        {
            yield return null;
        }
        animWeight = OriginWeight;
    }
    public void ChangeForward(int toward)
    {
        if (toward != forward)
        {
            transform.localScale = new Vector2(-transform.localScale.x, transform.localScale.y);
            if (healthBarTransform)
            {
                healthBarTransform.transform.localScale = new Vector3(-healthBarTransform.transform.localScale.x, healthBarTransform.transform.localScale.y, healthBarTransform.transform.localScale.z);
            }
        }
        forward = toward;
    }

    public void ChangeVelocity(Vector2 vec,int weight)
    {
        if (weight > velWeight)
        {
            velocity = vec;
            velWeight = weight;
        }
        else
        {
            Debug.Log("cur weight is higher " + velWeight + "than " + weight);
        }
    }
    public void ChangeVelocityX(float velX, int weight)
    {
        if (weight > velWeight)
        {
            velocity.x = velX;
            velWeight = weight;
        }
        else
        {
            //Debug.Log("changeX speed reject, cur weight " + velWeight + " higher than" + weight);
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
    public void GetHurt(int damage)
    {
        if (showHP == false)
        {
            HealthBar();
            showHP = true;
        }
        if (damage == 10) attackCnt.AttackCount(0);
        else attackCnt.AttackCount(1);
        int minusHp = DamageSystem.CalculateRealDamage(damage, def);
        hp -= minusHp;
        healthSystem.Damage(minusHp);
        Debug.Log(healthSystem.GetHealth());
        if (hp <= 0)
        {
            death();
        }
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

    private void HealthBar()
    {
        healthSystem = new HealthSystem(hp);
        healthBarTransform = Instantiate(pfHealthBar, new Vector3(0, 10), Quaternion.identity);
        healthBarTransform.transform.parent = this.transform;
        float height = this.GetComponent<Renderer>().bounds.size.x;
        healthBarTransform.transform.position = new Vector3(this.transform.position.x, this.transform.position.y + height/2 + 0.2f, this.transform.position.z);

        HealthBar healthBar = healthBarTransform.GetComponent<HealthBar>();
        healthBar.Setup(healthSystem);
    }
    IEnumerator NormalDieing()
    {
        Debug.Log("death");
        ChangeAnim("death", 2, false);
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        Color fade = new Color(0, 0, 0, 0.1f);
        yield return null;
        float interval = animator.GetCurrentAnimatorStateInfo(0).length / 10;
        while (spriteRenderer.color.a > 0)
        {
            spriteRenderer.color -= fade;
            yield return new WaitForSeconds(interval);
        }
        Debug.Log(this.name + " is going to be destroyed");
        Destroy(gameObject);
        yield return null;
    }
}
