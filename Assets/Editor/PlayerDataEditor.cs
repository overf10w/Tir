using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(PlayerStats))]
public class PlayerDataEditor : Editor
{
    private PlayerStats ps;
    public void OnEnable()
    {
        ps = (PlayerStats) target;
    }

    public override void OnInspectorGUI()
    {
        if (GUILayout.Button("Read Data"))
        {
            ps.ReadSelf();
        }
        DrawDefaultInspector();
        //Editor tmpEditor = null;
        //tmpEditor = Editor.CreateEditor(ps.stats);
        if (GUILayout.Button("Reset Data"))
        {
            ps.Reset();
        }
        if (GUILayout.Button("Write Data"))
        {
            ps.WriteSelf();
        }
    }

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
