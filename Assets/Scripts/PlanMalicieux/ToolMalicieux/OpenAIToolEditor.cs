using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using Newtonsoft.Json.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem.HID;
using UnityEngine.InputSystem.Interactions;

[CustomEditor(typeof(OpenAITool))]
public class OpenAIToolEditor : Editor
{

    private OpenAITool _openAITool = null;
    public Rect windowRect = new Rect(10, 20, 120, 50);
    public Texture tex;

    private void OnEnable()
    {
        this._openAITool = (OpenAITool)this.target;
    }

    public override void OnInspectorGUI()
    {
        _openAITool.Key = GUILayout.TextField(_openAITool.Key);
        //_openAITool.parametersFunction = EditorGUILayout.ObjectField(_openAITool.parametersFunction, typeof(Object));
        if (GUILayout.Button("TestPrompt"))
        {
            OpenAIToolWindow window = new OpenAIToolWindow();
            window.tool = this._openAITool;
            window.initWindow();
        }
    }
}
