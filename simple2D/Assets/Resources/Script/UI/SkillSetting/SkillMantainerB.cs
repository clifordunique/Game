using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SkillMantainerB : MonoBehaviour
{
    GameObject playerObj;
    PlayerSkill playerSkill;
    private void Start()
    {
        playerSkill = PlayerSkill.Instance;
        playerObj = playerSkill.gameObject;
    }
    public void addSkill201()
    {
        ChangeSkillB<skill201>();
    }
    public void addSkill202()
    {
        ChangeSkillB<skill202>();
    }
    void ChangeSkillB<T>() where T : Component
    {
        playerSkill.Clear_B_Listenter();
        playerObj.AddComponent<T>();
    }
}
