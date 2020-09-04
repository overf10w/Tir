using System;
using System.Linq;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace Game
{
    [CustomPropertyDrawer(typeof(Criteria))]
    public class CriteriaDrawer : PropertyDrawer   // TODO: rename it to CriteriaDrawer
    {
        private int _index = 0;
        private string[] _teamSkills = new string[] { "DPSMultiplier" };
        private string[] _clickGunSkills = new string[] { "DMGMultiplier", "GoldGainedMultiplier" };

        private int _listIndex = 0;

        GUIContent[] _availableOptions;

        // Draw the property inside the given rect
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            // Using BeginProperty / EndProperty on the parent property means that
            // prefab override logic works on the entire property.
            EditorGUI.BeginProperty(position, label, property);

            var statsListProp = property.FindPropertyRelative("_statsList");

            if (statsListProp.enumNames[statsListProp.enumValueIndex] == StatsLists.TeamSkillsList.ToString())
            {
                _listIndex = 0;
                _availableOptions = _teamSkills.Select(item => new GUIContent(item)).ToArray();

                Debug.Log("Hello, TeamSkillsList selected");
            }
            else if (statsListProp.enumNames[statsListProp.enumValueIndex] == StatsLists.ClickGunSkillsList.ToString())
            {
                _listIndex = 1;
                _availableOptions = _clickGunSkills.Select(item => new GUIContent(item)).ToArray();

                Debug.Log("Hello, ClickGunSkillsList selected");
            }

            switch(_listIndex)
            {
                case 0:
                    _index = Array.FindIndex(_teamSkills, item => item == statsListProp.enumNames[statsListProp.enumValueIndex].ToString());
                    //_availableOptions = _teamSkills.Select(item => new GUIContent(item)).ToArray();
                    break;
                case 1:
                    _index = Array.FindIndex(_clickGunSkills, item => item == statsListProp.enumNames[statsListProp.enumValueIndex].ToString());
                    //_availableOptions = _clickGunSkills.Select(item => new GUIContent(item)).ToArray();
                    break;
            }

            //Debug.Log("statsListProp.enumDisplayNames[statsListProp.enumValueIndex]: " + statsListProp.enumNames[statsListProp.enumValueIndex].ToString());

            // Draw label
            position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);

            // Don't make child fields be indented
            var indent = EditorGUI.indentLevel;
            EditorGUI.indentLevel = 0;

            // Calculate rects
            var _statsListRect = new Rect(position.x, position.y, 160, 20);
            var _statRect = new Rect(position.x, position.y + 25, 100, 20);
            var _statSelectRect = new Rect(position.x, position.y + 100, 140, 20);
            var _thresholdRect = new Rect(position.x, position.y + 50, position.width - 90, 20);
            var _thresholdComparisonRect = new Rect(position.x, position.y + 75, 90, 20);

            var _upgradeRect = new Rect(position.x, position.y + 121, 160, 20);

            // Draw fields - passs GUIContent.none to each so they are drawn without labels
            EditorGUI.PropertyField(_statsListRect, property.FindPropertyRelative("_statsList"), GUIContent.none);
            EditorGUI.PropertyField(_statRect, property.FindPropertyRelative("_stat"), GUIContent.none);
            //_index = EditorGUI.Popup(_statSelectRect, _index, _availableOptions);
            EditorGUI.PropertyField(_thresholdRect, property.FindPropertyRelative("_threshold"), GUIContent.none);
            EditorGUI.PropertyField(_thresholdComparisonRect, property.FindPropertyRelative("_thresholdComparison"), GUIContent.none);
            EditorGUI.PropertyField(_upgradeRect, property.FindPropertyRelative("_upgrade"), GUIContent.none);

            var statProp = property.FindPropertyRelative("_stat");

            //var selectedString = _availableOptions[_index].text;
            //statProp.stringValue = selectedString;

            //statProp.stringValue = 

            // Set indent back to what it was
            EditorGUI.indentLevel = indent;

            EditorGUI.BeginChangeCheck();
            _index = EditorGUI.Popup(new Rect(position.x, position.y + 100.0f, 180, 20), _index, _availableOptions);
            //_index = EditorGUI.IntField(contentPosition, new GUIContent("X"), X.intValue);
            if (EditorGUI.EndChangeCheck())
            {
                statProp.stringValue = _availableOptions[_index].text;
            }

            EditorGUI.EndProperty();
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return 145.0f;
        }
    }
}