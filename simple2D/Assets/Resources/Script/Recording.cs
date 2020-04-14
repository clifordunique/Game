using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Recording : MonoBehaviour
{
    public GameObject player;
    public GameObject monster;
    int timer;

    public List<float> playerVelocityX = new List<float>(); // 當前 X 速度
    public List<float> playerVelocityY = new List<float>(); // 當前 Y 速度
    public List<float> playerHP = new List<float>(); //
    public List<float> monsterHP = new List<float>(); // 當前 Y 速度

    // 偏好用跳/變色
    List<float> ColorChangeCount = new List<float>();
    List<float> JumpCount = new List<float>();
    List<float> MonsterSkill = new List<float>();

    public List<Vector3> distance = new List<Vector3>(); // BOSS的保持距離(經平均距離)

    // Start is called before the first frame update
    void Start()
    {
        timer = 0;
        player = GameObject.Find("player");
        monster = GameObject.Find("BossSnake");
        InvokeRepeating("TimeCounter", 1f, 1f);
    }

    void TimeCounter()
    {
        timer += 1;
        distance.Add(monster.transform.position - player.transform.position);
        playerVelocityX.Add(Player.Instance.velocity.x);
        playerVelocityY.Add(Player.Instance.velocity.y);
        playerHP.Add(Player.Instance.hp);
        monsterHP.Add(monster.GetComponent<Monster>().hp);
        Debug.Log("Time: " + timer);

    }
    // Update is called once per frame
    //void Update()
    //{
        
    //}
}
