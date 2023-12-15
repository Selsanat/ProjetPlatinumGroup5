using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.Rendering;
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
            //_quit.onClick.AddListener(() => onQuit());
            _continue.Select();

        }
        else
        {
            foreach (var player in RoundManager.Instance.alivePlayers)
            {
                //if(player._playerStateMachine. != null) 
                player._playerStateMachine._iMouvementLockedWriter.isMouvementLocked = false;
            }
            Time.timeScale = 1;
            _continue.onClick.RemoveAllListeners(); 
            _quit.onClick.RemoveAllListeners();
            _pauseMenu.SetActive(false);
        }
    }

    public void onQuit()
    {
        foreach (var player in RoundManager.Instance.alivePlayers)
        {
            //if(player._playerStateMachine. != null) 
            player._playerStateMachine._iMouvementLockedWriter.isMouvementLocked = false;
        }
        Time.timeScale = 1;
        _continue.onClick.RemoveAllListeners();
        _quit.onClick.RemoveAllListeners();
        _pauseMenu.SetActive(false);

        Volume vol = RoundManager.Instance.Volume;
        vol.profile.TryGet<ChromaticAberration>(out ChromaticAberration CA);
        CA.intensity.value = 0;

        RoundManager.Instance.DestroyAllPlayers();

        InputsManager.Instance.resetPlayers();

        RoundManager.Instance.alivePlayers.Clear();
        ManagerManager.Instance.characterSelector.Clear();
        foreach (BouleMouvement boule in GameObject.FindObjectsOfType<BouleMouvement>())
        {
            GameObject.Destroy(boule.gameObject);
        }
        foreach (GameObject gm in RoundManager.Instance.cadrants)
        {
            gm.SetActive(false);
        }
        foreach (var score in RoundManager.Instance.scores)
        {
            score.text = "";
        }
        foreach (var player in RoundManager.Instance.players)
        {
            player._playerStateMachine._iMouvementLockedWriter.isMouvementLocked = false;
        }

        SceneManager.LoadScene("LeandroMenu");

        GameStateMachine.Instance.ChangeState(GameStateMachine.Instance.menuState);

        
    }
    


    
}
