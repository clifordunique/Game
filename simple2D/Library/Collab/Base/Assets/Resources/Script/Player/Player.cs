using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI; // Required when Using UI elements.

[RequireComponent(typeof(Controller2D))]

public class Player : Singleton<Player>
{
    public List<Buff> buff = new List<Buff>();  //  buff will clear initially
    public float gravity = -10;
    public int hp, mp;
    public int atk, def;
    public int forward;    // 1 is right, -1 is left
    public float moveSpeed;
    public float jumpForce;
    public float attackLength;
    public int maxCombo;
    public bool colorState; // false is white or true is black
    public Vector3 velocity;

    /* for enemy layer */
    public LayerMask layerForMonster;

    public Controller2D controller;
    public State state;

    public HealthSystem healthSystem;
    public Image HpBar;

    void Awake()
    {
        controller = GetComponent<Controller2D>();
        Init();
        HealthBar();
        HandleHp();
    }
    private void Update()
    {
        Upstate();
        UpdateBuff();
        if (controller.collisionInfo.resetFlag)
        {
            /* if player hit the ceiling or other object */
            velocity.y = 0;
            velocity.x = 0;
        }
        if (state.canControll)
        {   // sometime we can't controll. EX: attacking when jumping
            velocity.x = 0;
        }
        if (state.canControll)
        {
            if (Input.GetKeyDown(KeyBoardInput.Instance.change))
            {
                ChangeColor();
            }
            if (Input.GetKey(KeyBoardInput.Instance.right))
            {
                velocity.x = 1 * moveSpeed;
                forward = 1;
            }
            if (Input.GetKey(KeyBoardInput.Instance.left))
            {
                velocity.x = -1 * moveSpeed;
                forward = -1;
            }
            if (controller.collisionInfo.below)
            {
                if (Input.GetKeyDown(KeyBoardInput.Instance.up))
                {
                    velocity.y = jumpForce;
                }
            }
            // test
            if (Input.GetKey(KeyBoardInput.Instance.down))
            {
                Buff effect = new Buff();
                effect.type = BuffType.Slow;
                effect.Amount = 0.4f;
                effect.RemainTime = 1.0f;
                if (Buff.PlayerGetBuff(effect)) Debug.Log("Add buff slow");
            }
            if (Input.GetKey(KeyCode.S))
            {
                SavePlayer();
            }
            if (Input.GetKey(KeyCode.L))
            {
                LoadPlayer();
            }

        }
        if (state.canRigidbody)
        {
            velocity.y += gravity * Time.deltaTime;
        }
        //  if (Input.GetKey(KeyCode.R)) PlayerInfo.ReFlashInfo();

        if (Input.GetKey(KeyCode.Q))
        {
            healthSystem.Damage(10);
            Debug.Log(healthSystem.GetHealth());
            HandleHp();
        }
    }
    void LateUpdate()
    {   // move when other script change the velocity
        controller.Move(velocity * Time.deltaTime);
    }

    private void Init()
    {
        buff.Clear();
        state.canControll = true;
        state.canRigidbody = true;
        state.isSkilling = true;
        state.comboAuto = maxCombo;
        state.isGround = true;
    }
    private void Upstate()
    {
        state.isGround = controller.collisionInfo.isGround;
    }
    private void UpdateBuff()
    {
        for(int i = 0; i < buff.Count; i++)
        {
            buff[i].RemainTime -= Time.deltaTime;
            if(0 > buff[i].RemainTime)
            {
                Buff.PlayerReleaseBuff(buff[i]);
            }
        }
    }

    // Player health control
    private void HealthBar()
    {
        healthSystem = new HealthSystem(hp);
    }

    private void HandleHp()
    {
        //healthText.text = "Health: " + currentHp;
        //currentXValue = healthSystem.GetHealthPercent;
        HpBar.fillAmount = healthSystem.GetHealthPercent();


    }

    private void ChangeColor()
    {
        colorState = !colorState;
        if (!colorState) //white
        {
            gameObject.layer = 11;
        }
        else
        {
            gameObject.layer = 12;
        }
    }

    // save and load file
    public void SavePlayer()
    {
        SaveFile.SavePlayerFile(this);
    }

    public void LoadPlayer()
    {
        PlayerData data = SaveFile.LoadPlayerFile();

        hp = data.Hp;
        mp = data.Mp;
        atk = data.Attack;
        def = data.Defence;
        forward = data.Forward;
        Vector3 position;
        position.x = data.position[0];
        position.y = data.position[1];
        position.z = data.position[2];
        transform.position = position;
    }
}
