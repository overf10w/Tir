using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

namespace Game
{
    // TODO (LP): 
    // 1. Resource.Load be called through the Init() chain all the way from GameManager;
    // 2. PlayerDataFile is referenced (through inspector) by other SO's
    [CustomEditor(typeof(Upgrades))]
    public class UpgradesEditor : Editor
    {
        private Upgrades upgrades;

        private string path;

        public void OnEnable()
        {
            path = Path.Combine(Application.persistentDataPath, "upgrades.dat");
            upgrades = (Upgrades)target;
        }

        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            if (GUILayout.Button("Read playerStats data from disk"))
            {
                upgrades.upgrades = ResourceLoader.Load<Upgrades.Upgrade[]>(path);
            }

            if (GUILayout.Button("Write playerStats data to disk"))
            {
                ResourceLoader.Save<Upgrades.Upgrade[]>(path, upgrades.upgrades);
            }
        }
    }
}