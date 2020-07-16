using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

namespace Game
{
    [CustomEditor(typeof (WeaponDataFiles))]
    public class WeaponDataFilesEditor : Editor
    {
        private WeaponDataFiles _weaponsDataFile;
        private string _path;
        private string _backupPath;

        public void OnEnable()
        {
            _path = Path.Combine(Application.persistentDataPath, "weapons.dat");
            _backupPath = Path.Combine(Application.persistentDataPath, "backupWeapons.dat");
            _weaponsDataFile = (WeaponDataFiles)target;
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
                _weaponsDataFile.weapons = ResourceLoader.Load<WeaponStatData[]>(_path);
            }

            if (GUILayout.Button("Write"))
            {
                ResourceLoader.Save<WeaponStatData[]>(_path, _weaponsDataFile.weapons);
            }

            if (GUILayout.Button("Write Default", redStyle))
            {
                ResourceLoader.Save<WeaponStatData[]>(_backupPath, _weaponsDataFile.weapons);
            }

            if (GUILayout.Button("Reset Default", blueStyle))
            {
                _weaponsDataFile.weapons = ResourceLoader.Load<WeaponStatData[]>(_backupPath);
            }
        }
    }
}