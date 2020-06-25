using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

namespace Game
{
    [CustomEditor(typeof(PlayerDataFile))]
    public class PlayerDataFileEditor : Editor
    {
        private PlayerDataFile playerDataFile;

        private string path;

        public void OnEnable()
        {
            path = Path.Combine(Application.persistentDataPath, "playerStats.dat");
            playerDataFile = (PlayerDataFile)target;
        }

        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            if (GUILayout.Button("Read playerStats data from disk"))
            {
                playerDataFile.playerStats = ResourceLoader.Load<PlayerStats>(path);
            }

            if (GUILayout.Button("Write playerStats data to disk"))
            {
                ResourceLoader.Save<PlayerStats>(path, playerDataFile.playerStats);
            }
        }
    }
}