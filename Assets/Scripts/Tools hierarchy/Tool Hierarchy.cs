using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

[ExecuteInEditMode]
public class ToolHierarchy : MonoBehaviour
{

    private List<string> categorie;
    public bool _setUp = false;
    public bool _reset = false;
    // Start is called before the first frame update


    // Update is called once per frame
    void Update()
    {
        
        if(_setUp)
        {
            changeCategorie();
            addCategorie();
            organiseCategorie();
            _setUp = false;
        }
        if(_reset)
        {
            resetCategorie();
            _reset = false;
        }
        
    }

    private void changeCategorie()
    {
        SerializedObject tagManager = new SerializedObject(AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/TagManager.asset")[0]);
        SerializedProperty tagsProp = tagManager.FindProperty("tags");

        for (int i = 0; i < tagsProp.arraySize; i++)
        {
            SerializedProperty t = tagsProp.GetArrayElementAtIndex(i);
            string str = t.stringValue;
            str = "------" + str + "------";
            if (GameObject.Find(str) == null)
            {
                categorie.Add(str);

            }

        }

        string str2 = "------Player------";
        if (GameObject.Find(str2) == null)
        {
            categorie.Add(str2);

        }
        str2 = "------Untagged------";
        if (GameObject.Find(str2) == null)
        {
            categorie.Add(str2);

        }
        






    }

    private void addCategorie()
    {
        foreach (string str in categorie) 
        {
            if (GameObject.Find(str) == null)
            {
                GameObject ob = new GameObject(str);
                ob.tag = "Organisation";
            }
            
        }
    }

    private void resetCategorie()
    {
        foreach(string str in categorie)
        {
            GameObject ob = GameObject.Find(str);
            if (ob != null)
            {
                ob.transform.DetachChildren();
                if(ob.transform.childCount == 0)
                    DestroyImmediate(ob);
            }

        }
        categorie.Clear();
    }
    private void organiseCategorie()
    {
        foreach (GameObject go in FindObjectsOfType<GameObject>())
        {
            if(go.transform.parent == null)
            {
                switch (go.tag)
                {
                    case "Player":
                        go.transform.parent = GameObject.Find("------Player------").transform;
                        break;
                    case "Manager":
                        go.transform.parent = GameObject.Find("------Manager------").transform;
                        break;
                    case "MainCamera":
                        go.transform.parent = GameObject.Find("------Camera------").transform;
                        break;
                    case "SpawnPoints":
                        go.transform.parent = GameObject.Find("------SpawnPoints------").transform;
                        break;
                    case "Untagged":
                        go.transform.parent = GameObject.Find("------Untagged------").transform;
                        break;
                    case "Team1":
                        go.transform.parent = GameObject.Find("------Team1------").transform;
                        break;
                    case "Team2":
                        go.transform.parent = GameObject.Find("------Team2------").transform;
                        break;
                    case "Teleporter":
                        go.transform.parent = GameObject.Find("------Teleporter------").transform;
                        break;
                    case "Cube":
                        go.transform.parent = GameObject.Find("------Cube------").transform;
                        break;
                    case "Organisation":
                        break;
                    case "Grid":
                        go.transform.parent = GameObject.Find("------Grid------").transform;
                        break;
                    default:
                        break;
                }
            }
            
        }
        
    }
}
