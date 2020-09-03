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

        private int _index = 0;
        private string[] _teamSkills = new string[] { "DPSMultiplier" };
        private string[] _clickGunSkills = new string[] { "DMGMultiplier", "GoldGainedMultiplier" };

        private int _listIndex = 0;

        private void OnEnable()
        {
            upgrade = (Upgrade)target;
            var statProp = serializedObject.FindProperty("_stat");

            if (upgrade.StatsList == StatsLists.TeamSkillsList.ToString())
            {
                _listIndex = 0;
            }
            else if (upgrade.StatsList == StatsLists.ClickGunSkillsList.ToString())
            {
                _listIndex = 1;
            }

            switch (_listIndex)
            {
                case 0:
                    _index = Array.FindIndex(_teamSkills, item => item == statProp.stringValue);
                    break;
                case 1:
                    _index = Array.FindIndex(_clickGunSkills, item => item == statProp.stringValue);
                    break;
            }
            //_listIndex = Array.FindIndex()
            //_index = Array.FindIndex(_teamSkills, item => item == statProp.stringValue);
        }

        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            // load real target values into SerializedProperties
            serializedObject.Update();

            SerializedProperty _statProperty = serializedObject.FindProperty("_stat");
            if (_statProperty != null)
            {
                if (upgrade.StatsList == StatsLists.TeamSkillsList.ToString())
                {
                    Debug.Log("UpgradeEditor: showing only ClickGunSkillsList...");
                    availableOptions = _teamSkills.Select(item => new GUIContent(item)).ToArray();
                }
                else if (upgrade.StatsList == StatsLists.ClickGunSkillsList.ToString())
                {
                    Debug.Log("UpgradeEditor: showing only TeamSkillsList...");
                    availableOptions = _clickGunSkills.Select(item => new GUIContent(item)).ToArray();
                }
                _index = EditorGUI.Popup(new Rect(185, 140, 300, 20), new GUIContent(""), _index, availableOptions);
                var selectedString = availableOptions[_index].text;
                _statProperty.stringValue = selectedString;
            }

            //SerializedProperty _statProperty = serializedObject.FindProperty("criterias_stat");

            //var p = serializedObject.GetIterator();
            //do
            //{
            //    if (p.name == "_stat")
            //    {
            //        if (upgrade.StatsList == StatsLists.TeamSkillsList.ToString())
            //        {
            //            Debug.Log("UpgradeEditor: showing only ClickGunSkillsList...");
            //            availableOptions = _teamSkills.Select(item => new GUIContent(item)).ToArray();
            //        }
            //        else if (upgrade.StatsList == StatsLists.ClickGunSkillsList.ToString())
            //        {
            //            Debug.Log("UpgradeEditor: showing only TeamSkillsList...");
            //            availableOptions = _clickGunSkills.Select(item => new GUIContent(item)).ToArray();
            //        }
            //        _index = EditorGUI.Popup(new Rect(185, 140, 300, 20), new GUIContent(""), _index, availableOptions);
            //        var selectedString = availableOptions[_index].text;
            //        p.stringValue = selectedString;
            //    }
            //}
            //while (p.NextVisible(true));
            serializedObject.ApplyModifiedProperties();

            //EditorGUI.BeginChangeCheck();

            //if (EditorGUI.EndChangeCheck())
            //{
            //    // Write back changed values into the real target
            //    serializedObject.ApplyModifiedProperties();

            //    // Update the existing character names as GuiContent[]
            //    //availableOptions = dialogue.CharactersList.Select(item => new GUIContent(item)).ToArray();
            //}

            //var statProp = serializedObject.FindProperty("_stat");
            //availableOptions = kekArray.Select(item => new GUIContent(item)).ToArray();

            //index = EditorGUI.Popup(new Rect(185, 140, 300, 20), new GUIContent(""), index, availableOptions);


            //var selectedString = availableOptions[index].text;
            //statProp.stringValue = selectedString;

            //// Write back changed values into the real target
            //serializedObject.ApplyModifiedProperties();
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