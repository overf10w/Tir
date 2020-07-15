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

        private string backupPath;

        public void OnEnable()
        {
            path = Path.Combine(Application.persistentDataPath, "upgrades.dat");
            backupPath = Path.Combine(Application.persistentDataPath, "backupUpgrades.dat");
            upgrades = (Upgrades)target;
        }

        public override void OnInspectorGUI()
        {
            var redStyle = new GUIStyle(GUI.skin.button);
            redStyle.normal.textColor = Color.red;

            var blueStyle = new GUIStyle(GUI.skin.button);
            blueStyle.normal.textColor = Color.blue;

            DrawDefaultInspector();

            if (GUILayout.Button("Read"))
            {
                upgrades.upgrades = ResourceLoader.Load<Upgrades.Upgrade[]>(path);
            }

            if (GUILayout.Button("Write"))
            {
                ResourceLoader.Save<Upgrades.Upgrade[]>(path, upgrades.upgrades);
            }

            if (GUILayout.Button("Write Default", redStyle))
            {
                ResourceLoader.Save<Upgrades.Upgrade[]>(backupPath, upgrades.upgrades);
            }

            if (GUILayout.Button("Reset Default", blueStyle))
            {
                upgrades.upgrades = ResourceLoader.Load<Upgrades.Upgrade[]>(backupPath);
            }
        }
    }
}