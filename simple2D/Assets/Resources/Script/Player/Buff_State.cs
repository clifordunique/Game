using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buff
{
    public BuffType type;
    public float Amount;
    public float RemainTime;

    public static bool PlayerGetBuff(Buff buff)
    {
        switch (buff.type)
        {
            case BuffType.Burning:
                break;
            case BuffType.Concertrate:
                break;
            case BuffType.Powerful:
                break;
            case BuffType.Slow:
                Player.Instance.moveSpeed -= buff.Amount;
                if (0 > Player.Instance.moveSpeed)
                {
                    Player.Instance.moveSpeed += buff.Amount;
                    return false;
                }
                break;
            case BuffType.SpeedUp:
                break;
        }
        Player.Instance.buff.Add(buff);
        return true;
    }
    public static void PlayerReleaseBuff(Buff buff)
    {
        switch (buff.type)
        {
            case BuffType.Burning:
                break;
            case BuffType.Concertrate:
                break;
            case BuffType.Powerful:
                break;
            case BuffType.Slow:
                Player.Instance.moveSpeed += buff.Amount;
                break;
            case BuffType.SpeedUp:
                break;
        }

        Player.Instance.buff.Remove(buff);
    }
}
public enum BuffType
{
    SpeedUp = 0,    //  move speed up
    Powerful,       //  attack damgage up
    Concertrate,    //  reduce the mp using amount
    Slow,           //  decrease the move speed
    Burning         //  keep lost hp
}
public struct State
{
    public bool canControll, canRigidbody;
    public bool isGround;
    public bool isSkilling;
    public int comboAuto;
};
public struct MonsterState
{
    public bool isAttack;
    public bool isClosedToEdge;
    public bool isGround;
    public bool isHurt;
    public bool isWall;
};
