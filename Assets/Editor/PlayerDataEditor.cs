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
    [CustomEditor(typeof(PlayerData))]
    public class PlayerDataEditor : Editor
    {
        private PlayerData _playerData;

        private string _path;
        private string _backupPath;

        public void OnEnable()
        {
            _path = Path.Combine(Application.persistentDataPath, "playerStats.dat");
            _backupPath = Path.Combine(Application.persistentDataPath, "backupPlayerStats.dat");
            _playerData = (PlayerData)target;
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

            if (GUILayout.Button("Read"))
            {
                _playerData.playerStats = ResourceLoader.Load<PlayerStats>(_path);
            }

            if (GUILayout.Button("Write"))
            {
                ResourceLoader.Save<PlayerStats>(_path, _playerData.playerStats);
            }

            if (GUILayout.Button("Write Default", redStyle))
            {
                ResourceLoader.Save<PlayerStats>(_backupPath, _playerData.playerStats);
            }

            if (GUILayout.Button("Read Default", greenStyle))
            {
                _playerData.playerStats = ResourceLoader.Load<PlayerStats>(_backupPath);
            }

            if (GUILayout.Button("Reset to Default", blueStyle))
            {
                _playerData.playerStats = ResourceLoader.Load<PlayerStats>(_backupPath);
                ResourceLoader.Save<PlayerStats>(_path, _playerData.playerStats);
            }
        }
    }
}