using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerData
{
    // Current data
    public int Hp, Mp;
    public int Attack, Defence;
    public int Forward;
    public float[] position;
    //public float MoveSpeed, JumpForce;
    public bool State;


    // Init Setting
    public int initAttack = 10, initDefence = 10;
    public int maxHp = 100000, maxMp = 10;
    public float maxMoveSpeed = 5, maxJumpForce = 6;
    public bool DefaultState = true; // White


    public PlayerData(Player player)
    {
        Hp = player.hp;
        Mp = player.mp;
        Attack = player.atk;
        Defence = player.def;
        Forward = player.forward;

        // position saving
        position = new float[3];
        position[0] = player.transform.position.x;
        position[1] = player.transform.position.y;
        position[2] = player.transform.position.z;

        State = player.colorState;
    }
}
