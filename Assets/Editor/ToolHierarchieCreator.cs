/*using UnityEngine;
using UnityEditor;



[InitializeOnLoad]
static class ToolHierarchieCreator
{
    //hierarchie On GUI 

    static ToolHierarchieCreator()
    {
        Undo.postprocessModifications += OnPostprocessModifications;
        
    }

    private static UndoPropertyModification[] OnPostprocessModifications(UndoPropertyModification[] modifications)
    {
        if (FindObjectOfType(typeof(ToolHierarchy)) == null)
        {
            GameObject newObject = new GameObject("Tool");
            newObject.AddComponent<ToolHierarchy>();
            newObject.tag = "Organisation";

        }
        return modifications;
    }

    static Object FindObjectOfType(System.Type type)
    {
        return Object.FindObjectOfType(type); 
    }
  

public class CustomToolbar
{
    static CustomToolbar()
    {
        EditorApplication.playModeStateChanged += OnPlayModeStateChanged;
    }

    static void OnPlayModeStateChanged(PlayModeStateChange state)
    {
        if (state == PlayModeStateChange.EnteredPlayMode)
        {
            // Créer un bouton dans la barre d'outils
            ToolbarCallback.OnToolbarGUI();
        }
    }
}

public class ToolbarCallback
{
    public static void OnToolbarGUI()
    {
        GUILayout.Button("Mon Bouton", EditorStyles.toolbarButton);
        if (GUILayout.Button("Mon Bouton", EditorStyles.toolbarButton))
        {
            // Action à exécuter lors du clic sur le bouton
            Debug.Log("Bouton cliqué !");
        }
    }
}

}
*/