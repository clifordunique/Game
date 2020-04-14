using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillState : MonoBehaviour
{
    Ski01 ski01;
    Ski02 ski02;
    Ski03 ski03;
    Ski04 ski04;
    Ski05 ski05;
    Monster monster;
    Player player;
    Controller2D controller;
    [SerializeField]
    LayerMask playerLayer;
    [SerializeField]
    GameObject shootImpact;
    [SerializeField]
    GameObject dropImpact;

    GameObject shootImpactOnScreen;
    GameObject dropImpactOnScreen;

    public struct Ski01
    {
        public Vector3 angle;
        public float color;   //  0 is black ,1 is white, -1 is gray
        public float damage;
        public void PredictColor()
        {
            /*
             * 判斷距離 > 我預判他的反應距離 ->
             */
        }
        public void PredictTheAngle()
        {

        }
        public void SetInitValue()
        {
            color = -1;
            damage = 10;
        }
    };
    public struct Ski02
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
    public struct Ski03
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
            maxjumpDis = 10;
        }
    };
    public struct Ski04
    {
        public float color;   //  0 is black ,1 is white, -1 is gray
        public float damage;
        public Vector3 target;
        public void PredictFlashTarget(Vector3 playerPosition)
        {   //   do predict
            target = playerPosition;
        }
        public void SetInitValue()
        {
            color = -1;
            damage = 10;
        }
    };
    public struct Ski05
    {
        public Vector3 angle;
        public float color;   //  0 is black ,1 is white, -1 is gray
        public float damage;
        public void SetInitValue()
        {
            color = -1;
            damage = 10;
        }
    };

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
        if (-1 == ski01.color)
        {   //  gray
            bossSnakeImpact.collisionMask = (1 << LayerMask.NameToLayer("Player_w"));
            bossSnakeImpact.collisionMask |= (1 << LayerMask.NameToLayer("Player_b"));
            bossSnakeImpact.damage = 2.5f*ski01.damage;
        }
        else if (0 == ski01.color)
        {   //  black
            bossSnakeImpact.collisionMask = 0;
            bossSnakeImpact.collisionMask = (1 << LayerMask.NameToLayer("Player_w"));
            bossSnakeImpact.damage = ski01.damage;
        }
        else if (1 == ski01.color)
        {   // white
            bossSnakeImpact.collisionMask = 0;
            bossSnakeImpact.collisionMask = (1 << LayerMask.NameToLayer("Player_b"));
            bossSnakeImpact.damage = ski01.damage;
        }
    }
    public void Shoot(Vector3 playerPosition)
    {
        float degree = Vector3.Angle(new Vector3(1, 0, 0), playerPosition);
        shootImpactOnScreen.transform.Rotate(0, 0, degree, Space.World);
        BossSnakeImpact bossSnakeImpact = shootImpactOnScreen.GetComponent<BossSnakeImpact>();
        bossSnakeImpact.SetInit();
    }
    public void PreSkillPar02()
    {
        ski02.PredictColor();
    }
    public void SetSkillPar02()
    {

    }
    public IEnumerator Slave(float time)
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
        StartCoroutine(MonPhysic.ChangeX(monster, 15, 5, time / 3));
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
                    Debug.Log("slave toutch the player");
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
        return ski03.jumpTarget.x - gameObject.transform.position.x;
    }
    public float SetSkillPar03()
    {   //  it will return the deltaX betweem boss and player
        float temp = monster.playerPosition.x - gameObject.transform.position.x;
        if (ski03.maxjumpDis <= Mathf.Abs(temp)) return ski03.maxjumpDis;
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
    public Vector3 PreSkillPar04()
    {
        ski04.PredictFlashTarget(monster.playerPosition);
        return Vector3.zero;
    }
    public Vector3 SetSkillPar04()
    {
        return Vector3.zero;
    }
    public IEnumerator Flash(float time)
    {
        /* how to disappear */
        bool isHit = false;
        StartCoroutine(MonPhysic.ChangeX(monster, 15, 5, time / 3));
        yield return null;
    }
    public void SetSkillPar05()
    {
        Vector2 position = gameObject.transform.position + new Vector3(0,3,0);
        dropImpactOnScreen = Instantiate(dropImpact, position, Quaternion.identity);
        BossSnakeImpact bossSnakeImpact = dropImpactOnScreen.GetComponent<BossSnakeImpact>();
        if (-1 == ski05.color)
        {   //  gray
            bossSnakeImpact.collisionMask = (1 << LayerMask.NameToLayer("Player_w"));
            bossSnakeImpact.collisionMask |= (1 << LayerMask.NameToLayer("Player_b"));
            bossSnakeImpact.damage = 2.5f * ski01.damage;
        }
        else if (0 == ski05.color)
        {   //  black
            bossSnakeImpact.collisionMask = 0;
            bossSnakeImpact.collisionMask = (1 << LayerMask.NameToLayer("Player_w"));
            bossSnakeImpact.damage = ski01.damage;
        }
        else if (1 == ski05.color)
        {   // white
            bossSnakeImpact.collisionMask = 0;
            bossSnakeImpact.collisionMask = (1 << LayerMask.NameToLayer("Player_b"));
            bossSnakeImpact.damage = ski01.damage;
        }
    }
    public IEnumerator Drop()
    {
        float degree = Vector3.Angle(new Vector3(1, 0, 0), new Vector3(0,1,0));
        dropImpactOnScreen.transform.Rotate(0, 0, degree, Space.World);
        BossSnakeImpact dropImpact = shootImpactOnScreen.GetComponent<BossSnakeImpact>();
        dropImpact.SetInit();
        yield return null;
    }
    void Start()
    {
        controller = gameObject.GetComponent<Controller2D>();
        ski01.SetInitValue();
        ski02.SetInitValue();
        ski03.SetInitValue();
        monster = gameObject.GetComponent<Monster>();
        player = Player.Instance;
    }
}
