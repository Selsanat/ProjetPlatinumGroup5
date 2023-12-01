using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class playSoundOnSelect : MonoBehaviour
{
    public Button currentSelected;
    // Start is called before the first frame update
   

    private void Update()
    {
        if (currentSelected == null || currentSelected.gameObject.activeSelf == false||currentSelected != EventSystem.current.currentSelectedGameObject?.GetComponent<Button>() )
        {
            //currentSelected = EventSystem.current.currentSelectedGameObject.GetComponent<Button>();
            //SoundManager.instance.PlayClip("click Menu 1");
        }
    }

        
    public void setitslef()
    { 
        
    }




}
