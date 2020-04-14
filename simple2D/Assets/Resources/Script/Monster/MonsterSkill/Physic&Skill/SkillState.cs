using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillState : MonoBehaviour
{
    public Ski01 ski01;
    public Ski02 ski02;
    public Ski03 ski03;
    public Ski04 ski04;
    public Ski05 ski05;
    Monster monster;
    Controller2D controller;

    [SerializeField]
    LayerMask playerLayer;
    [SerializeField]
    GameObject shootImpact;
    [SerializeField]
    GameObject dropImpact;

    GameObject shootImpactOnScreen;
    GameObject[] dropImpactOnScreen = new GameObject[3];


    public void PreSkillPar01()
    {
        ski01.PredictColor();
        ski01.PredictTheAngle();
    }
    public void SetSkillPar01()
    {
        Vector2 position = gameObject.transform.position;
        position.x += (monster.forward == -1) ? -0.8f : 0.8f;   /* the revision */
        shootImpactOnScreen = Instantiate(shootImpact, position, Quaternion.identity);
        BossSnakeImpact bossSnakeImpact = shootImpactOnScreen.GetComponent<BossSnakeImpact>();
        Debug.Log("shoot");
        if (-1 == ski01.color)
        {   //  gray
            bossSnakeImpact.collisionMask = (1 << LayerMask.NameToLayer("Player_w"));
            bossSnakeImpact.collisionMask |= (1 << LayerMask.NameToLayer("Player_b"));
            bossSnakeImpact.damage = ski01.damage;
            shootImpactOnScreen.GetComponent<Animator>().SetFloat("color", 0);
        }
        else if (0 == ski01.color)
        {   //  black
            bossSnakeImpact.collisionMask = 0;
            bossSnakeImpact.collisionMask = (1 << LayerMask.NameToLayer("Player_w"));
            bossSnakeImpact.damage = 2.5f*ski01.damage;
            shootImpactOnScreen.GetComponent<Animator>().SetFloat("color", 1);
        }
        else if (1 == ski01.color)
        {   // white
            bossSnakeImpact.collisionMask = 0;
            bossSnakeImpact.collisionMask = (1 << LayerMask.NameToLayer("Player_b"));
            bossSnakeImpact.damage = 2.5f*ski01.damage;
            shootImpactOnScreen.GetComponent<Animator>().SetFloat("color", 2);
        }
    }
    public void Shoot(Vector3 playerVec)
    {
        float degree = Vector3.Angle(new Vector3(1, 0, 0), playerVec);
        if (degree > 100) degree += 1;
        else degree -= 1;
        if (shootImpactOnScreen)
        {
            shootImpactOnScreen.transform.Rotate(0,0, degree + ski01.angle, Space.World);
            BossSnakeImpact bossSnakeImpact = shootImpactOnScreen.GetComponent<BossSnakeImpact>();
            bossSnakeImpact.SetInit();
        }
    }
    public IEnumerator Slave(float time)
    {
        float curTime = Time.time;
        float endTime = Time.time + time;
        Debug.Log("attack" + ski02.color);
        if (-1 == ski02.color)
        {   //  gray
            playerLayer = (1 << LayerMask.NameToLayer("Player_w"));
            playerLayer |= (1 << LayerMask.NameToLayer("Player_b"));
        }
        else if (0 == ski02.color)
        {   //  black
            playerLayer = 0;
            playerLayer = (1 << LayerMask.NameToLayer("Player_w"));
        }
        else if (1 == ski02.color)
        {   // white
            playerLayer = 0;
            playerLayer = (1 << LayerMask.NameToLayer("Player_b"));
        }
        bool isHit = false;
        StartCoroutine(MonPhysic.ChangeX(monster, 15*monster.forward, 5, time / 4));
        while (Time.time < endTime)
        {
            for (int i = 1; i < controller.horizontalRayCount-1; i++)
            {
                Vector2 rayOrigin = (monster.forward == -1) ? controller.raycastOrigins.bottomLeft : controller.raycastOrigins.bottomRight;
                rayOrigin += Vector2.up * controller.horizontalRaySpacing * i;
                Vector2 direction = Vector2.right * monster.forward;
                RaycastHit2D hit = Physics2D.Raycast(rayOrigin, direction, ski02.swordDis, playerLayer);
                Debug.DrawRay(rayOrigin, direction * ski02.swordDis, Color.white);
                if (hit && false == isHit)
                {
                    isHit = true;
                }
            }
            yield return null;
        }
    }
    public float PreSkillPar03(Vector3 position)
    {
        ski03.PredictColor();
        ski03.PredictJumpTarget(position);
        float temp = ski03.jumpTarget.x - gameObject.transform.position.x;
        if (ski03.maxjumpDis <= Mathf.Abs(temp)) return ski03.maxjumpDis * Mathf.Sign(temp);
        else return temp;
    }
    public float SetSkillPar03()
    {   //  it will return the deltaX betweem boss and player
        float temp = monster.playerPosition.x - gameObject.transform.position.x;
        if (ski03.maxjumpDis <= Mathf.Abs(temp)) return ski03.maxjumpDis * Mathf.Sign(temp);
        else return temp;
    }
    public IEnumerator JumpSlave(float time)
    {
        float curTime = Time.time;
        float endTime = Time.time + time;
        if (-1 == ski02.color)
        {   //  gray
            playerLayer = (1 << LayerMask.NameToLayer("Player_w"));
            playerLayer |= (1 << LayerMask.NameToLayer("Player_b"));
        }
        else if (0 == ski02.color)
        {   //  black
            playerLayer = 0;
            playerLayer = (1 << LayerMask.NameToLayer("Player_w"));
        }
        else if (1 == ski02.color)
        {   // white
            playerLayer = 0;
            playerLayer = (1 << LayerMask.NameToLayer("Player_b"));
        }
        bool isHit = false;
        StartCoroutine(MonPhysic.ChangeX(monster, 10, 5, time / 3));
        while (Time.time < endTime)
        {
            for (int i = 1; i < controller.horizontalRayCount - 1; i++)
            {
                Vector2 rayOrigin = (monster.forward == -1) ? controller.raycastOrigins.bottomLeft : controller.raycastOrigins.bottomRight;
                rayOrigin += Vector2.up * controller.horizontalRaySpacing * i;
                Vector2 direction = Vector2.right * monster.forward;
                RaycastHit2D hit = Physics2D.Raycast(rayOrigin, direction, ski02.swordDis, playerLayer);
                Debug.DrawRay(rayOrigin, direction * ski02.swordDis, Color.white);
                if (hit && false == isHit)
                {
                    Debug.Log("slave toutch the player");
                    isHit = true;
                }
            }
            yield return null;
        }
    }
    public Vector3 SetSkillPar04(Vector3 position)
    {
        return ski04.ChoosePosition(position);
    }
    public IEnumerator Flash()
    {
        gameObject.transform.position = ski04.dropPosition;
        yield return null;
    }
    public void SetSkillPar05()
    {
        ski05.ChoosePosition();
        float degree = Vector3.Angle(new Vector3(1, 0, 0), new Vector3(0, 1, 0));
        for (int i=0; i < 3; i++)
        {
            Vector2 position = ski05.dropPosition[i] + new Vector3(0, 6, 0);
            dropImpactOnScreen[i] = Instantiate(dropImpact, position, Quaternion.identity);
            dropImpactOnScreen[i].transform.Rotate(0, 0, degree, Space.World);
            BossSnakeImpact bossSnakeImpact = dropImpactOnScreen[i].GetComponent<BossSnakeImpact>();
            if (-1 == ski05.color)
            {   //  gray
                bossSnakeImpact.collisionMask = (1 << LayerMask.NameToLayer("Player_w"));
                bossSnakeImpact.collisionMask |= (1 << LayerMask.NameToLayer("Player_b"));
                bossSnakeImpact.damage = ski05.damage;
            }
            else if (0 == ski05.color)
            {   //  black
                bossSnakeImpact.collisionMask = 0;
                bossSnakeImpact.collisionMask = (1 << LayerMask.NameToLayer("Player_w"));
                bossSnakeImpact.damage = 2.5f * ski05.damage;
            }
            else if (1 == ski05.color)
            {   // white
                bossSnakeImpact.collisionMask = 0;
                bossSnakeImpact.collisionMask = (1 << LayerMask.NameToLayer("Player_b"));
                bossSnakeImpact.damage = 2.5f * ski05.damage;
            }
        }

    }
    public void Drop()
    {
        for(int i = 0; i < 3; i++)
        {
            BossSnakeImpact dropImpact = dropImpactOnScreen[i].GetComponent<BossSnakeImpact>();
            dropImpact.SetInit();
        }
    }
    void Start()
    {
        ski01 = Ski01.Instance;
        ski02 = Ski02.Instance;
        ski03 = Ski03.Instance;
        ski04 = Ski04.Instance;
        ski05 = Ski05.Instance;
        controller = gameObject.GetComponent<Controller2D>();
        ski01.SetInitValue();
        ski02.SetInitValue();
        ski03.SetInitValue();
        ski04.SetInitValue(gameObject.transform.position);
        ski05.SetInitValue(gameObject.transform.position);
        monster = gameObject.GetComponent<Monster>();
    }
}

public class Ski01 : Singleton<Ski01>
{
    public float angle;
    public float color;   //  0 is black ,1 is white, -1 is gray
    public float damage;
    public void PredictColor()
    {
        //if (/*愛用跳*/)
        //{
        //    if (/*距離<10*/)
        //    {

        //    }
        //    else/*距離>10*/
        //    {

        //    }
        //}
        //else // 不愛用跳
        //{

        //}

    }
    public void PredictTheAngle()
    {

    }
    public void SetInitValue()
    {
        color = -1;
        damage = 10;
        angle = 0;
    }
};
public class Ski02 : Singleton<Ski02>
{
    public float color;   //  0 is black ,1 is white, -1 is gray
    public float damage;
    public float swordDis;
    public void PredictTheAngle()
    {

    }
    public void PredictColor()
    {

    }
    public void SetInitValue()
    {
        color = -1;
        damage = 10;
        swordDis = 3;
    }
};
public class Ski03 : Singleton<Ski03>
{
    public float color;   //  0 is black ,1 is white, -1 is gray
    public float damage;
    public float swordDis;
    public Vector3 jumpTarget;
    public Vector3 predictJumpTarget;
    public float maxjumpDis;
    public void PredictTheAngle()
    {

    }
    public void PredictColor()
    {

    }
    public void PredictJumpTarget(Vector3 playerPosition)
    {   //   do predict
        jumpTarget = playerPosition;
    }
    public void SetInitValue()
    {
        color = -1;
        damage = 10;
        swordDis = 3;
        jumpTarget = Player.Instance.transform.position;
        maxjumpDis = 7;
    }
};
public class Ski04 : Singleton<Ski04>
{
    public float color;   //  0 is black ,1 is white, -1 is gray
    public float damage;
    public Vector3 dropPosition;
    public Vector3[] dropPositionSet;
    public Vector3 ChoosePosition(Vector3 playerPosition)
    {
        dropPosition = dropPositionSet[Random.Range(0, 20)];
        for(int i =0;i<5 || Mathf.Abs(dropPosition.x - playerPosition.x) <3; i++)
            dropPosition = dropPositionSet[Random.Range(0, 20)];
        float deltaX = gameObject.transform.position.x - playerPosition.x;
        /* write */
        return dropPosition;
    }
    public Vector3 PredictPosition(Vector3 temp)
    {
        return temp;
    }
    public void SetInitValue(Vector3 position)
    {
        color = -1;
        damage = 10;
        dropPositionSet = new Vector3[20];
        for (int i = 0; i < 20; i++)
        {
            dropPositionSet[i] = position + new Vector3(1, 0, 0) * i;
        }
    }
};
public class Ski05 : Singleton<Ski05>
{
    public Vector3 angle;
    public float color;   //  0 is black ,1 is white, -1 is gray
    public float damage;
    public Vector3[] dropPosition;
    public Vector3[] dropPositionSet;
    public void ChoosePosition()
    {
        dropPosition[0] = dropPositionSet[Random.Range(0, 20)];
        dropPosition[1] = dropPositionSet[Random.Range(0, 20)];
        if (dropPosition[0] == dropPosition[1])
            dropPosition[1] = dropPositionSet[Random.Range(0, 20)];
        dropPosition[2] = dropPositionSet[Random.Range(0, 20)];
        if (dropPosition[0] == dropPosition[2] || dropPosition[0] == dropPosition[1])
            dropPosition[2] = dropPositionSet[Random.Range(0, 20)];
    }
    public void SetInitValue(Vector3 position)
    {
        color = -1;
        damage = 10;
        dropPosition = new Vector3[3];
        dropPositionSet = new Vector3[20];
        for (int i = 0; i < 20; i++)
        {
            dropPositionSet[i] = position + new Vector3(1, 0, 0) * i;
        }
    }
};