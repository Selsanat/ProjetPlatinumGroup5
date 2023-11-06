using UnityEngine;
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
}
