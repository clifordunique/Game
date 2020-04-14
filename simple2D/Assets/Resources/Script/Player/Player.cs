using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI; // Required when Using UI elements.
[RequireComponent(typeof(ControllerPlayer))]
public class Player : Singleton<Player>
{
    public List<Buff> buff = new List<Buff>();  //  buff will clear initially
    public float gravity = -10;
    [SerializeField]
    float heavyGravity = 0, baseGravity = 0;
    public int hp, mp;
    public int atk, def = 0;
    public int forward;    // 1 is right, -1 is left
    public float moveSpeed;
    public float jumpForce;
    public float attackLength;
    public int maxCombo;
    public bool colorState; // false is white or true is black
    public Action<bool> Change;
    public Vector3 velocity;

    /* for enemy layer */
    public LayerMask layerForMonster;

    public ControllerPlayer controller;
    public State state;

    public HealthSystem healthSystem;
    public Image HpBar;

    private bool jumpLock;
    private bool hurtLock = false;

    void Awake()
    {
        gravity = baseGravity;
        controller = GetComponent<ControllerPlayer>();
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
        if (state.canControll) CanControll();
        if (velocity.y <= 0) SetToBaseGravity();
        if (state.canRigidbody) velocity.y += gravity * Time.deltaTime;

        /*if (Input.GetKey(KeyCode.Q))
        {
            GetHurt(10);
            //Debug.Log(healthSystem.GetHealth());
            HandleHp();
        }*/
        touch();
    }
    void LateUpdate()
    {   // move when other script change the velocity
        if (!GameManager.Instance.isPause)
        {
            controller.Move(velocity * Time.deltaTime);
        }
        else
        {
            velocity = Vector3.zero;
        }
        
    }
    public void GetHurt(int damage, Vector2 hitPoint)
    {
        if (hurtLock) return;
        StartCoroutine(HurtFree(hitPoint));
        int minusHp = DamageSystem.CalculateRealDamage(damage, def);
        hp -= minusHp;
        healthSystem.Damage(minusHp);
        //Debug.Log(healthSystem.GetHealth());
        HandleHp();

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
    private void CanControll()
    {
        velocity.x = 0;
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
            if (Input.GetKeyDown(KeyBoardInput.Instance.jump))
            {
                velocity.y = jumpForce;
                jumpLock = true;
            }
        }
        if (Input.GetKeyUp(KeyBoardInput.Instance.jump) && velocity.y > 0)
        {
            gravity = heavyGravity;
            jumpLock = false;
            //  never Go to heavy gravity
        }
        if (Input.GetKey(KeyBoardInput.Instance.jump) && velocity.y > 0 && jumpLock)
        {
            SetToBaseGravity();
        }

        // test
        if (Input.GetKeyDown(KeyBoardInput.Instance.down))
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
    public void SetToBaseGravity()
    {
        gravity = baseGravity;
    }
    public void SetState()
    {
        colorState = !colorState;
        Change?.Invoke(colorState);
        if (!colorState)
        {
            gameObject.layer = LayerMask.NameToLayer("Player_w");
            layerForMonster |= (1 << LayerMask.NameToLayer("Enemy_b"));
            layerForMonster &= ~(1 << LayerMask.NameToLayer("Enemy_w"));
        }
        else
        {
            gameObject.layer = LayerMask.NameToLayer("Player_b");
            layerForMonster |= (1 << LayerMask.NameToLayer("Enemy_w"));
            layerForMonster &= ~(1 << LayerMask.NameToLayer("Enemy_b"));
        }
    }

    void touch()
    {
        Bounds bounds = controller.collider.bounds;
        VerticalRaycast(1, bounds);
        VerticalRaycast(-1, bounds);
        Vector2 startFrom = new Vector2(bounds.center.x, bounds.min.y);
        Debug.DrawRay(startFrom, Vector2.up * -0.2f, Color.cyan);
        RaycastHit(Physics2D.Raycast(startFrom, Vector2.up, -0.1f, layerForMonster));
        
    }
    void VerticalRaycast(int Dir, Bounds bounds)
    {
        Vector2 startFrom = new Vector2(bounds.center.x, bounds.min.y);
        float spacing = (bounds.max.y - bounds.min.y) / 3;
        for (int i = 0; i < 4; i++)
        {
            RaycastHit2D hit;
            Debug.DrawRay(startFrom, Vector2.right * Dir * 0.2f, Color.cyan);
            if (hit = Physics2D.Raycast(startFrom, Vector2.right * Dir, 0.2f, layerForMonster))
            {
                if (RaycastHit(hit) && !hurtLock)
                {
                    break;
                }
            }
            startFrom += new Vector2(0, spacing);
        }
    }
    bool RaycastHit(RaycastHit2D hit)
    {
        if (!hit) return false;
        GameObject tmp = hit.collider.gameObject;
        if (tmp.layer == LayerMask.NameToLayer("Trap"))
        {
            tmp.GetComponent<Trap>().Trapped();
            return true;
        }
        else
        {
            GetHurt(10, (Vector2)hit.point);
            return true;
        }
    }
    IEnumerator HurtFree(Vector2 hitPoint)
    {
        hurtLock = true;
        for (int i = 0; i < 10; i++)
        {
            velocity.x += 10 * Math.Sign(transform.position.x - hitPoint.x);
            yield return null;
        }
        yield return new WaitForSeconds(0.5f);
        hurtLock = false;
    }
}
