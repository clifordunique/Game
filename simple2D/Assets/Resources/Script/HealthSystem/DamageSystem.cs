using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageSystem
{
    public static int CalculateRealDamage(int hurtDamage,int myDef)
    {
        float reductionPercent = ReturnPercentOfReduction(myDef);
        hurtDamage -= (myDef/2);
        int reDamage = (int)(hurtDamage * reductionPercent);
        hurtDamage -= reDamage;
        if (hurtDamage < 1) return 1;
        return hurtDamage;
    }


    private static float ReturnPercentOfReduction(int def)
    {
        return def / (100 + def);
    }
}
