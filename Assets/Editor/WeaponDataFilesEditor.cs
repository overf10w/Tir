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
        private WeaponDataFiles weaponDataFiles;
        
        private string path;

        public void OnEnable()
        {
            path = Path.Combine(Application.persistentDataPath, "weapons.dat");
            weaponDataFiles = (WeaponDataFiles)target;
        }

        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            if (GUILayout.Button("Read WeaponDataArray From Disk"))
            {
                weaponDataFiles.weapons = ResourceLoader.Load<WeaponStatData[]>(path);
            }

            if (GUILayout.Button("Write WeaponDataArray To Disk"))
            {
                ResourceLoader.Save<WeaponStatData[]>(path, weaponDataFiles.weapons);
            }
        }
    }
}