using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TmpAttackCnt : MonoBehaviour
{
    [SerializeField]
    string fieldName;
    [SerializeField]
    int simpleAttack, shoot;
    public void AttackCount(int which)
    {
        if (which == 0)
        {
            simpleAttack++;
        }
        else
        {
            shoot++;
        }
        float v = simpleAttack + (float)(shoot) * 0.1f;
        TmpSaver.Instance.ChangeValue(2, fieldName, v);
    }
}
