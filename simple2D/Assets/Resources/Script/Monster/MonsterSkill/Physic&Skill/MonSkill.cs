using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonSkill
{
    public enum BattleState
    {
        Stand = 0,
        Walk,
        Skilling
    }
    [SerializeField]
    public BattleState battleState;
    Dictionary<string, bool> Dic = new Dictionary<string, bool>();

    public void AddSkill(string str)
    {
        Dic.Add(str, true);
    }
    public void SetState(BattleState state)
    {
        battleState = state;
    }
    public void StartSkill(string str)
    {
        Debug.Log("use skill " + str);
        battleState = BattleState.Skilling;
        Dic[str] = false;  //  enter to cooldown
    }
    public void EndSkill(string str)
    {
        Dic[str] = true;
    }
    public bool IsReady(string str)
    {
        if (true == Dic[str])return true;
        return false;
    }
    public bool IsReadyForAll()
    {
        return Dic.ContainsValue(true);
    }
}
