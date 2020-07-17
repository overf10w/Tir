using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

namespace Game
{
    [CustomEditor(typeof(GunData))]
    public class GunDataEditor : Editor
    {
        private GunData _gunData;
        private string _path;
        private string _backupPath;

        public void OnEnable()
        {
            _path = Path.Combine(Application.persistentDataPath, "clickGun.dat");
            _backupPath = Path.Combine(Application.persistentDataPath, "backupClickGun.dat");
            _gunData = (GunData)target;
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
                _gunData.gunStats = ResourceLoader.Load<WeaponData>(_path);
            }

            if (GUILayout.Button("Write"))
            {
                ResourceLoader.Save<WeaponData>(_path, _gunData.gunStats);
            }

            if (GUILayout.Button("Write Default", redStyle))
            {
                ResourceLoader.Save<WeaponData>(_backupPath, _gunData.gunStats);
            }

            if (GUILayout.Button("Read Default", greenStyle))
            {
                _gunData.gunStats = ResourceLoader.Load<WeaponData>(_backupPath);
            }

            if (GUILayout.Button("Reset to Default", blueStyle))
            {
                _gunData.gunStats = ResourceLoader.Load<WeaponData>(_backupPath);
                ResourceLoader.Save<WeaponData>(_path, _gunData.gunStats);
            }
        }
    }
}