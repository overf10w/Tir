using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Game
{
    [CustomPropertyDrawer(typeof(WeaponAlgorithm))]
    public class WeaponAlgorithmDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            float rowLabelWidth = 58.0f;
            float elementHeight = 16.0f;

            float additionalWidth = 29.0f;

            label = GUIContent.none;

            /*label = */EditorGUI.BeginProperty(position, label, property);

            Rect contentRect = EditorGUI.PrefixLabel(position, label);
            Rect cachedRect = new Rect(contentRect);

            float halfWidth = cachedRect.width / 2;


            // TODO: why we need it (?)
            EditorGUI.indentLevel = 1;

            EditorGUI.PropertyField(contentRect, property.FindPropertyRelative("_stats"), true);

            // Price/Value algorithms

            float kek = 17.0f;
            if (property.FindPropertyRelative("_stats").isExpanded)
            {
                for (int i = 0; i < property.FindPropertyRelative("_stats").arraySize; i++)
                {
                    if (property.FindPropertyRelative("_stats").GetArrayElementAtIndex(i).isExpanded)
                    {
                        contentRect.y += 37.0f;
                    }
                }

                contentRect.y += 44.0f;
                kek *= property.FindPropertyRelative("_stats").arraySize;
            }

            contentRect.y += kek;
            contentRect.height = 16.0f;

            EditorGUI.LabelField(contentRect, new GUIContent("Base: "));

            contentRect.x += halfWidth + additionalWidth;
            EditorGUI.LabelField(contentRect, new GUIContent("Multiplier: "));
            contentRect.x -= additionalWidth;

            contentRect.x -= halfWidth;
            contentRect.y += 18;
            contentRect.height = elementHeight;
            contentRect.width = rowLabelWidth;
            EditorGUI.LabelField(contentRect, new GUIContent("Price:"));

            contentRect.x += rowLabelWidth;
            contentRect.width = halfWidth - rowLabelWidth + additionalWidth;
            EditorGUI.PropertyField(contentRect, property.FindPropertyRelative("_basePrice"), GUIContent.none);

            contentRect.x += contentRect.width;
            EditorGUI.PropertyField(contentRect, property.FindPropertyRelative("_priceMultiplier"), GUIContent.none);

            contentRect.x -= halfWidth + additionalWidth;
            contentRect.y += 18.0f;
            contentRect.height = elementHeight;
            contentRect.width = rowLabelWidth;
            EditorGUI.LabelField(contentRect, new GUIContent("Value:"));

            contentRect.x += rowLabelWidth /*- 15.0f*/;
            contentRect.width = halfWidth - rowLabelWidth + additionalWidth;
            EditorGUI.PropertyField(contentRect, property.FindPropertyRelative("_baseValue"), GUIContent.none);

            contentRect.x += contentRect.width;
            EditorGUI.PropertyField(contentRect, property.FindPropertyRelative("_valueMultiplier"), GUIContent.none);


            // Upgrade algorithm:

            contentRect.y += 18.0f;
            contentRect.height = elementHeight;
            contentRect.x = cachedRect.x;
            EditorGUI.LabelField(contentRect, new GUIContent("Base: "));

            contentRect.x += halfWidth + additionalWidth;
            EditorGUI.LabelField(contentRect, new GUIContent("Multiplier: "));
            contentRect.x -= additionalWidth;

            contentRect.x -= halfWidth;
            contentRect.y += 18;
            contentRect.height = elementHeight;
            contentRect.width = rowLabelWidth;
            EditorGUI.LabelField(contentRect, new GUIContent("UpgPrice:"));

            contentRect.x += rowLabelWidth;
            contentRect.width = halfWidth - rowLabelWidth + additionalWidth;
            EditorGUI.PropertyField(contentRect, property.FindPropertyRelative("_baseUpgradePrice"), GUIContent.none);

            contentRect.x += contentRect.width;
            EditorGUI.PropertyField(contentRect, property.FindPropertyRelative("_upgradePriceMultiplier"), GUIContent.none);

            contentRect.x -= halfWidth + additionalWidth;

            contentRect.y += 18.0f;
            contentRect.height = elementHeight;
            contentRect.width = rowLabelWidth;
            EditorGUI.LabelField(contentRect, new GUIContent("UpgrVal:"));

            contentRect.x += rowLabelWidth;
            contentRect.width = halfWidth - rowLabelWidth + additionalWidth;
            EditorGUI.PropertyField(contentRect, property.FindPropertyRelative("_maxUpgradeLevel"), GUIContent.none);

            contentRect.x += contentRect.width;
            EditorGUI.PropertyField(contentRect, property.FindPropertyRelative("_upgradeValueMultiplier"), GUIContent.none);

            EditorGUI.EndProperty();
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            float height = 112.0f;

            float kek = 16.0f;
            if (property.FindPropertyRelative("_stats").isExpanded)
            {
                for (int i = 0; i < property.FindPropertyRelative("_stats").arraySize; i++)
                {
                    if (property.FindPropertyRelative("_stats").GetArrayElementAtIndex(i).isExpanded)
                    {
                        height += 38.0f;
                    }
                }

                height += 44.0f;
                kek *= property.FindPropertyRelative("_stats").arraySize;
            }

            return height + kek;
        }
    }
}