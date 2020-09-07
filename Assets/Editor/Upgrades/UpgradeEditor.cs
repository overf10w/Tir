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

        private GUIContent[] _availableOptions;

        private int _index = 0;
        private string[] _teamSkills = new string[] { "DPSMultiplier" };
        private string[] _clickGunSkills = new string[] { "DMGMultiplier", "GoldGainedMultiplier" };

        private int _listIndex = 0;

        private void OnEnable()
        {
            upgrade = (Upgrade)target;
            var statProp = serializedObject.FindProperty("_stat");

            FindListIndex(upgrade, ref _listIndex);
            FindIndex(_listIndex, statProp, ref _index);
        }

        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            // load real target values into SerializedProperties
            serializedObject.Update();

            SerializedProperty _statProperty = serializedObject.FindProperty("_stat");

            if (_statProperty != null)
            {
                SetAvailableOptions(upgrade, ref _availableOptions);

                Rect lastRect = GUILayoutUtility.GetLastRect();

                float rectWidth = lastRect.width / 1.85f;
                _index = EditorGUI.Popup(new Rect(lastRect.max.x - rectWidth, 115, rectWidth, 15), new GUIContent(""), _index, _availableOptions);

                var selectedString = _availableOptions[_index].text;
                _statProperty.stringValue = selectedString;
            }
            serializedObject.ApplyModifiedProperties();
        }

        private void FindListIndex(Upgrade upgrade, ref int listIndex)
        {
            if (upgrade.StatsList == StatsLists.TeamSkillsList.ToString())
            {
                listIndex = 0;
            }
            else if (upgrade.StatsList == StatsLists.ClickGunSkillsList.ToString())
            {
                listIndex = 1;
            }
        }

        private void SetAvailableOptions(Upgrade upgrade, ref GUIContent[] availableOptions)
        {
            if (upgrade.StatsList == StatsLists.TeamSkillsList.ToString())
            {
                availableOptions = _teamSkills.Select(item => new GUIContent(item)).ToArray();
            }
            else if (upgrade.StatsList == StatsLists.ClickGunSkillsList.ToString())
            {
                availableOptions = _clickGunSkills.Select(item => new GUIContent(item)).ToArray();
            }

        }

        private void FindIndex(int listIndex, SerializedProperty stat, ref int index)
        {
            switch (listIndex)
            {
                case 0:
                    index = Array.FindIndex(_teamSkills, item => item == stat.stringValue);
                    break;
                case 1:
                    index = Array.FindIndex(_clickGunSkills, item => item == stat.stringValue);
                    break;
            }
        }
    }
}