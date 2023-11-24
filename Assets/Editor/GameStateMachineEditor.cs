using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.Events;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

[CustomEditor(typeof(GameStateMachine))]
public class GameStateMachineEditor : Editor
{

    private GameStateMachine _gameStateMachine = null;
    [SerializeField]
    private SerializedProperty _menus;

    private void OnEnable()
    {
        var t2 = serializedObject.FindProperty("_choices");
        this._gameStateMachine = (GameStateMachine)this.target;
        _menus = serializedObject.FindProperty("Menus");

        var info = new DirectoryInfo("Assets/Scripts/RoundManager/States");
        var fileInfo = info.GetFiles();
        _gameStateMachine._choices = new string[fileInfo.Length];
        int compteur = 0;
        for (int i = 0; i < fileInfo.Length; i++)
        {
                FileInfo file = fileInfo[i];
                if (!file.Name.Contains(".meta"))
                {
                    _gameStateMachine._choices[i-compteur] = file.Name.Replace(".cs", string.Empty);
                    t2.arraySize = fileInfo.Length;
                    t2.GetArrayElementAtIndex(i - compteur).stringValue = file.Name.Replace(".cs", string.Empty);
                }
                else
                {
                    compteur++;
                }
        }
        _gameStateMachine._choices.ToList().RemoveAll(x => x == null);
        t2.GetArrayElementAtIndex(fileInfo.Length - 1).stringValue = "Go to scene";
        serializedObject.ApplyModifiedProperties();
    }
    public override void OnInspectorGUI()
    {
        
        SerializedProperty _choiceStates = serializedObject.FindProperty("_choiceStates");
        
        serializedObject.Update();
        _menus.arraySize = EditorGUILayout.IntField("Size", _menus.arraySize);

        _gameStateMachine._choiceState ??= new GameStateMachine._choiceStates[_menus.arraySize];
        if (_gameStateMachine._choiceState.Length != _menus.arraySize) _gameStateMachine._choiceState = new GameStateMachine._choiceStates[_menus.arraySize];

        for (int i = 0; i < _menus.arraySize; i++)
        {
            var menu = _menus.GetArrayElementAtIndex(i);
            EditorGUILayout.PropertyField(menu, new GUIContent("Menu " + i), true);
            var menuObject = menu.FindPropertyRelative("menuObject");
            GameObject menuObjectGameObject = (GameObject)menuObject.objectReferenceValue;

            var thismenuchoix = menu.FindPropertyRelative("thisMenu");
            int index = _gameStateMachine._choices.ToList().IndexOf(thismenuchoix.stringValue);
            index = EditorGUILayout.Popup("This menu is :", index, _gameStateMachine._choices);
            thismenuchoix.stringValue = _gameStateMachine._choices[index];


            EditorGUILayout.Space();
            if (menuObjectGameObject != null)
            {
                List<Button> ButtonsObjects = menuObjectGameObject.GetComponentsInChildren<Button>().ToList();
                var listeButton = menuObjectGameObject.GetComponentsInChildren<Button>();
                var buttons = menu.FindPropertyRelative("buttons");
                buttons.arraySize = listeButton.Length;

                var scenes = menu.FindPropertyRelative("scenes");
                scenes.arraySize = listeButton.Length;

                if (_gameStateMachine._choiceState[i] != null)
                {
                    _gameStateMachine._choiceState[i].choices ??= new string[listeButton.Length];
                    if (_gameStateMachine._choiceState[i].choices.Length != listeButton.Length) _gameStateMachine._choiceState[i].choices = new string[listeButton.Length];
                    for (int j = 0; j < listeButton.Length; j++)
                    {

                        Button bouton = (Button)menu.FindPropertyRelative("buttons").GetArrayElementAtIndex(j).objectReferenceValue;
                        int _choiceIndex = _gameStateMachine._choices.ToList().IndexOf(_gameStateMachine._choiceState[i].choices[j]);



                        if (bouton != null)
                        {
                            if (_gameStateMachine._choices.Length > _choiceIndex && _choiceIndex > 0)
                            {
                                if (_gameStateMachine._choices[_choiceIndex] == "Go to scene")
                                {
                                    var scene = (SceneAsset)scenes.GetArrayElementAtIndex(j).objectReferenceValue;
                                    scene =
                                        EditorGUILayout.ObjectField("Scene", scene, typeof(SceneAsset),
                                            false) as SceneAsset;
                                    if (scene != null)
                                        scenes.GetArrayElementAtIndex(j).objectReferenceValue = scene;
                                }
                            }

                            if (bouton.onClick.GetPersistentEventCount() > 0) UnityEventTools.RemovePersistentListener(bouton.onClick, 0);
                            if (bouton.onClick.GetPersistentEventCount() > 0) UnityEventTools.RemovePersistentListener(bouton.onClick, 0);

                            if (scenes.GetArrayElementAtIndex(j).objectReferenceValue != null)
                            {
                                var targetinfo2 = UnityEvent.GetValidMethodInfo(_gameStateMachine, "ChangeScene", new Type[] { typeof(string) });
                                UnityAction<string> action2 = Delegate.CreateDelegate(typeof(UnityAction<string>), _gameStateMachine, targetinfo2, false) as UnityAction<string>;
                                UnityEventTools.AddStringPersistentListener(bouton.onClick, action2, scenes.GetArrayElementAtIndex(j).objectReferenceValue.name);
                            }


                            var targetinfo = UnityEvent.GetValidMethodInfo(_gameStateMachine,
                                "ChangeState", new Type[] { typeof(string) });
                            UnityAction<string> action = Delegate.CreateDelegate(typeof(UnityAction<string>), _gameStateMachine, targetinfo, false) as UnityAction<string>;
                            UnityEventTools.AddStringPersistentListener(bouton.onClick, action, _gameStateMachine._choiceState[i].choices[j]);
                        }
                        var button = buttons.GetArrayElementAtIndex(j);
                        button.objectReferenceValue = listeButton[j];
                        _choiceIndex = EditorGUILayout.Popup(listeButton[j].name, _choiceIndex, _gameStateMachine._choices);
                        _choiceIndex = Mathf.Clamp(_choiceIndex, 0, _choiceIndex);
                        _gameStateMachine._choiceState[i].choices[j] = _gameStateMachine._choices[_choiceIndex];
                    }
                }
            }
            EditorGUILayout.Space();
            EditorGUILayout.Space();
        }
        GameStateMachine myBaseScript = (GameStateMachine)target;
        if (GUI.changed)
        {
            EditorUtility.SetDirty(myBaseScript);
                EditorUtility.SetDirty(target);
            EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
        }
        serializedObject.ApplyModifiedProperties();
    }
}
