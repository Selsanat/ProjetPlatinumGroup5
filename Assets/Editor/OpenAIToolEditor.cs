using OpenAI_API.Chat;
using UnityEditor;
using UnityEngine;

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
            _openAITool.request ??= new ChatRequest();
            OpenAIToolWindow window = new OpenAIToolWindow();
            window.tool = this._openAITool;
            window.initWindow();
        }
    }
}
