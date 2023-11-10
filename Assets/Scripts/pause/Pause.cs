using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static System.Net.Mime.MediaTypeNames;

public class Pause : MonoBehaviour
{
    public GameObject _pauseMenu;
    public Button _continue;
    public Button _quit;
    private bool _isPaused = false;
    
    public void onPause()
    {
        if (GameStateMachine.Instance.CurrentState != GameStateMachine.Instance.roundState)
            return;

        
        _isPaused = !_isPaused;
        if (_isPaused)
        {
            Time.timeScale = 0;
            //GameObject ob = Instantiate(pauseMenu);

            _pauseMenu.SetActive(true);
            _continue.onClick.AddListener(() => onPause());
            _quit.onClick.AddListener(() => onQuit());
            _continue.Select();

        }
        else
        {
            Time.timeScale = 1;
            _continue.onClick.RemoveAllListeners();
            _quit.onClick.RemoveAllListeners();
            _pauseMenu.SetActive(false);
        }
    }

    public void onQuit()
    {
        _pauseMenu.SetActive(false);
        
    }
    


    
}
