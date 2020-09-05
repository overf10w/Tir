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
                    _index = Array.FindIndex(_teamSkills, item => item == statProp.stringValue); // TODO: this can be merged with if/else if clause above
                    break;
                case 1:
                    _index = Array.FindIndex(_clickGunSkills, item => item == statProp.stringValue);
                    break;
            }
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
                    availableOptions = _teamSkills.Select(item => new GUIContent(item)).ToArray();
                }
                else if (upgrade.StatsList == StatsLists.ClickGunSkillsList.ToString())
                {
                    availableOptions = _clickGunSkills.Select(item => new GUIContent(item)).ToArray();
                }

                Rect lastRect = GUILayoutUtility.GetLastRect();

                float rectWidth = lastRect.width / 1.85f;
                _index = EditorGUI.Popup(new Rect(lastRect.max.x - rectWidth, 115, rectWidth, 15), new GUIContent(""), _index, availableOptions);

                var selectedString = availableOptions[_index].text;
                _statProperty.stringValue = selectedString;
            }
            serializedObject.ApplyModifiedProperties();
        }
    }
}