using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SkillMantainerA : MonoBehaviour
{
    GameObject playerObj;
    PlayerSkill playerSkill;
    private void Awake()
    {
        playerSkill = PlayerSkill.Instance;
    }
    private void Start()
    {
        playerObj = playerSkill.gameObject;
    }
    public void addSkill001()
    {
        ChangeSkillA<skill001>();
    }
    public void addSkill002()
    {
        ChangeSkillA<skill002>();
    }
    public void addSkill003()
    {
        ChangeSkillA<skill003>();
    }
    void ChangeSkillA<T>() where T : Component
    {
        playerSkill.Clear_A_Listenter();
        playerObj.AddComponent<T>();
    }
}
