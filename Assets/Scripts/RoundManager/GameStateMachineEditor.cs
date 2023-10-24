using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using Newtonsoft.Json.Linq;
using OpenAI_API.Chat;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem.HID;
using UnityEngine.InputSystem.Interactions;
using UnityEngine.UI;

[CustomEditor(typeof(GameStateMachine))]
public class GameStateMachineEditor : Editor
{

    private GameStateMachine _gameStateMachine = null;
    private SerializedProperty _menus;
    private SerializedProperty States;
    private GameStateMachine.GameState _choiceState;
    private void OnEnable()
    {
        this._gameStateMachine = (GameStateMachine)this.target;
        _menus = serializedObject.FindProperty("Menus");
        States = serializedObject.FindProperty("AllStates");
    }
    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        _menus.arraySize = EditorGUILayout.IntField("Size", _menus.arraySize);


        for (int i = 0; i < _menus.arraySize; i++)
        {
            var menu = _menus.GetArrayElementAtIndex(i);
            EditorGUILayout.PropertyField(menu, new GUIContent("Menu " + i), true);
            var menuObject = menu.FindPropertyRelative("menuObject");
            GameObject menuObjectGameObject = (GameObject)menuObject.objectReferenceValue;
            if (menuObjectGameObject != null)
            {
                var listeButton = menuObjectGameObject.GetComponentsInChildren<Button>();
                var buttons = menu.FindPropertyRelative("buttons");
                buttons.arraySize = listeButton.Length;
                for (int j = 0; j < listeButton.Length; j++)
                {
                    var button = buttons.GetArrayElementAtIndex(j);
                    button.objectReferenceValue = listeButton[j];
                    _choiceState = (GameStateMachine.GameState)EditorGUILayout.EnumFlagsField(_choiceState);
                }
            }
        }

        serializedObject.ApplyModifiedProperties();
    }
}
