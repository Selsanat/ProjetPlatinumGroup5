using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Pause : MonoBehaviour
{
    public GameObject pauseMenu;
    public bool _isPaused = false;

    public void onPause()
    {
        _isPaused = !_isPaused;
        if (_isPaused)
        {
            Time.timeScale = 0;
            GameObject ob = Instantiate(pauseMenu);
            GameObject.Find("Continue").GetComponent<Button>().onClick.AddListener(onPause);
        }
        else
        {
            Time.timeScale = 1;
            Destroy(pauseMenu);
        }
    }
    


    
}
