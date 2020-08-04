using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

namespace Game
{
    [CustomEditor(typeof (WeaponsData))]
    public class WeaponsDataEditor : Editor
    {
        private WeaponsData _weaponsData;
        private string _path;
        private string _backupPath;

        public void OnEnable()
        {
            _path = Path.Combine(Application.persistentDataPath, "weapons.dat");
            _backupPath = Path.Combine(Application.persistentDataPath, "backupWeapons.dat");
            _weaponsData = (WeaponsData)target;
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
                _weaponsData.Weapons = ResourceLoader.Load<WeaponData[]>(_path);
            }

            if (GUILayout.Button("Write"))
            {
                ResourceLoader.Save<WeaponData[]>(_path, _weaponsData.Weapons);
            }

            if (GUILayout.Button("Write Default", redStyle))
            {
                ResourceLoader.Save<WeaponData[]>(_backupPath, _weaponsData.Weapons);
            }

            if (GUILayout.Button("Read Default", greenStyle))
            {
                _weaponsData.Weapons = ResourceLoader.Load<WeaponData[]>(_backupPath);
            }

            if (GUILayout.Button("Reset to Default", blueStyle))
            {
                _weaponsData.Weapons = ResourceLoader.Load<WeaponData[]>(_backupPath);
                ResourceLoader.Save<WeaponData[]>(_path, _weaponsData.Weapons);
            }
        }
    }
}