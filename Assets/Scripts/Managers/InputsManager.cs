using MoreMountains.Feedbacks;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;


public class InputsManager : MonoBehaviour
{
    // Start is called before the first frame update

    //Input Manager Singleton

    public static InputsManager Instance;
    public PlayerInputManager _playerInputManager => GetComponent<PlayerInputManager>();
    public List<PlayersInputs> playerInputs = new List<PlayersInputs>();
    public GameObject playerPrefab;

    private GameObject lastSelected;
    public class PlayersInputs
    {
        public PlayerInput _playerInputs;
        public PlayerStateMachine _playerStateMachine;
        
        public PlayersInputs(PlayerInput playerInputs, PlayerStateMachine playerStateMachine)
        {
            _playerInputs = playerInputs;
            _playerStateMachine = playerStateMachine;
        }
    }
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else Destroy(this.gameObject);
    }

    void Start()
    {
        _playerInputManager.playerPrefab = playerPrefab;
    }

    private void Update()
    {
        GameObject current = EventSystem.current.currentSelectedGameObject;
        if(GameStateMachine.Instance.CurrentState == GameStateMachine.Instance.menuState)
        {
        if(lastSelected == null)
        {
            lastSelected = current;
            current.transform.parent.GetComponent<MMWiggle>().RotationWiggleProperties.WigglePermitted = true;
            current.transform.parent.GetComponent<MMWiggle>().ScaleWiggleProperties.WigglePermitted = true;
/*
            current.GetComponent<MMPositionShaker>().enabled = true;
            current.GetComponent<MMPositionShaker>().ShakeRange = 10;*/
            return;
        }
        if (current != null && lastSelected != current)
        {
            lastSelected.transform.parent.GetComponent<MMWiggle>().RotationWiggleProperties.WigglePermitted = false;
            current.transform.parent.GetComponent<MMWiggle>().RotationWiggleProperties.WigglePermitted = true;

            lastSelected.transform.parent.GetComponent<MMWiggle>().ScaleWiggleProperties.WigglePermitted = false;
            current.transform.parent.GetComponent<MMWiggle>().ScaleWiggleProperties.WigglePermitted = true;
/*
            lastSelected.GetComponent<MMPositionShaker>().ShakeRange = 0;
            current.GetComponent<MMPositionShaker>().enabled = true;
            current.GetComponent<MMPositionShaker>().ShakeRange = 10;*/
            lastSelected = EventSystem.current.currentSelectedGameObject;
        }
        }
    }
    public void resetPlayers()
    {
        foreach(PlayerInput pi in GetComponents<PlayerInput>())
        {
            Destroy(pi);
        }
    }
}
