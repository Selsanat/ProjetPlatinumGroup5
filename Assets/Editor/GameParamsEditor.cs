using Microsoft.Extensions.Logging.Abstractions.Internal;
using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.Events;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


[CustomEditor(typeof(GameParams))]
public class GameParamsEditor : Editor
{
    private SerializedProperty PointsToWin;
    private SerializedProperty PointsPerRound;
    private SerializedProperty TransiTimeAfterRound;
    private SerializedProperty Scenes;
    private SerializedProperty objects;
    private SceneAsset[] scene;

    private void OnEnable()
    {
        PointsToWin = serializedObject.FindProperty("PointsToWin");
        PointsPerRound = serializedObject.FindProperty("PointsPerRound");
        TransiTimeAfterRound = serializedObject.FindProperty("TransiTimeAfterRound");
        Scenes = serializedObject.FindProperty("Scenes");
        objects = serializedObject.FindProperty("objects");
    }
    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        Array.Resize(ref scene, objects.arraySize);
        for (int i = 0; i < scene.Length; i++)
        {
            if (objects.arraySize > i) scene[i] = objects.GetArrayElementAtIndex(i).objectReferenceValue as SceneAsset;
        }
            PointsToWin.intValue = EditorGUILayout.IntField("Points to win ", PointsToWin.intValue);
        PointsPerRound.intValue = EditorGUILayout.IntField("Points win per Rounds", PointsPerRound.intValue);
        TransiTimeAfterRound.floatValue = EditorGUILayout.FloatField("Transition time in between rounds", TransiTimeAfterRound.floatValue);

        if (scene[0] == null) Scenes.GetArrayElementAtIndex(0).stringValue = null;
        if (scene!=null) scene = scene.Where(x => x != null).ToArray();
        if ( scene == null || scene.Length < 1 ) scene = new SceneAsset[1];
        for (int i = 0; i < scene.Length; i++)
        {
            if (scene[^1] != null)
            {
                Array.Resize(ref scene, scene.Length + 1);
            }
        }
        Scenes.arraySize = scene.Length;
        for (int i = 0; i < scene.Length; i++)
        {
            scene[i] = EditorGUILayout.ObjectField("Scene " + i, scene[i], typeof(SceneAsset), false) as SceneAsset;
            if (scene[i]!=null && i!= scene.Length-1) Scenes.GetArrayElementAtIndex(i).stringValue = scene[i].name;
        }
        objects.arraySize = scene.Length;
        for (int i = 0; i < scene.Length; i++)
        {
            objects.GetArrayElementAtIndex(i).objectReferenceValue = scene[i];
        }
        serializedObject.ApplyModifiedProperties();
    }
}
