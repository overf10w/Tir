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
    [CustomEditor(typeof(PlayerDataFile))]
    public class PlayerDataFileEditor : Editor
    {
        private PlayerDataFile _playerDataFile;

        private string _path;
        private string _backupPath;

        public void OnEnable()
        {
            _path = Path.Combine(Application.persistentDataPath, "playerStats.dat");
            _backupPath = Path.Combine(Application.persistentDataPath, "backupPlayerStats.dat");
            _playerDataFile = (PlayerDataFile)target;
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
                _playerDataFile.playerStats = ResourceLoader.Load<PlayerStats>(_path);
            }

            if (GUILayout.Button("Write"))
            {
                ResourceLoader.Save<PlayerStats>(_path, _playerDataFile.playerStats);
            }

            if (GUILayout.Button("Write Default", redStyle))
            {
                ResourceLoader.Save<PlayerStats>(_backupPath, _playerDataFile.playerStats);
            }

            if (GUILayout.Button("Reset Default", blueStyle))
            {
                _playerDataFile.playerStats = ResourceLoader.Load<PlayerStats>(_backupPath);
            }
        }
    }
}