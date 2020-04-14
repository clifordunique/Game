using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInfo : MonoBehaviour
{
    Player player;
    public float maxMoveSpeed = 5;
    public float maxJumpForce = 6;
    public int maxDamage = 10;
    public int maxHp = 10;
    public int maxMp = 10;

    void Start()
    {
        player = GetComponent<Player>();
        ReFlashInfo();
    }
    void ReFlashInfo()
    {
        player.moveSpeed = maxMoveSpeed;
        player.jumpForce = maxJumpForce;
        player.damage = maxDamage;
        player.hp = maxHp;
        player.mp = maxMp;
        player.canControll = player.canRigidbody = player.canVisible = true;
        player.forward = 1;
    }
    private void Update()
    {
        if (Input.GetKey(KeyCode.R)) ReFlashInfo();
    }
}
