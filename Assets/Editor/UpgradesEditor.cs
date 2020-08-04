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
    [CustomEditor(typeof(UpgradesSO))]
    public class UpgradesEditor : Editor
    {
        private UpgradesSO _upgrades;
        //private string _path;
        //private string _backupPath;

        private string _upgradesSave;

        public void OnEnable()
        {
            //_path = Path.Combine(Application.persistentDataPath, "upgrades.dat");
            //_backupPath = Path.Combine(Application.persistentDataPath, "backupUpgrades.dat");
            _upgradesSave = Path.Combine(Application.persistentDataPath, "upgradesSave.dat");
            _upgrades = (UpgradesSO)target;
        }

        public override void OnInspectorGUI()
        {
            var redStyle = new GUIStyle(GUI.skin.button);
            redStyle.normal.textColor = Color.red;

            var blueStyle = new GUIStyle(GUI.skin.button);
            blueStyle.normal.textColor = Color.blue;

            var greenStyle = new GUIStyle(GUI.skin.button);
            greenStyle.normal.textColor = Color.green;

            DrawDefaultInspector();

            //if (GUILayout.Button("Read"))
            //{
            //    _upgrades.upgrades = ResourceLoader.Load<Upgrade[]>(_path);
            //}

            //if (GUILayout.Button("Write"))
            //{
            //    ResourceLoader.Save<Upgrade[]>(_path, _upgrades.upgrades);
            //}

            //if (GUILayout.Button("Write Default", redStyle))
            //{
            //    ResourceLoader.Save<Upgrade[]>(_backupPath, _upgrades.upgrades);
            //}

            //if (GUILayout.Button("Read Default", greenStyle))
            //{
            //    _upgrades.upgrades = ResourceLoader.Load<Upgrade[]>(_backupPath);
            //}

            //if (GUILayout.Button("Reset to Default", blueStyle))
            //{
            //    _upgrades.upgrades = ResourceLoader.Load<Upgrade[]>(_backupPath);
            //    ResourceLoader.Save<Upgrade[]>(_path, _upgrades.upgrades);
            //}

            if (GUILayout.Button("Read Upgrades Data"))
            {
                _upgrades.SetUpgradesData(ResourceLoader.Load<UpgradeData[]>(_upgradesSave));
            }

            if (GUILayout.Button("Save Upgrades Data"))
            {
                UpgradeData[] upgradesData = _upgrades.GetUpgradesData();
                ResourceLoader.Save<UpgradeData[]>(_upgradesSave, upgradesData);
            }
        }
    }
}