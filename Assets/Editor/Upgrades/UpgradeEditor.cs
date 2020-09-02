using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using System.Linq;
using System;

namespace Game
{
    [CustomEditor(typeof(Upgrade))]
    public class UpgradeEditor : Editor
    {
        private Upgrade upgrade;

        private GUIContent[] availableOptions;

        int index = 0;
        string[] kekArray = new string[] { "DPSMultiplier", "DMGMultiplier", "GoldGainedMultiplier" };

        private void OnEnable()
        {
            upgrade = (Upgrade)target;
            var statProp = serializedObject.FindProperty("_stat");
            index = Array.FindIndex(kekArray, item => item == statProp.stringValue);
            //index = EditorGUI.Popup(new Rect(45, 150, 180, 20), new GUIContent(statProp.displayName), index, availableOptions);
        }

        public override void OnInspectorGUI()
        {
            DrawScriptField();

            DrawDefaultInspector();

            // load real target values into SerializedProperties
            serializedObject.Update();

            EditorGUI.BeginChangeCheck();
            //charactersList.DoLayoutList();
            if (EditorGUI.EndChangeCheck())
            {
                // Write back changed values into the real target
                serializedObject.ApplyModifiedProperties();

                // Update the existing character names as GuiContent[]
                //availableOptions = dialogue.CharactersList.Select(item => new GUIContent(item)).ToArray();
            }

            var statProp = serializedObject.FindProperty("_stat");

            Debug.Log("statProp == null: " + (statProp == null).ToString());
            availableOptions = kekArray.Select(item => new GUIContent(item)).ToArray();

            index = EditorGUI.Popup(new Rect(185, 160, 300, 20), new GUIContent(""), index, availableOptions);
            //someIndex = EditorGUI.Popup(new Rect(45, 150, 180, 20), new GUIContent(statProp.displayName), someIndex, availableOptions);

            var selectedString = availableOptions[index].text;

            //EditorGUI.Popup(new Rect(45, 150, 180, 20), new GUIContent(statProp.displayName), someIndex, availableOptions);

            //statProp.stringValue = availableOptions[Int32.Parse(statProp.stringValue)].text;
            statProp.stringValue = selectedString;
            //statProp.displayName = statProp.stringValue;
            Debug.Log("statProp: stringValue: " + statProp.stringValue.ToString());

            //dialogItemsList.DoLayoutList();

            // Write back changed values into the real target
            serializedObject.ApplyModifiedProperties();
        }

        private void DrawScriptField()
        {
            EditorGUI.BeginDisabledGroup(true);
            EditorGUILayout.ObjectField("Script", MonoScript.FromScriptableObject((Upgrade)target), typeof(Upgrade), false);
            EditorGUI.EndDisabledGroup();

            EditorGUILayout.Space();
        }

        //private Upgrade script;
        //public void OnEnable()
        //{
        //    script = (Upgrade)target;
        //}

        //public override void OnInspectorGUI()
        //{

        //    //Debug.Log("HELLO LOG");
        //    DrawDefaultInspector();
        //    serializedObject.Update();
        //    var p = serializedObject.GetIterator();
        //    do
        //    {
        //        //if (p.name != "_script")
        //        //{
        //        //    EditorGUILayout.PropertyField(p);
        //        //}
        //        if (p.name == "_stat")
        //        {
        //            if (script.StatsList == StatsLists.ClickGunSkillsList.ToString())
        //            {
        //                Debug.Log("UpgradeEditor: showing only ClickGunSkillsList...");
        //            }
        //            else if (script.StatsList == StatsLists.TeamSkillsList.ToString())
        //            {
        //                Debug.Log("UpgradeEditor: showing only TeamSkillsList...");
        //                //NameEnum
        //                //script._stat = EditorGUILayout.EnumPopup(TeamSkillsList.DPSMultiplier).ToString();
        //            }
        //            // Debug.Log("UpgradeEditor: stat: HELLO");
        //            // Add extra GUI after "someProperty"
        //        }
        //    }
        //    while (p.NextVisible(true));
        //    serializedObject.ApplyModifiedProperties();

        //    //if (GUI.changed)
        //    //    Debug.Log("test");
        //}
    }
}