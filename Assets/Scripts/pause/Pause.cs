using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static System.Net.Mime.MediaTypeNames;
using DG.Tweening;

public class Pause : MonoBehaviour
{
    public GameObject _pauseMenu;
    public Button _continue;
    public Button _quit;
    private bool _isPaused = false;
    Sequence mySequence;
    HorizontalLayoutGroup horizontalLayoutGroup;
    DepthOfField dof;



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
        #region SequenceSetter
        mySequence = DOTween.Sequence();
        horizontalLayoutGroup = ManagerManager.Instance.horizontalLayoutGroup;
        Volume vol = ManagerManager.Instance.Volume;
        vol.profile.TryGet<DepthOfField>(out dof);
        mySequence.Append(DOTween.To(() => dof.focalLength.value, x => dof.focalLength.value = x, 50, 0.5f));
        mySequence.Join(DOTween.To(() => horizontalLayoutGroup.padding.top, x => horizontalLayoutGroup.padding.top = x, 0, 0.5f));
        mySequence.Join(DOTween.To(() => horizontalLayoutGroup.spacing, x => horizontalLayoutGroup.spacing = x, -360, 0.5f));
        foreach (Transform child in horizontalLayoutGroup.transform)
        {
            mySequence.Join(child.DOScale(Vector3.one, 0.5f));
        }
        mySequence.AppendInterval(0.5f);
        mySequence.AppendCallback(() => RoundManager.Instance.UpdateScores());
        mySequence.AppendInterval(4f);
        #endregion

        mySequence.Play().OnComplete(() =>
        {
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
            SoundManager.instance.StopbackgroundMusic();
            SoundManager.instance.ResetValues();






        });
        }
    


    
}
