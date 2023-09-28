using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class selectionPerso : MonoBehaviour
{
    public GameObject perso1;
    public GameObject perso2;
    public GameObject perso3;
    public GameObject perso4;

    // Update is called once per frame
    void Update()
    {
        switch (Gamepad.all.Count)
        {
            case 0:
                break;
            case 1:
                perso1.GetComponentInChildren<Button>().Select();
                perso1.SetActive(true);
                break;
            case 2:
                perso2.SetActive(true);
                break;
            case 3:
                perso3.SetActive(true);
                break;
            case 4:
                perso4.SetActive(true);
                break;
                
        }

    }


}
