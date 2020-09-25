using System;
using System.Collections.Generic;
using System.Linq;
#if UNITY_EDITOR
using UnityEditor;
using UnityEditorInternal;
#endif
using UnityEngine;

[CreateAssetMenu]
public class Dialogue : ScriptableObject
{
    public string[] CharactersList;
    public List<DialogueElement> DialogueItems;
}

[Serializable]
public class DialogueElement
{
    public int CharacterID;

    [TextArea] public string DialogueText;
}

#if UNITY_EDITOR
[CustomEditor(typeof(Dialogue))]
public class DialogueEditor : Editor
{
    private SerializedProperty CharactersList;
    private SerializedProperty DialogueItems;

    private ReorderableList charactersList;
    private ReorderableList dialogItemsList;

    private Dialogue dialogue;

    private GUIContent[] availableOptions;

    // Called when the Inspector is opened / ScriptableObject is selected
    private void OnEnable()
    {
        dialogue = (Dialogue)target;

        CharactersList = serializedObject.FindProperty(nameof(Dialogue.CharactersList));
        DialogueItems = serializedObject.FindProperty(nameof(Dialogue.DialogueItems));

        charactersList = new ReorderableList(serializedObject, CharactersList)
        {
            displayAdd = true,
            displayRemove = true,
            draggable = false,

            // As the header we use display name of the CharactersList
            drawHeaderCallback = rect => EditorGUI.LabelField(rect, CharactersList.displayName),

            // How shall elements be displayed
            drawElementCallback = (rect, index, focused, active) =>
            {
                // get the current element's SerializedProperty
                var element = CharactersList.GetArrayElementAtIndex(index);

                // Get all characters as string[]
                var availableIDs = dialogue.CharactersList;

                // store the original GUI.color
                var color = GUI.color;
                // Tint the field in red for invalid values (empty or duplicate)
                if (string.IsNullOrWhiteSpace(element.stringValue) || availableIDs.Count(item => string.Equals(item, element.stringValue)) > 1)
                {
                    GUI.color = Color.red;
                }
                // Draw the property which automatically will select the correct drawer -> a single line text field
                EditorGUI.PropertyField(new Rect(rect.x, rect.y, rect.width, EditorGUI.GetPropertyHeight(element)), element);

                // reset to the default color
                GUI.color = color;

                // If the value is invalid draw a HelpBox to explain why it is invalid
                if (string.IsNullOrWhiteSpace(element.stringValue))
                {
                    rect.y += EditorGUI.GetPropertyHeight(element);
                    EditorGUI.HelpBox(new Rect(rect.x, rect.y, rect.width, EditorGUIUtility.singleLineHeight), "ID may not be empty!", MessageType.Error);
                }
                else if (availableIDs.Count(item => string.Equals(item, element.stringValue)) > 1)
                {
                    rect.y += EditorGUI.GetPropertyHeight(element);
                    EditorGUI.HelpBox(new Rect(rect.x, rect.y, rect.width, EditorGUIUtility.singleLineHeight), "Duplicate! ID has to be unique!", MessageType.Error);
                }
            },

            // Get the correct display height of elements in the list
            // according to their values
            // in this case e.g. dependent whether a HelpBox is displayed or not
            elementHeightCallback = index =>
            {
                var element = CharactersList.GetArrayElementAtIndex(index);
                var availableIDs = dialogue.CharactersList;

                var height = EditorGUI.GetPropertyHeight(element);

                if (string.IsNullOrWhiteSpace(element.stringValue) || availableIDs.Count(item => string.Equals(item, element.stringValue)) > 1)
                {
                    height += EditorGUIUtility.singleLineHeight;
                }

                return height;
            },

            // Overwrite what shall be done when an element is added via the +
            // Reset all values to the defaults for new added elements
            // By default Unity would clone the values from the last or selected element otherwise
            onAddCallback = list =>
            {
                // This adds the new element but copies all values of the select or last element in the list
                list.serializedProperty.arraySize++;

                var newElement = list.serializedProperty.GetArrayElementAtIndex(list.serializedProperty.arraySize - 1);
                newElement.stringValue = "";
            }

        };

        dialogItemsList = new ReorderableList(serializedObject, DialogueItems)
        {
            displayAdd = true,
            displayRemove = true,
            draggable = true,

            drawHeaderCallback = rect => EditorGUI.LabelField(rect, DialogueItems.displayName),

            // How shall elements be displayed
            drawElementCallback = (rect, index, focused, active) =>
            {
                // get the current element's SerializedProperty
                var element = DialogueItems.GetArrayElementAtIndex(index);

                // Get the nested property fields of the DialogueElement class
                var character = element.FindPropertyRelative(nameof(DialogueElement.CharacterID));
                var text = element.FindPropertyRelative(nameof(DialogueElement.DialogueText));

                var popUpHeight = EditorGUI.GetPropertyHeight(character);

                // store the original GUI.color
                var color = GUI.color;

                // if the value is invalid tint the next field red
                if (character.intValue < 0) GUI.color = Color.red;

                // Draw the Popup so you can select from the existing character names
                character.intValue = EditorGUI.Popup(new Rect(rect.x, rect.y, rect.width, popUpHeight), new GUIContent(character.displayName), character.intValue, availableOptions);

                GUI.color = color;
                rect.y += popUpHeight;

                // Draw the text field
                // since we use a PropertyField it will automatically recognize that this field is tagged [TextArea]
                // and will choose the correct drawer accordingly
                var textHeight = EditorGUI.GetPropertyHeight(text);
                EditorGUI.PropertyField(new Rect(rect.x, rect.y, rect.width, textHeight), text);
            },

            // Get the correct display height of elements in the list
            // according to their values
            // in this case e.g. we add an additional line as a little spacing between elements
            elementHeightCallback = index =>
            {
                var element = DialogueItems.GetArrayElementAtIndex(index);

                var character = element.FindPropertyRelative(nameof(DialogueElement.CharacterID));
                var text = element.FindPropertyRelative(nameof(DialogueElement.DialogueText));

                return EditorGUI.GetPropertyHeight(character) + EditorGUI.GetPropertyHeight(text) + EditorGUIUtility.singleLineHeight;
            },

            // Overwrite what shall be done when an element is added via the +
            // Reset all values to the defaults for new added elements
            // By default Unity would clone the values from the last or selected element otherwise
            onAddCallback = list =>
            {
                // This adds the new element but copies all values of the select or last element in the list
                list.serializedProperty.arraySize++;

                var newElement = list.serializedProperty.GetArrayElementAtIndex(list.serializedProperty.arraySize - 1);
                var character = newElement.FindPropertyRelative(nameof(DialogueElement.CharacterID));
                var text = newElement.FindPropertyRelative(nameof(DialogueElement.DialogueText));

                character.intValue = -1;
                text.stringValue = "";
            }
        };

        // Get the existing character names ONCE as GuiContent[]
        // Later only update this if the charcterList was changed
        availableOptions = dialogue.CharactersList.Select(item => new GUIContent(item)).ToArray();
    }

    public override void OnInspectorGUI()
    {
        DrawScriptField();

        // load real target values into SerializedProperties
        serializedObject.Update();

        EditorGUI.BeginChangeCheck();
        charactersList.DoLayoutList();
        if (EditorGUI.EndChangeCheck())
        {
            // Write back changed values into the real target
            serializedObject.ApplyModifiedProperties();

            // Update the existing character names as GuiContent[]
            availableOptions = dialogue.CharactersList.Select(item => new GUIContent(item)).ToArray();
        }

        dialogItemsList.DoLayoutList();

        // Write back changed values into the real target
        serializedObject.ApplyModifiedProperties();
    }

    private void DrawScriptField()
    {
        EditorGUI.BeginDisabledGroup(true);
        EditorGUILayout.ObjectField("Script", MonoScript.FromScriptableObject((Dialogue)target), typeof(Dialogue), false);
        EditorGUI.EndDisabledGroup();

        EditorGUILayout.Space();
    }
}
#endif