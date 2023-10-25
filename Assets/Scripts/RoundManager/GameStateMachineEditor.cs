using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using JetBrains.Annotations;
using Newtonsoft.Json.Linq;
using OpenAI_API.Chat;
using Unity.VisualScripting;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.InputSystem.HID;
using UnityEngine.InputSystem.Interactions;
using UnityEngine.UI;
using static UnityEditor.VersionControl.Asset;

[CustomEditor(typeof(GameStateMachine))]
public class GameStateMachineEditor : Editor
{

    private GameStateMachine _gameStateMachine = null;
    private SerializedProperty _menus;



    private void OnEnable()
    {
        var t2 = serializedObject.FindProperty("_choices");
        this._gameStateMachine = (GameStateMachine)this.target;
        _menus = serializedObject.FindProperty("Menus");

        var info = new DirectoryInfo("Assets/Scripts/RoundManager/States");
        var fileInfo = info.GetFiles();
        _gameStateMachine._choices = new string[fileInfo.Length];
        for (int i = 0; i < fileInfo.Length; i++)
        {
            FileInfo file = fileInfo[i];
            if (!file.Name.Contains(".meta"))
            {
                _gameStateMachine._choices[i] = file.Name.Replace(".cs", string.Empty);
                t2.arraySize = fileInfo.Length;
                t2.GetArrayElementAtIndex(i).stringValue = file.Name.Replace(".cs", string.Empty);
            }
                

        }


        serializedObject.ApplyModifiedProperties();
    }
    public override void OnInspectorGUI()
    {
        var t1 = serializedObject.FindProperty("_choiceState");
        Debug.Log(t1);
        serializedObject.Update();
        _menus.arraySize = EditorGUILayout.IntField("Size", _menus.arraySize);

        _gameStateMachine._choiceState ??= new string[_menus.arraySize][];
        if (_gameStateMachine._choiceState.Length != _menus.arraySize) _gameStateMachine._choiceState = new string[_menus.arraySize][];

        t1.arraySize = _menus.arraySize;
        for (int i = 0; i < _menus.arraySize; i++)
        {
            var menu = _menus.GetArrayElementAtIndex(i);
            EditorGUILayout.PropertyField(menu, new GUIContent("Menu " + i), true);
            var menuObject = menu.FindPropertyRelative("menuObject");
            GameObject menuObjectGameObject = (GameObject)menuObject.objectReferenceValue;

            var thismenuchoix = menu.FindPropertyRelative("thisMenu");
            int index = _gameStateMachine._choices.ToList().IndexOf(thismenuchoix.stringValue);
            index = Mathf.Clamp(index, 0, _gameStateMachine._choices.Length - 1);
            index = EditorGUILayout.Popup("This menu is :", index, _gameStateMachine._choices);
            thismenuchoix.stringValue = _gameStateMachine._choices[index];

            EditorGUILayout.Space();
            if (menuObjectGameObject != null)
            {
                List<Button> ButtonsObjects = menuObjectGameObject.GetComponentsInChildren<Button>().ToList();
                var listeButton = menuObjectGameObject.GetComponentsInChildren<Button>();
                var buttons = menu.FindPropertyRelative("buttons");
                buttons.arraySize = listeButton.Length;

                _gameStateMachine._choiceState[i] ??= new string[listeButton.Length];
                if (_gameStateMachine._choiceState[i].Length != listeButton.Length) _gameStateMachine._choiceState[i] = new string[listeButton.Length];
                t1.GetArrayElementAtIndex(i).arraySize = listeButton.Length;
                for (int j = 0; j < listeButton.Length; j++)
                {
                    int _choiceIndex = _gameStateMachine._choices.ToList().IndexOf(_gameStateMachine._choiceState[i][j]);
                    ButtonsObjects[j].onClick.RemoveAllListeners();
                    var button = buttons.GetArrayElementAtIndex(j);
                    button.objectReferenceValue = listeButton[j];
                    _choiceIndex = EditorGUILayout.Popup(listeButton[j].name, _choiceIndex, _gameStateMachine._choices);
                    _gameStateMachine._choiceState[i][j] = _gameStateMachine._choices[_choiceIndex];
                    ButtonsObjects[j].onClick.AddListener(() => _gameStateMachine.ChangeState(_choiceIndex, _gameStateMachine._choices[index]));

                    t1.GetArrayElementAtIndex(i).GetArrayElementAtIndex(j).objectReferenceValue = listeButton[j];
                }
            }



            EditorGUILayout.Space();
            EditorGUILayout.Space();

        }
        if (GUI.changed)
        {
            EditorUtility.SetDirty(target);
            EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
        }
        serializedObject.ApplyModifiedProperties();
    }
}
