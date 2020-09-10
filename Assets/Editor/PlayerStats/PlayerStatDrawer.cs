using UnityEditor;
using UnityEngine;

namespace Game
{
    [CustomPropertyDrawer(typeof(PlayerStat))]
    public class PlayerStatDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            label = GUIContent.none;

            EditorGUI.BeginProperty(position, label, property);
            
            Rect contentPosition = EditorGUI.PrefixLabel(position, label);
            float cachedWidth = contentPosition.width;
            float iconWidth = 48.0f;
            EditorGUI.indentLevel = 0;

            contentPosition.width = iconWidth;
            EditorGUI.PropertyField(contentPosition, property.FindPropertyRelative("_icon"), GUIContent.none);

            contentPosition.x += contentPosition.width;
            contentPosition.width = cachedWidth - contentPosition.x;

            EditorGUI.PropertyField(contentPosition, property.FindPropertyRelative("_name"), GUIContent.none);

            contentPosition.x += contentPosition.width + 1;
            contentPosition.width = cachedWidth - contentPosition.x + 44;
            EditorGUI.PropertyField(contentPosition, property.FindPropertyRelative("_value"), GUIContent.none);

            EditorGUI.EndProperty();
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return 16.0f;
        }
    }
}