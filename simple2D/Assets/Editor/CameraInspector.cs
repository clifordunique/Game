using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Camera_Control))]
public class CameraInspector : Editor
{
    Camera_Control camera_Control;

    public override void OnInspectorGUI()
    {
        //base.OnInspectorGUI();
        DrawDefaultInspector();
        camera_Control = (Camera_Control)target;
        //OnDrawGizmos();
        SceneView.RepaintAll();
    }
    
}
