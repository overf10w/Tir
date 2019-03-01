using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(ResourceLoader))]
public class PlayerDataEditor : Editor
{
    //private ResourceLoader resourceLoader;
    //public void OnEnable()
    //{
    //    resourceLoader = (ResourceLoader) target;
    //    //resourceLoader.ReadPlayerStats();
    //}

    //public override void OnInspectorGUI()
    //{
    //    if (GUILayout.Button("Read Data"))
    //    {
    //        resourceLoader.ReadPlayerStats();
    //    }
    //    DrawDefaultInspector();
    //    if (GUILayout.Button("Reset Player Data"))
    //    {
    //        resourceLoader.Reset();
    //        //resourceLoader.ReadPlayerStats();
    //    }
    //    if (GUILayout.Button("Write Data"))
    //    {
    //        resourceLoader.WritePlayerStats(resourceLoader.playerStats);
    //    }
    //}	
}
