using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class OpenAIToolWindow : EditorWindow
{
    public OpenAITool tool = null;
    private OpenAIToolWindow window;
    [MenuItem("LeandroTools/ToolsTropStylésDeLaMortQuiTue")]
    public void initWindow()
    {
        window = (OpenAIToolWindow)EditorWindow.GetWindow(typeof(OpenAIToolWindow));
        window.titleContent = new GUIContent("ToolLeandroTropStylés");
        window.minSize = new Vector2(250, 500);
        window.Show();
    }

    private void OnGUI()
    {
        GUILayout.Label("Prompt :");
         tool.prompt = GUILayout.TextArea( tool.prompt, GUILayout.ExpandHeight(true), GUILayout.ExpandWidth(true));

        if (GUILayout.Button("TestPrompt"))
        {
            tool.stp();
        }

        GUILayout.Label("Prompt :");
        GUILayout.TextArea(tool.Out,  GUILayout.ExpandHeight(true), GUILayout.ExpandWidth(true));
    }

    void OnEnable()
        => AssemblyReloadEvents.afterAssemblyReload += OnAfterAssemblyReload;

    void OnDisable()
        => AssemblyReloadEvents.afterAssemblyReload -= OnAfterAssemblyReload;

    void OnAfterAssemblyReload()
    {
        if (!tool.TempFileExists) return;
        EditorApplication.ExecuteMenuItem("Edit/Do Task");
        AssetDatabase.DeleteAsset(tool.TempFilePath + "AIExCommandTest.cs");
    }
}