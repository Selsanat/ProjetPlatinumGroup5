/*using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Unity.Properties;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;


[ExecuteAlways]
public class ToolHierarchy : MonoBehaviour
{
    //hierarchy highliter pour les colrotds
    //attribute initialaze on load editor scene manager scene si load scene
    private List<string> categorie;
    public bool _setUp = false;
    public bool _reset = false;
    // Start is called before the first frame update

    void Start()
    {
        categorie = new List<string>();
        
    }
    // Update is called once per frame
    void Update()
    {
        
        if (_setUp)
        {
            addCategories();
            createGameObjectCategories();
            organiseCategorie();
            _setUp = false;
        }
        if(_reset)
        {
            resetCategorie();
            _reset = false;
        }
        
    }

    private void addCategories()
    {
        categorie.Clear();
        foreach (string str in UnityEditorInternal.InternalEditorUtility.tags)
        {
            string str3 = str;
            str3 = "------" + str3 + "------";
            categorie.Add(str3);

            
        }
        string str2;
        str2 = "------UI------";
        categorie.Add(str2);

        
    }

    private void createGameObjectCategories()
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
        categorie.Clear();
        addCategories();
        print(categorie.Count);
        foreach(string str in categorie)
        {
            print(str);
            GameObject ob = GameObject.Find(str);
            if (ob != null)
            {
                ob.transform.DetachChildren();
                if(ob.transform.childCount == 0)
                    UnityEngine.Object.DestroyImmediate(ob);
            }

        }
        categorie.Clear();
    }
    private void organiseCategorie()
    {
        foreach (GameObject go in SceneManager.GetActiveScene().GetRootGameObjects())
        {
            if(go.transform.parent == null)
            {
                if (go.layer == 5)
                {
                    go.transform.parent = GameObject.Find("------UI------").transform;
                    break;
                }
                switch (go.tag)
                {
                    case "Player":
                        go.transform.parent = GameObject.Find("------Player------").transform;
                        break;
                    case "Manager":
                        go.transform.parent = GameObject.Find("------Manager------").transform;
                        break;
                    case "MainCamera":
                        go.transform.parent = GameObject.Find("------MainCamera------").transform;
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
        foreach (string str in categorie)
        {
            GameObject ob = GameObject.Find(str);
            if (ob == null)
                return;
            if (ob.transform.childCount == 0)
            {
                UnityEngine.Object.DestroyImmediate(ob);
            }

        }

    }
}
*/