using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    
    public static MenuManager Instance;

    public GameStateMachine gameStateMachine;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else Destroy(this.gameObject);
    }

    public void HideAllMenusExceptThis(GameObject menu)
    {
        foreach (Transform child in transform)
        {
            if (child.gameObject != menu)
            {
                child.gameObject.SetActive(false);
            }
        }
    }


}