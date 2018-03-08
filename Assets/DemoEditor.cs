using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(DemoControls))]
public class DemoEditor : Editor
{

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        DemoControls myDemoControls = (DemoControls) target;
        if (GUILayout.Button("Spawn Robots"))
        {
            myDemoControls.SpawnRobots();
        }
        if (GUILayout.Button("Update gun mode"))
        {
            myDemoControls.UpdateGunSetting();
        }


    }
}
