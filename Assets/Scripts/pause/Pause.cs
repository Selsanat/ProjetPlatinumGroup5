using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static System.Net.Mime.MediaTypeNames;

public class Pause : MonoBehaviour
{
    public GameObject pauseMenu;
    private bool _isPaused = false;
    
    public void onPause()
    {
        if (GameStateMachine.Instance.CurrentState != GameStateMachine.Instance.roundState)
            return;

        if(pauseMenu == null)
        {
            pauseMenu = GameObject.FindGameObjectWithTag("pause");
            print("pause null");
        }
        _isPaused = !_isPaused;
        if (_isPaused)
        {
            Time.timeScale = 0;
            //GameObject ob = Instantiate(pauseMenu);
            
            pauseMenu.SetActive(true);  
            GameObject.Find("Continue").GetComponent<Button>().onClick.AddListener(() => onPause());
            GameObject.Find("quit").GetComponent<Button>().onClick.AddListener(() => onQuit());
            GameObject.Find("Continue").GetComponent<Button>().Select();

        }
        else
        {
            Time.timeScale = 1;
            GameObject.Find("Continue").GetComponent<Button>().onClick.RemoveAllListeners();
            GameObject.Find("quit").GetComponent<Button>().onClick.RemoveAllListeners();
            pauseMenu.SetActive(false);
        }
    }

    public void onQuit()
    {
        GameStateMachine.Instance.ChangeState(GameStateMachine.Instance.menuState);
    }
    


    
}
