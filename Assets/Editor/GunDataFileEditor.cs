using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

namespace Game
{
    [CustomEditor(typeof(GunDataFile))]
    public class GunDataFileEditor : Editor
    {
        private GunDataFile gunDataFile;

        private string path;

        public void OnEnable()
        {
            path = Path.Combine(Application.persistentDataPath, "clickGun.dat");
            gunDataFile = (GunDataFile)target;
        }

        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            if (GUILayout.Button("Read ClickGun data from disk"))
            {
                gunDataFile.gunStats = ResourceLoader.Load<WeaponStatData>(path);
            }

            if (GUILayout.Button("Write ClickGun data to disk"))
            {
                ResourceLoader.Save<WeaponStatData>(path, gunDataFile.gunStats);
            }
        }
    }
}