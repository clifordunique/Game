using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public static class MonsterDictionary
{
    public static Dictionary<string, Type> MonsterDic = new Dictionary<string, Type>() {
        { "Slim", typeof(Slim) },
    };
    /*public static Dictionary<string, Component> MonsterDic = new Dictionary<string, Component>() {
        { "Slim", Component(MonsterAI_00) },
    };*/
}
