using System;
using System.Linq;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace Game
{
    [CustomPropertyDrawer(typeof(Criteria))]
    public class CriteriaDrawer : PropertyDrawer   
    {
        private int _index = 0;
        private string[] _teamSkills = new string[] { "DPSMultiplier" };
        private string[] _clickGunSkills = new string[] { "DMGMultiplier", "GoldGainedMultiplier" };

        private int _listIndex = 0;

        private GUIContent[] _availableOptions;

        // Draw the property inside the given rect
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            // Using BeginProperty / EndProperty on the parent property means that
            // prefab override logic works on the entire property.
            EditorGUI.BeginProperty(position, label, property);

            var statsListProp = property.FindPropertyRelative("_statsList");
            var stat = property.FindPropertyRelative("_stat");

            FindListIndex(statsListProp, ref _listIndex);
            SetAvailableOptions(_listIndex, ref _availableOptions);
            FindIndex(_listIndex, stat, ref _index);

            // Draw label
            position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);
            
            // Don't make child fields indented
            var indent = EditorGUI.indentLevel;
            EditorGUI.indentLevel = 0;

            var statsListRect = new Rect(position.x, position.y, position.width, 15);
            var statRect = new Rect(position.x, position.y + 20, position.width, 15);
            var thresholdRect = new Rect(position.x, position.y + 40, 40, 15);
            var thresholdComparisonRect = new Rect(position.x + 40, position.y + 40, position.width - 40, 15);
            var upgradeRect = new Rect(position.x, position.y + 60, position.width, 15);

            // Draw fields - passs GUIContent.none to each so they are drawn without labels
            EditorGUI.PropertyField(statsListRect, property.FindPropertyRelative("_statsList"), GUIContent.none);
            EditorGUI.PropertyField(thresholdRect, property.FindPropertyRelative("_threshold"), GUIContent.none);
            EditorGUI.PropertyField(thresholdComparisonRect, property.FindPropertyRelative("_thresholdComparison"), GUIContent.none);
            EditorGUI.PropertyField(upgradeRect, property.FindPropertyRelative("_upgrade"), GUIContent.none);

            EditorGUI.BeginChangeCheck();
            _index = EditorGUI.Popup(statRect, _index, _availableOptions);
            if (EditorGUI.EndChangeCheck())
            {
                stat.stringValue = _availableOptions[_index].text;
            }

            EditorGUI.indentLevel = indent;
            EditorGUI.EndProperty();
        }

        private void FindListIndex(SerializedProperty statsListProp, ref int listIndex)
        {
            if (statsListProp.enumNames[statsListProp.enumValueIndex] == StatsLists.TeamSkillsList.ToString())
            {
                listIndex = 0;
            }
            else if (statsListProp.enumNames[statsListProp.enumValueIndex] == StatsLists.ClickGunSkillsList.ToString())
            {
                listIndex = 1;
            }
        }

        private void SetAvailableOptions(int listIndex, ref GUIContent[] availableOptions)
        {
            switch(listIndex)
            {
                case 0:
                    availableOptions = _teamSkills.Select(item => new GUIContent(item)).ToArray();
                    break;
                case 1:
                    availableOptions = _clickGunSkills.Select(item => new GUIContent(item)).ToArray();
                    break;
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

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return 80.0f;
        }
    }
}