using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(StartEndPlat))]
public class MovingPlatEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        StartEndPlat startEndPlat = (StartEndPlat)target;
        for(int i = 0; i < startEndPlat.positionArr.Length; i++)
        {
            if (GUILayout.Button("Set " + i + "'th position"))
            {
                startEndPlat.positionArr[i] = startEndPlat.transform.position;
            }
        }
    }
}
